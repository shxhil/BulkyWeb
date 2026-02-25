using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;


        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId
                , includeProperty: "Product"),
                OrderHeader = new()

            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedonQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * (cart.Count);
            }

            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId
                , includeProperty: "Product"),
                OrderHeader = new()

            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(x => x.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedonQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * (cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Load cart items
            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperty: "Product"
            );

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            var applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            // 2. Calculate total amount
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedonQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            if (ShoppingCartVM.OrderHeader.OrderTotal <= 0)
                throw new Exception("Invalid order total: " + ShoppingCartVM.OrderHeader.OrderTotal);

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    #region OrderHeader
                    // 3. Set order header status
                    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
                    {
                        ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                        ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                    }
                    else
                    {
                        ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                        ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                    }

                    // 4. Save OrderHeader
                    _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                    _unitOfWork.Save();
                    #endregion OrderHeader >>

                    #region OrderDetails
                    // 5. Save Order Details (NO SAVE INSIDE LOOP)
                    foreach (var item in ShoppingCartVM.ShoppingCartList)
                    {
                        OrderDetail detail = new()
                        {
                            ProductId = item.ProductId,
                            OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                            Price = item.Price,
                            Count = item.Count
                        };
                        _unitOfWork.OrderDetail.Add(detail);
                    }
                    _unitOfWork.Save();
                    #endregion OrderDetails >>

                    #region Razorpay
                    // 6. If regular user, create Razorpay Order
                    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
                    {
                         
                        var razorSettings = _configuration.GetSection("Razorpay").Get<RazorpaySettings>();
                        RazorpayClient client = new RazorpayClient(razorSettings.Key, razorSettings.Secret);

                        Dictionary<string, object> options = new()
                {
                    { "amount", (int)(ShoppingCartVM.OrderHeader.OrderTotal * 100) },
                    { "currency", "INR" },
                    { "receipt", $"receipt_{ShoppingCartVM.OrderHeader.Id}" }
                };

                        Razorpay.Api.Order razorOrder = client.Order.Create(options);

                        // Save Razorpay Order ID
                        // Using PaymentIntentId column to store Razorpay OrderId (rename later)
                        ShoppingCartVM.OrderHeader.PaymentIntentId = razorOrder["id"].ToString();
                        _unitOfWork.Save();

                        transaction.Commit();

                        return RedirectToAction("Payment", new { id = ShoppingCartVM.OrderHeader.Id });
                    }

                    #endregion  Razorpay >>
                    // 7. Company user → No payment
                    transaction.Commit();
                    return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public IActionResult OrderConfirmation(int id)
        {
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var caretFromDB = _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
            caretFromDB.Count += 1;
            _unitOfWork.ShoppingCart.Update(caretFromDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int CartId)
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(x => x.Id == CartId);
            if (cartFromDB.Count <= 1)
            {
                //remove from the cart
                _unitOfWork.ShoppingCart.Remove(cartFromDB);
            }
            else
            {
                cartFromDB.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int CartId)
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(x => x.Id == CartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        private double GetPriceBasedonQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}

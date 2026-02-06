using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        //IActionResult , custom class or object includes all possible retun types in .net
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperty: "Category");
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperty: "Category"),
                ProductId = productId,
                Count = 1
            };

            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
           var claimsIdentity=(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId=userId;

            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });


        }

        // Only tocheck the Working of images in Sqlite
        //https://localhost:xxxx/Customer/Home/TestImageUrl
        public IActionResult TestImageUrl()
        {
            var products = _unitOfWork.Product.GetAll();

            foreach (var p in products)
            {
                Console.WriteLine($"ID:{p.Id}, ImageUrl:{p.ImageUrl}");
            }
            return Content("Check Output window(Debug) for ImageUrl Values");

        }
    }
}


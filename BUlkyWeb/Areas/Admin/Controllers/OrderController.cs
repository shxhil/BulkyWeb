using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderVM  = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(x => x.Id == id, includeProperty: "ApplicationUser"),
                OrderDetail=_unitOfWork.OrderDetail.GetAll(x=> x.OrderHeaderId == id, includeProperty:"product"),
            };
            
            return View(OrderVM);
        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetails(OrderVM orderVM)
        {
            var orderHeaderFromDB = _unitOfWork.OrderHeader
                .Get(x => x.Id == orderVM.OrderHeader.Id);

            orderHeaderFromDB.Name = orderVM.OrderHeader.Name;
            orderHeaderFromDB.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDB.StreetAddress = orderVM.OrderHeader.StreetAddress;
            orderHeaderFromDB.City = orderVM.OrderHeader.City;
            orderHeaderFromDB.State = orderVM.OrderHeader.State;
            orderHeaderFromDB.PostalCode = orderVM.OrderHeader.PostalCode;
            orderHeaderFromDB.ShipmentDate=orderVM.OrderHeader.ShipmentDate;

            if (!string.IsNullOrEmpty(orderVM.OrderHeader.Carrier))
                orderHeaderFromDB.Carrier = orderVM.OrderHeader.Carrier;

            if (!string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber))
                orderHeaderFromDB.TrackingNumber = orderVM.OrderHeader.TrackingNumber;

            _unitOfWork.OrderHeader.Update(orderHeaderFromDB);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully.";

            return RedirectToAction(nameof(Details), new { id = orderHeaderFromDB.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing(OrderVM orderVM)
        {
            var orderHeaderFromDB = _unitOfWork.OrderHeader
                .Get(x => x.Id == orderVM.OrderHeader.Id);
            if (orderHeaderFromDB == null)
                return NotFound();
            orderHeaderFromDB.OrderStatus = SD.StatusInProcess;
            _unitOfWork.OrderHeader.Update(orderHeaderFromDB);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";

            return RedirectToAction(nameof(Details), new { id = orderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            var orderHeaderFromDB = _unitOfWork.OrderHeader
                .Get(x => x.Id == orderVM.OrderHeader.Id);
            if (orderHeaderFromDB == null)
                return NotFound();
            orderHeaderFromDB.OrderStatus = SD.StatusInProcess;
            _unitOfWork.OrderHeader.Remove(orderHeaderFromDB);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Removed Successfully.";

            return RedirectToAction(nameof(Details));
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaderObj; 
            if(User.IsInRole(SD.Role_Admin)|| User.IsInRole(SD.Role_Employee))
            {
                orderHeaderObj = _unitOfWork.OrderHeader.GetAll(includeProperty: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderHeaderObj = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId== userId, includeProperty: "ApplicationUser");
            }

            switch (status)
            {
                case "inprocess":
                    orderHeaderObj = orderHeaderObj.Where(x => x.PaymentStatus == SD.StatusInProcess);
                    break;
                case "Pending":
                    orderHeaderObj = orderHeaderObj.Where(x => x.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "Completed":
                    orderHeaderObj = orderHeaderObj.Where(x => x.PaymentStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaderObj = orderHeaderObj.Where(x => x.PaymentStatus == SD.StatusApproved);
                    break;
                default:
                    break;

            }
            return Json(new { data = orderHeaderObj });
        }

        //[HttpPost]
        //public IActionResult Status(int Id)
        //{
        //    var status=_unitOfWork.OrderHeader.Get(x=> x.Id == Id)
        //}
    }
}

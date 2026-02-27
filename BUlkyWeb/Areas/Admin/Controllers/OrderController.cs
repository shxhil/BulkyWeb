using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
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
            OrderVM orderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(x => x.Id == id, includeProperty: "ApplicationUser"),
                OrderDetail=_unitOfWork.OrderDetail.GetAll(x=> x.OrderHeaderId == id, includeProperty:"product"),
            };
            
            return View(orderVM);
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaderObj = _unitOfWork.OrderHeader.GetAll(includeProperty: "ApplicationUser").ToList();

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

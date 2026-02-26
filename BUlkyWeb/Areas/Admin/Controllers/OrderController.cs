using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> orderHeaderObj = _unitOfWork.OrderHeader.GetAll(includeProperty: "ApplicationUser").ToList();
            return Json(new { data = orderHeaderObj });
        }
    }
}

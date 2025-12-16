using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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


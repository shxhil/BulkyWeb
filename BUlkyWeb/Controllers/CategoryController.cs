using BUlkyWeb.Data;
using BUlkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BUlkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;//local variable
        public CategoryController(ApplicationDbContext db)
        {// constructor is for db nn datas kittaan
            _db = db;
           // var check=db.Categories.ToList();
            //ivda db l varthana change neerit local variable lk idth vekkunnu
        }
        public IActionResult Index()
        {
           // var categoryList = _db.Categories.ToList();
           List<Category> categoryList = _db.Categories.ToList();
            return View(categoryList);
        }

        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create( Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }

            return View();
        }
    }
}

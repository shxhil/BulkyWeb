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
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                //ModelState.AddModelError("name", "The DisplayOrder Cannot Exactly match the same");
                ModelState.AddModelError("", "The DisplayOrder Cannot Exactly match the same");

            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category Created Successfully";
                
                                        //Action  ,Controller
                return RedirectToAction("Index", "Category");
            }

            return View();  
        }
        public IActionResult Edit(int? id)
        {
            if(id == null ||id == 0)
            {
                return NotFound();
            }
            Category? catobj = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (catobj == null)
            {
                return NotFound();
            }
            return View(catobj);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category Updated Successfully";

                //Action  ,Controller
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? catobj = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (catobj == null)
            {
                return NotFound();
            }
            return View(catobj);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Category Deleted Successfully";

            //Action  ,Controller
            return RedirectToAction("Index", "Category");
           
        }
    }
}

using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using BUlky.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace BUlkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;//local variable
        public CategoryController(ICategoryRepository db)
        {// constructor is for db nn datas kittaan
            _categoryRepository = db;
           // var check=db.Categories.ToList();
            //ivda db l varthana change neerit local variable lk idth vekkunnu
        }
        public IActionResult Index()
        {
           // var categoryList = _db.Categories.ToList();
           List<Category> categoryList = _categoryRepository.GetAll().ToList();
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
                _categoryRepository.Add(obj);
                _categoryRepository.Save();
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
            Category? catobj = _categoryRepository.Get(x=> x.Id==id);
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
                _categoryRepository.Update(obj); 
                _categoryRepository.Save();
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
            Category? catobj = _categoryRepository.Get(x=> x.Id==id);
            if (catobj == null)
            {
                return NotFound();
            }
            return View(catobj);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _categoryRepository.Get(x => x.Id == id);
            if(obj == null)
            {
                return NotFound();
            }
            _categoryRepository.Remove(obj);
            _categoryRepository.Save();
            TempData["Success"] = "Category Deleted Successfully";

            //Action  ,Controller
            return RedirectToAction("Index", "Category");
           
        }
    }
}

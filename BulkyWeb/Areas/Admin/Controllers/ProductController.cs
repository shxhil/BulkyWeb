using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> productObj = _unitOfWork.Product.GetAll().ToList();

            return View(productObj);
        }

        public IActionResult Create()
        {
            //Projection in EF => to convert from db to directly into EF
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new ProductVM()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };


            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["Success"] = "Product Saved Successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? prodObj = _unitOfWork.Product.Get(x => x.Id == id);
            if (prodObj == null)
            {
                return NotFound(prodObj);
            }
            return View(prodObj);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated Successfully";

                return RedirectToAction("Index", "Product");

            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            Product productobj = _unitOfWork.Product.Get(x => x.Id == id);
            return View(productobj);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product productobj = _unitOfWork.Product.Get(x => x.Id == id);
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Remove(productobj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Deleted Succesfully";
                return RedirectToAction("Index", "Product");


            }
            return View();

        }
    }
}

using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
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
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Saved Successfully";
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Product? prodObj = _unitOfWork.Product.Get(x => x.Id == id);
            if (prodObj == null)
            {
                return NotFound(prodObj);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated Successfully";


            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            Product productobj = _unitOfWork.Product.Get(x => x.Id == id);
            return View(productobj);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
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

            }
            return View();

        }
    }
}

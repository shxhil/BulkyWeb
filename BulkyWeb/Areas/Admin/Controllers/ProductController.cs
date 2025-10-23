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
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> productObj = _unitOfWork.Product.GetAll().ToList();

            return View(productObj);
        }

        //public IActionResult Create() 
        //{
        //    //Projection in EF => to convert from db to directly into EF
        //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
        //        .Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });
        //    ViewBag.CategoryList = CategoryList;
        //    ProductVM productVM = new ProductVM()
        //    {
        //        CategoryList = CategoryList,
        //        Product = new Product()
        //    };


        //    return View(productVM);
        //}
        //[HttpPost]
        //public IActionResult Create(ProductVM productvm)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(productvm.Product);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product Saved Successfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? prodObj = _unitOfWork.Product.Get(x => x.Id == id);
        //    if (prodObj == null)
        //    {
        //        return NotFound(prodObj);
        //    }
        //    return View(prodObj);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product productvm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(productvm);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product Updated Successfully";

        //        return RedirectToAction("Index", "Product");

        //    }
        //    return View();
        //}

        public IActionResult Upsert(int? id) //Update + Insert
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
            if(id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

                
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productvm, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productvm.Product.ImageUrl))
                    {
                        string oldImageUrl = Path.Combine(wwwRootPath, productvm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImageUrl))
                        {
                            System.IO.File.Delete(oldImageUrl);
                        }
                    }

                    //to save that image in folder
                    using( var fileStream = new FileStream(Path.Combine(productPath,fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productvm.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productvm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productvm.Product);
                    TempData["Success"] = "Product Saved Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productvm.Product);
                    TempData["Success"] = "Product Updated Successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productvm.CategoryList = _unitOfWork.Category.GetAll()
                    .Select(u => new SelectListItem
                    {
                         Text= u.Name,
                         Value=u.Id.ToString()
                    });
            }
            return View(productvm);
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

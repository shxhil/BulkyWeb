using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
   [Area("Admin")]
    //[Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> productObj = _unitOfWork.Company.GetAll().ToList();

            return View(productObj);
        }

        #region Create & Edit
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
        //        Company = new Company()
        //    };


        //    return View(productVM);
        //}
        //[HttpPost]
        //public IActionResult Create(ProductVM CompanyObj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Add(CompanyObj.Company);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Company Saved Successfully";
        //        return RedirectToAction("Index", "Company");
        //    }
        //    return View();
        //}

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? prodObj = _unitOfWork.Company.Get(x => x.Id == id);
        //    if (prodObj == null)
        //    {
        //        return NotFound(prodObj);
        //    }
        //    return View(prodObj);
        //}

        //[HttpPost]
        //public IActionResult Edit(Company CompanyObj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(CompanyObj);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Company Updated Successfully";

        //        return RedirectToAction("Index", "Company");

        //    }
        //    return View();
        //}
        #endregion

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
            if (id == null || id == 0)
            {
                //Create
                return View(new Company());
            }
            else
            {
                //Update
                Company CompanyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(CompanyObj);
            }


        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                    TempData["Success"] = "Company Saved Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                    TempData["Success"] = "Company Updated Successfully";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }
           
            return View(CompanyObj);
        }

        //public IActionResult Delete(int? id)
        //{
        //    Company productobj = _unitOfWork.Company.Get(x => x.Id == id);
        //    return View(productobj);
        //}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Company productobj = _unitOfWork.Company.Get(x => x.Id == id);
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Remove(productobj);
                _unitOfWork.Save();
                TempData["Success"] = "Company Deleted Succesfully";
                return RedirectToAction("Index", "Company");


            }
            return View();

        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> productObj = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = productObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var ProductToBeDeleted = _unitOfWork.Company.Get(x => x.Id == id);

            if (ProductToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error While Deleted" });
            }

            _unitOfWork.Company.Remove(ProductToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successfully" });
        }

        #endregion
    }
}

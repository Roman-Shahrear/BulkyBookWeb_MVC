using BulkyBook.DataAccess.Migrations;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
//using BulkyBook.Models;
using BulkyBook.Models.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;


//using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        //For Create
        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0)
            {
                // Create
                return View(new Company());
            }
            else
            {
                // Update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                    TempData["success"] = "Company created successfully!";
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                    TempData["success"] = "Company Updated successfully!";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }
        }

        ////For Edit
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //First Technique
        //    Company? CompanyFormDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //Second Technique for fency we can do it
        //    //Company? categoryFormDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //    //Third Technique
        //    //Company? categoryFormDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (CompanyFormDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFormDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company Updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);
        //}

        ////For Delete
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //First Technique
        //    Company? CompanyFormDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //Second Technique for fency we can do it
        //    //Company? categoryFormDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //    //Third Technique
        //    //Company? categoryFormDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (CompanyFormDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFormDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company Deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            if (objCompanyList == null || !objCompanyList.Any())
            {
                return Json(new { data = new List<Company>() });
            }
            return Json(new { data = objCompanyList });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();


            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

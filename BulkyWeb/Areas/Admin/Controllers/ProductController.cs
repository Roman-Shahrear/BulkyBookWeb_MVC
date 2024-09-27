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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objProductList);
        }

        //For Create
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            //    .GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM ProductVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                // Create
                return View(ProductVM);
            }
            else
            {
                // Update
                ProductVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(ProductVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM ProductVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string ProductPath = Path.Combine(wwwRootPath, @"images\Product");
                    
                    if(!string.IsNullOrEmpty(ProductVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));
                        
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(ProductPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    ProductVM.Product.ImageUrl = @"\images\Product\" + fileName;
                }
                if (ProductVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(ProductVM.Product);
                    TempData["success"] = "Product created successfully!";
                }
                else
                {
                    _unitOfWork.Product.Update(ProductVM.Product);
                    TempData["success"] = "Product Updated successfully!";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ProductVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(ProductVM);
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
        //    Product? ProductFormDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    //Second Technique for fency we can do it
        //    //Product? categoryFormDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //    //Third Technique
        //    //Product? categoryFormDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (ProductFormDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductFormDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Updated successfully";
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
        //    Product? ProductFormDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    //Second Technique for fency we can do it
        //    //Product? categoryFormDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //    //Third Technique
        //    //Product? categoryFormDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (ProductFormDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductFormDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            if (objProductList == null || !objProductList.Any())
            {
                return Json(new { data = new List<Product>() });
            }
            return Json(new { data = objProductList });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var ProductToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if(ProductToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                Path.Combine(_webHostEnvironment.WebRootPath,
                ProductToBeDeleted.ImageUrl.TrimStart('\\'));
            
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(ProductToBeDeleted);
            _unitOfWork.Save();

            
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

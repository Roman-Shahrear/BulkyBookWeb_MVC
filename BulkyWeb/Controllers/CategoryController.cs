using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
using Bulky.Models;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.ExceptionServices;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        //For Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder can not exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //For Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) {
                return NotFound();
            }
            //First Technique
            Category? categoryFormDb = _categoryRepo.Get(u=>u.Id == id);
            //Second Technique for fency we can do it
            //Category? categoryFormDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Third Technique
            //Category? categoryFormDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category Updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //For Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //First Technique
            Category? categoryFormDb = _categoryRepo.Get(u => u.Id == id);
            //Second Technique for fency we can do it
            //Category? categoryFormDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Third Technique
            //Category? categoryFormDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _categoryRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

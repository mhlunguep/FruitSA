using FruitSA.DataAccess.Repository.IRepository;
using FruitSA.Models;
using FruitSA.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FruitSA.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (ModelState.IsValid)
        {
            // Validate category code format
            if (!IsValidCategoryCode(obj.CategoryCode))
            {
                ModelState.AddModelError("CategoryCode", "Category code must contain 3 alphabet letters followed by 3 numeric characters.");
                return View(obj);
            }

            // Check for uniqueness of the category code
            if (!IsCategoryCodeUnique(obj.CategoryCode))
            {
                ModelState.AddModelError("CategoryCode", "Category code must be unique.");
                return View(obj);
            }

            // Set the user who created the category
            var userName = HttpContext.User.Identity.Name;
            obj.Username = userName;

            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    private bool IsValidCategoryCode(string categoryCode)
    {
        // Check if the category code is in the correct format (3 alphabet letters followed by 3 numeric characters)
        return Regex.IsMatch(categoryCode, @"^[A-Za-z]{3}\d{3}$");
    }

    private bool IsCategoryCodeUnique(string categoryCode)
    {
        // Check if there's any existing category with the same category code in the database
        return !_unitOfWork.Category.Any(c => c.CategoryCode == categoryCode);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == id);

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            // Validate category code format
            if (!IsValidCategoryCode(obj.CategoryCode))
            {
                ModelState.AddModelError("CategoryCode", "Category code must contain 3 alphabet letters followed by 3 numeric characters.");
                return View(obj);
            }

            // Set the user who updated the category
            var userName = HttpContext.User.Identity.Name;
            obj.Username = userName;

            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == id);

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryId == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");

    }
}

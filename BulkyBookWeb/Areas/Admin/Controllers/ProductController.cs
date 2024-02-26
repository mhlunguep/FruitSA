using FruitSA.DataAccess.Repository.IRepository;
using FruitSA.Models;
using FruitSA.Models.ViewModels;
using FruitSA.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;

namespace FruitSA.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            var categories = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            });

            var productVM = new ProductVM
            {
                CategoryList = categories // Assign your categories to the CategoryList property
            };

            if (id != null)
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.ProductId == id);
                                }
            else
            {
                productVM.Product = new Product();
                productVM.Product.ProductCode = GenerateProductCode(); // Generate product code for new product
            }

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Update the ImageUrl property of the product
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if (obj.Product.ProductId == 0) // New product
                {
                    // Generate new product code
                    obj.Product.ProductCode = GenerateProductCode();

                    // Set the user who created the product
                    var userName = HttpContext.User.Identity.Name;
                    obj.Product.Username = userName;

                    // Add the new product to the database
                    _unitOfWork.Product.Add(obj.Product);
                }
                else // Existing product
                {
                    // Retrieve the existing product from the database to update its values
                    var existingProduct = _unitOfWork.Product.GetFirstOrDefault(x => x.ProductId == obj.Product.ProductId);

                    if (existingProduct != null)
                    {
                        // Update existing product properties
                        existingProduct.Name = obj.Product.Name;
                        existingProduct.Description = obj.Product.Description;
                        existingProduct.Price = obj.Product.Price;
                        existingProduct.ImageUrl = obj.Product.ImageUrl;

                        // Update the product in the database
                        _unitOfWork.Product.Update(existingProduct);
                    }
                    else
                    {
                        // Handle the case where the existing product is not found
                        return NotFound();
                    }
                }

                // Save changes to the database
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }


        private string GenerateProductCode()
        {
            string yearMonth = DateTime.Now.ToString("yyyyMM");
            // Fetch the latest product code with the given yearMonth prefix
            var latestProduct = _unitOfWork.Product.GetAll().Where(p => p.ProductCode.StartsWith(yearMonth))
                                                        .OrderByDescending(p => p.ProductCode)
                                                        .FirstOrDefault();

            // If no product exists with the given yearMonth prefix, start with 001
            int sequenceNumber = 1;
            if (latestProduct != null)
            {
                // Extract the sequence number and increment it
                string sequenceStr = latestProduct.ProductCode.Substring(7);
                sequenceNumber = int.Parse(sequenceStr) + 1;
            }

            // Format the product code as yyyyMM-###
            string productCode = $"{yearMonth}-{sequenceNumber.ToString("D3")}";
            return productCode;
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = productList });
        }
        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.ProductId == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}

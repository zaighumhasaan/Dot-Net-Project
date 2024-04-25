﻿using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using IronBarCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
namespace Asp.NetProject.Controllers

{
    public class ProductController : Controller
    {
        private readonly PosContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
       
        public ProductController(PosContext context, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = context;
            webHostEnvironment = hostEnvironment;
        }

        #region List
        public IActionResult Index(int id)
        {
            
            int? StoreId = HttpContext.Session.GetInt32("StoreId");
            ViewBag.StoreId = StoreId;
            

            var department = _dbContext.Departments.FirstOrDefault(d => d.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            // Retrieve products belonging to the department
            TempData["depId"] = id;
            ViewBag.DepartmentId = id; 
            var products = _dbContext.Products.Where(p => p.DepartmentId == department.DepartmentId).ToList();

            ViewBag.SMessage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];

            return View(products);
        }

        #endregion List


        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SMessage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            ViewBag.ListCategories = _dbContext.ProductCategories.ToList();
            return View();  
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            try
            {
                int depId = (int) TempData["depId"];
                int? departmentId = HttpContext.Session.GetInt32("departmentId");
                
                if(departmentId==null)
                {
                    departmentId = depId;
                }

                if (departmentId != null)
                {
                    // Check if the product already exists in the department
                    var existingProduct = _dbContext.Products.FirstOrDefault(p => p.Name.ToUpper() == product.Name.ToUpper() && p.DepartmentId == departmentId);
                    if (existingProduct != null)
                    {
                        TempData["EMessage"] = product.Name + " product already exists in this department.";
                        return RedirectToAction("Create", "Product");
                    }

                    // Continue with creating the product
                    product.Name = product.Name.ToUpper();
                    product.Formula = product.Formula.ToUpper();
                    product.Description = product.Description.ToUpper();
                    product.CreatedAt = DateTime.Now;
                    product.ImagePath = UploadedFile(product);
                    product.DepartmentId = departmentId;

                    _dbContext.Products.Add(product);
                    _dbContext.SaveChanges();

                    TempData["SMessage"] = "Product created successfully.";
                    return RedirectToAction("Index", "Product",new { id= departmentId });
                }

                TempData["EMessage"] = "Please login again and try again.";
                return RedirectToAction("Create", "Product");
            }
            catch (Exception)
            {
                TempData["EMessage"] = "Some error occurred. Please try again later.";
            }
            return View();
        }

        #endregion Create


        #region Detail
        [HttpGet]
        public IActionResult Detail(int id)
        {
            try
            {
                var product = _dbContext.Products
                    .Include(p => p.Category) // Include the Category navigation property
                    .Include(p => p.Department) // Include the Department navigation property
                    .FirstOrDefault(p => p.ProductId ==id);
                TempData["depId"] = product.DepartmentId;
                ViewBag.DepartmentId = TempData["depId"];

                //Product product = _dbContext.Products.Find(id);
                if (product != null)
                {
                    return View(product);
                }

            }
            catch (Exception)
            {
                TempData["EMessage"] = "some error occured please try again lator !";
            }

            return View();
        }

        #endregion Detail

        #region Update

        [HttpGet]
        public IActionResult Update(int id)
        {

            try
            {

                Product product = _dbContext.Products.Find(id);

                if(product!=null)
                {
                    TempData["depId"] = product.DepartmentId;
                    ViewBag.DepartmentId = TempData["depId"];

                    ViewBag.ListCategories = _dbContext.ProductCategories.ToList();
                    return View(product);
                }
            }
            catch(Exception)
            {

            }
            return View();
        }
        [HttpPost]
        public IActionResult Update(Product product, string originalImage)
        {
            try
            {

             if(product !=null)
                {
                   // product.DepartmentId =(int) TempData["depId"];
                    if (product.ImageFile == null)
                    {
                        product.ImagePath = originalImage;
                    }
                    else
                    {
                        product.ImagePath = UploadedFile(product);
                    }



                    product.Name = product.Name.ToUpper();
                    product.Formula = product.Formula.ToUpper();
                    product.Description = product.Description.ToUpper();
                    product.UpdatedAt = DateTime.Now;
                    //  product.Barcode = GenerateBarcode(product);
                    _dbContext.Products.Update(product);
                    _dbContext.SaveChanges();
                    TempData["SMessage"] = "Success";
                    var department = _dbContext.Departments.FirstOrDefault(d => d.DepartmentId == product.DepartmentId);
                    

                    return RedirectToAction("Index", "Product", new { id = department.DepartmentId});

                }
                TempData["EMessage"] = "some error occured please try again lator !";
                return RedirectToAction("Index", "Product");


            }
            catch (Exception)
            {
                TempData["EMessage"] = "some error occured please try again lator !";
            }
            return View();
        }


        #endregion Update

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {


                Product prod = _dbContext.Products.Find(id);
                if (prod == null)
                {
                    TempData["SMessage"] = "Product not found";
                    return RedirectToAction("Index", "Product");
                }
                int departmentId = (int)prod.DepartmentId;
                // Check if there are any related sale lines
                var relatedSaleLines = _dbContext.SaleLines.Where(sl => sl.ProductId == id).ToList();
                if (relatedSaleLines.Any())
                {
                    // Handle related sale lines (e.g., delete them)
                    _dbContext.SaleLines.RemoveRange(relatedSaleLines);
                }

                _dbContext.Products.Remove(prod);
                _dbContext.SaveChanges();

                TempData["SMessage"] = "Product deleted successfully";
                return RedirectToAction("Index", "Product", new { id = departmentId });
            }
            catch (Exception ex)
            {
                TempData["SMessage"] = "An error occurred while deleting the product. Please try again later.";
                // Log the exception for debugging purposes
                // logger.LogError(ex, "An error occurred while deleting the product with ID {ProductId}", id);
                return RedirectToAction("Index", "Product");
            }
        }




        #endregion Delete


        #region Image Processing
        private string UploadedFile(Product model)
        {
            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Saving the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }

                // Returning the unique file name
                return uniqueFileName;
            }

            // If no file was uploaded, return null or appropriate default value
            return null;
        }
        #endregion Image Processing


        #region Generate Barcode


        //Pending

        #endregion Generate Barcode


        #region Exp
        public IActionResult ExpiredProduct(int id)
        {
            try
            {

                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

                var expiredProducts = _dbContext.Products
                    .Where(p => p.ExpDate.HasValue && p.ExpDate.Value < currentDate && p.DepartmentId == id)
                    .ToList();
                ViewBag.departmentId = id;

                    return View(expiredProducts);

                /*  var expiringProducts = _dbContext.Products
                     .Where(p => p.ExpDate.HasValue && p.ExpDate.Value < currentDate.AddDays(10) && p.DepartmentId == departmentId)
                     .ToList();
                */

            }
            catch (Exception)
            {

            }



            return View();
        }

        public IActionResult ExpiringProduct(int id)
        {
            try
            {

                if(id == 0)
                {
                    return View("some error occured please try again lator !");
                }
                ViewBag.departmentId = id;

                var currentDate = DateOnly.FromDateTime(DateTime.Now);
                var expirationThreshold = currentDate.AddDays(10);

                var expiringProducts = _dbContext.Products
                    .Where(p => p.ExpDate.HasValue && p.ExpDate.Value > currentDate && p.ExpDate.Value <= expirationThreshold && p.DepartmentId == id)
                    .ToList();

                return View(expiringProducts);

            }
            catch (Exception)
            {

            }



            return View();
        }
        #endregion Exp


    }
}

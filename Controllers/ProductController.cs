using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using IronBarCode;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            ViewBag.SMessage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];

            return View(_dbContext.Products.ToList());
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
                var existingProduct = _dbContext.Products.FirstOrDefault(p => p.Name == product.Name);
                if (existingProduct != null)
                {
                    TempData["EMessage"] = product.Name+" Product Alreadt Exists";
                    return RedirectToAction("Create", "Product");
                }


                int? departmentId = HttpContext.Session.GetInt32("departmentId");
                if(departmentId != null)
                {

                    product.Name = product.Name.ToUpper();
                    product.Formula = product.Formula.ToUpper();
                    product.Description = product.Description.ToUpper();
                    product.CreatedAt = DateTime.Now;
                  //  product.Barcode = GenerateBarcode(product);
                    product.ImagePath = UploadedFile(product);
                    product.DepartmentId = departmentId;
                    _dbContext.Products.Add(product);
                    _dbContext.SaveChanges();
                    TempData["SMessage"] = "Success";
                    return RedirectToAction("Create", "Product");


                }
                TempData["EMessage"] = "please login again and try again";
                return RedirectToAction("Create", "Product");

            }
            catch(Exception)
            {
                TempData["EMessage"] = "some error occured please try again lator !";
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

                Product product = _dbContext.Products.Find(id);
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
                    return RedirectToAction("Index", "Product");

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
                if (prod != null)
                {

                    _dbContext.Products.Remove(prod);
                    _dbContext.SaveChanges();
                    TempData["SMessage"] = "Deleted";
                    return RedirectToAction("Index", "Product");
                }
            }
            catch (Exception)
            {
                TempData["SMessage"] = "some error occured please try lator !";
            }

            return View();
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


    }
}

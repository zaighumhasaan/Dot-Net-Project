using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using IronBarCode;
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
     
        #endregion Generate Barcode


    }
}

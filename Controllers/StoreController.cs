using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Asp.NetProject.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PosContext _dbcontext;
        public StoreController(ILogger<HomeController> logger, PosContext dbcontext, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region item Management 
        [HttpGet]
        public IActionResult AddStore()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddStore(Store store)
        {
            try
            {

                store.Logo = UploadedFile(store);

                _dbcontext.Stores.Add(store);
                _dbcontext.SaveChanges();
                ViewBag.SMessage = "Data saved successfully";
            }
            catch (Exception ex)
            {
                ViewBag.EMessage = "Some Error Occurred"+ex;
            }
            return View();
        }
       


        private string UploadedFile(Store model)
        {
            if (model.LogoFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Saving the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.LogoFile.CopyTo(fileStream);
                }

                // Returning the unique file name
                return uniqueFileName;
            }

            // If no file was uploaded, return null or appropriate default value
            return null;
        }


        #endregion


    }
}

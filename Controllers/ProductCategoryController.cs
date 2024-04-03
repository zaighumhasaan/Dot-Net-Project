using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetProject.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly PosContext _dbContext;
        public ProductCategoryController(PosContext context)
        {
            _dbContext = context;
        }
        #region List
        public IActionResult Index()
        {
            ViewBag.SMessage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            return View(_dbContext.ProductCategories.ToList());
        }
        #endregion List


        #region Create 
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                ViewBag.SMessage = TempData["SMessage"];
                ViewBag.EMessage = TempData["EMessage"];
                return View();
            }
            catch(Exception)
            {

            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductCategory category)
        {
            try
            {
                var existingCategory = _dbContext.ProductCategories.FirstOrDefault(pc => pc.CategoryName==category.CategoryName);
                if(existingCategory==null)
                {
                    if (category != null)
                    {
                        category.CategoryName = category.CategoryName.ToUpper();
                        category.Description = category.Description.ToUpper();
                        category.CatCode = category.CatCode.ToUpper();
                        category.CatSerial = ("CAT / " + category.CatSerial);
                        category.CreatedBy = "Owner";
                        category.CreatedAt = DateTime.Now;
                        category.UpdatedAt = DateTime.Now;
                        _dbContext.ProductCategories.Add(category);
                        _dbContext.SaveChanges();
                        TempData["SMessage"] = "Success";

                        return RedirectToAction("Create", "ProductCategory");


                    }
                    TempData["EMessage"] = "some error occured please try again lator !";

                }
                TempData["EMessage"] = "category already exists  !";

                return RedirectToAction("Create", "ProductCategory");
            }
            catch (Exception)
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
                var category = _dbContext.ProductCategories.Find(id);
                if(category==null)
                {
                    return View();
                }
                else
                {
                    return View(category);
                }
            }
            catch(Exception)
            {
                return View("some error occured please try again lator !");
            }
            
        }


        #endregion Detail

        #region Update
        [HttpGet]
        public IActionResult Update(int id )
        {
            try
            {
                ViewBag.SMessage = TempData["SMessage"];
                ViewBag.EMessage = TempData["EMessage"];

                var category = _dbContext.ProductCategories.Find(id);
                if(category!=null)
                {
                    return View(category);
                }
               
            }
            catch(Exception)
            {

            }
            return View();
        }

        [HttpPost]
        public IActionResult Update(ProductCategory category)
        {
            try
            {
                var existingCategory = _dbContext.ProductCategories.FirstOrDefault(pc => pc.CategoryName == category.CategoryName);
                if(existingCategory==null)
                {
                    if (category != null)
                    {
                        category.CategoryName = category.CategoryName.ToUpper();
                        category.Description = category.Description.ToUpper();
                        category.CatCode = category.CatCode.ToUpper();
                        category.CatSerial = ("CAT / " + category.CatSerial);
                        category.CreatedBy = "Owner";
                        category.UpdatedAt = DateTime.Now;
                        _dbContext.ProductCategories.Update(category);
                        _dbContext.SaveChanges();
                        TempData["SMessage"] = "Success";

                        return RedirectToAction("Update", "ProductCategory");
                    }
                }
                TempData["EMessage"] = "category already exists";
                return RedirectToAction("Update", "ProductCategory");

            }
            catch(Exception)
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
                var category = _dbContext.ProductCategories.Find(id);
                if (category != null)
                {
                    _dbContext.ProductCategories.Remove(category);
                    _dbContext.SaveChanges();
                    TempData["SMessage"] = "Category Deleted Successfully";
                }
                else
                {
                    TempData["EMessage"] = "Record  not found";
                }
            }
            catch (Exception)
            {
                TempData["EMessage"] = "Some error occured";
            }
            return RedirectToAction("Index","ProductCategory");


        }
        #endregion Delete





    }
}

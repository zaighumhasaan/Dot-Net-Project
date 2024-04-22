using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PosContext _dbcontext;
        public DepartmentController(PosContext context, IWebHostEnvironment hostEnvironment)
        {
            _dbcontext = context;
            webHostEnvironment = hostEnvironment;
        }


        #region Create 

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SMessage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            int? storeId;
            storeId = HttpContext.Session.GetInt32("StoreId");
            ViewBag.StoreId = storeId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department obj)
        {
            try
            {
                int? storeId;
                storeId = HttpContext.Session.GetInt32("StoreId");
                if(storeId == null)
                {
                    ViewBag.EMessage = "some error occured please login again !";
                    return View();
                }
                else
                {
                    ViewBag.StoreId = storeId;
                    string name = obj.DepartmentName;
                    obj.DepartmentName = name.ToUpper();
                    
                    Department dep = _dbcontext.Departments.FirstOrDefault(d => d.DepartmentName == obj.DepartmentName);
                    if (dep == null)
                    {

                        obj.StoreId = storeId;
                        obj.DepartmentName = obj.DepartmentName.ToUpper();
                        obj.CreatedAt = DateTime.Now;
                        obj.UpdatedAt = DateTime.Now;
                        obj.Description = obj.Description.ToUpper();
                        _dbcontext.Departments.Add(obj);
                        _dbcontext.SaveChanges();
                        TempData["SMessage"] = "Success";

                        return RedirectToAction("Manage", "Store",new {id=storeId});
                    }
                    else
                    {
                        TempData["EMessage"] = "department with this name  already exists !";
                        return RedirectToAction("Create", "Department");
                    }


                }
 


            }
            catch(Exception)
            {
                TempData["EMessage"] = "some error occured please try again lator !";
            }





            return View();
        }


        #endregion Create 

        #region Detail

        public IActionResult Detail(int id)
        {

            try
            {
                Department dep = _dbcontext.Departments.Find(id);
                if (dep != null)
                {
                    int? storeId;
                    storeId = HttpContext.Session.GetInt32("StoreId");
                    ViewBag.StoreId = storeId;

                    int departmentId = dep.DepartmentId;
                    HttpContext.Session.SetInt32("departmentId", departmentId);
                    return View(dep);
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
                Department dep = _dbcontext.Departments.Find(id);
               
                if (dep != null)
                {

                    ViewBag.SMessage = TempData["SMessage"];
                    return View(dep);
                }
                else
                {

                    ViewBag.EMessage = TempData["EMessage"];
                    return View();
                }

            }
            catch(Exception)
            {
                ViewBag.EMessage = TempData["EMessage"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Update(Department dep)
        {
            try
            {
               

                if (dep!=null)
                {
                    dep.DepartmentName = dep.DepartmentName.ToUpper();
                    dep.Description = dep.Description.ToUpper();
                    dep.UpdatedAt = DateTime.Now;
              
                    _dbcontext.Departments.Update(dep);
                    _dbcontext.SaveChanges();
                    TempData["SMessage"] = "Updated";
                    //                    return RedirectToAction("Create","Department");
                    return RedirectToAction("Manage", "Store", new { id = dep.StoreId });
                }
                TempData["EMessage"] = "some error occured please try again lator !";
                return RedirectToAction("Create", "Department");
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
                Department dep = _dbcontext.Departments.Find(id);
                int storeId =(int) dep.StoreId;
                
                HttpContext.Session.SetInt32("OwnerId", storeId);
                if (dep!=null)
                {
                    
                    _dbcontext.Departments.Remove(dep);
                    _dbcontext.SaveChanges();
                    TempData["SMessage"] = "Deleted";
                    return RedirectToAction("Manage", "Store",new {id=storeId});
                }
            }
            catch(Exception)
            {

            }

            return View();
        }
        #endregion Delete



        public IActionResult Index()
        {
            ViewBag.SMessage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            return View(_dbcontext.Departments.ToList());
        }
    }
}

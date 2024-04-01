using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetProject.Controllers
{
    public class RoleController : Controller
    {

        private readonly PosContext _dbContext;
       public RoleController(PosContext context)
        {
            _dbContext = context;
        }

        #region Create Role

        [HttpGet]

        public IActionResult AddRole()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddRole(Role obj)
        {

            try
            {

                if(obj!=null)
                {
                    var existingRole = _dbContext.Roles.FirstOrDefault(r => r.RoleName == obj.RoleName);


                 if(existingRole==null)
                    {
                        obj.RoleName = obj.RoleName.ToUpper();
                        obj.Description = obj.Description.ToUpper();
                        _dbContext.Roles.Add(obj);
                        _dbContext.SaveChanges();
                        ViewBag.SMessage = "Success";
                        return View();
                    }
                    ViewBag.EMessage = obj.RoleName+" Role Already Exists ";
                    return View();


                }
                ViewBag.EMessage = "some error occured please try agin lator !";

            }
            catch(Exception)
            {
                ViewBag.EMessage = "some error occured please try agin lator !";
            }

            return View();
        }





        #endregion Create Role

        #region Detail

        [HttpGet]
        public IActionResult Detail(int id)
        {
            try
            {
                    Role obj = _dbContext.Roles.Find(id);
                    if(obj!=null)
                    {
                        return View(obj);
                    }


                


            }
            catch(Exception)
            {

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




                    return View(_dbContext.Roles.Find(id));
                
            }
            catch(Exception)
            {

            }

            return View();
        }

        [HttpPost]
        public IActionResult Update(Role obj)
        {
            try
            {
                if(obj!=null)
                {
                    obj.RoleName = obj.RoleName.ToUpper();
                    obj.Description = obj.Description.ToUpper();
                    _dbContext.Roles.Update(obj);
                    _dbContext.SaveChanges();

                    TempData["SMessage"]= "Success";
                    return RedirectToAction("Index", "Role");
                }
                TempData["EMessage"] = "some error occured please try again lator !";
                

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
                Role obj = _dbContext.Roles.Find(id);
                if (obj != null)
                {
                    _dbContext.Roles.Remove(obj);
                    _dbContext.SaveChanges();
                    TempData["SMessage"] = "Record Deleted Successfully";
                }
                else
                {
                    TempData["EMessage"] = "Record Store  not found";
                }
            }
            catch (Exception)
            {
                TempData["EMessage"] = "Some error occured";
            }
            return RedirectToAction(nameof(RoleController.Index));


        }

        #endregion Delete

        #region List
        public IActionResult Index()
        {
            ViewBag.SMessage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            return View(_dbContext.Roles.ToList());
        }
        #endregion List
    }
}

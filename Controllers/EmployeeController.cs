using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly PosContext _dbContext;
        public  EmployeeController(PosContext context)
        {
            _dbContext = context;

        }

        public IActionResult Index()
        {

            return View();
        }

        #region Create Employee

        public IActionResult Create()
        {
            ViewBag.Roles = _dbContext.Roles.ToList();
            return View();
        }


        public IActionResult Create(Employee obj) 
        {

            try
            {
                if(obj!=null)
                {
                    obj.FirstName = obj.FirstName.ToUpper();
                    obj.LastName = obj.LastName.ToUpper();
                    obj.Email = obj.Email.ToUpper();
                    obj.Address = obj.Address.ToUpper();
                    obj.City = obj.City.ToUpper();


                }
                return View("some error occured please try again lator !");

            }
            catch(Exception)
            {

            }

            return View();
        }

        #endregion Create Employee
    }
}

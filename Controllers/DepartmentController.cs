using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;

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






        public IActionResult Index()
        {
            return View();
        }
    }
}

using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Asp.NetProject.Controllers
{
    public class HomeController : Controller
    {
private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;//This is used in image uplloading
        private readonly PosContext _dbcontext;
        public HomeController(ILogger<HomeController> logger, PosContext dbcontext, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

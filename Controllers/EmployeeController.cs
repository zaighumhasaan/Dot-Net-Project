using Asp.NetProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly PosContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public  EmployeeController(PosContext context, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = context;
            webHostEnvironment = hostEnvironment;

        }





        public IActionResult Index()
        {
            
            int? storeId = HttpContext.Session.GetInt32("StoreId");

            var emplpyee = (from s in _dbContext.Employees
                          where s.StoreId == storeId
                            select s).ToList();

            return View(emplpyee);


        }

        #region Create Employee

        [HttpGet]
        public IActionResult Create()
        {
            var countries = new List<string>{

        "Afghanistan",
        "Albania",
        "Algeria",
        "Andorra",
        "Angola",
        "Antigua and Barbuda",
        "Argentina",
        "Armenia",
        "Australia",
        "Austria",
        "Azerbaijan",
        "Bahamas",
        "Bahrain",
        "Bangladesh",
        "Barbados",
        "Belarus",
        "Belgium",
        "Belize",
        "Benin",
        "Bhutan",
        "Bolivia",
        "Bosnia and Herzegovina",
        "Botswana",
        "Brazil",
        "Brunei",
        "Bulgaria",
        "Burkina Faso",
        "Burundi",
        "Côte d'Ivoire",
        "Cabo Verde",
        "Cambodia",
        "Cameroon",
        "Canada",
        "Central African Republic",
        "Chad",
        "Chile",
        "China",
        "Colombia",
        "Comoros",
        "Congo (Congo-Brazzaville)",
        "Costa Rica",
        "Croatia",
        "Cuba",
        "Cyprus",
        "Czechia (Czech Republic)",
        "Democratic Republic of the Congo",
        "Denmark",
        "Djibouti",
        "Dominica",
        "Dominican Republic",
        "Ecuador",
        "Egypt",
        "El Salvador",
        "Equatorial Guinea",
        "Eritrea",
        "Estonia",
        "Eswatini (fmr. \"Swaziland\")",
        "Ethiopia",
        "Fiji",
        "Finland",
        "France",
        "Gabon",
        "Gambia",
        "Georgia",
        "Germany",
        "Ghana",
        "Greece",
        "Grenada",
        "Guatemala",
        "Guinea",
        "Guinea-Bissau",
        "Guyana",
        "Haiti",
        "Holy See",
        "Honduras",
        "Hungary",
        "Iceland",
        "India",
        "Indonesia",
        "Iran",
        "Iraq",
        "Ireland",
        "Israel",
        "Italy",
        "Jamaica",
        "Japan",
        "Jordan",
        "Kazakhstan",
        "Kenya",
        "Kiribati",
        "Kuwait",
        "Kyrgyzstan",
        "Laos",
        "Latvia",
        "Lebanon",
        "Lesotho",
        "Liberia",
        "Libya",
        "Liechtenstein",
        "Lithuania",
        "Luxembourg",
        "Madagascar",
        "Malawi",
        "Malaysia",
        "Maldives",
        "Mali",
        "Malta",
        "Marshall Islands",
        "Mauritania",
        "Mauritius",
        "Mexico",
        "Micronesia",
        "Moldova",
        "Monaco",
        "Mongolia",
        "Montenegro",
        "Morocco",
        "Mozambique",
        "Myanmar (formerly Burma)",
        "Namibia",
        "Nauru",
        "Nepal",
        "Netherlands",
        "New Zealand",
        "Nicaragua",
        "Niger",
        "Nigeria",
        "North Korea",
        "North Macedonia",
        "Norway",
        "Oman",
        "Pakistan",
        "Palau",
        "Palestine State",
        "Panama",
        "Papua New Guinea",
        "Paraguay",
        "Peru",
        "Philippines",
        "Poland",
        "Portugal",
        "Qatar",
        "Romania",
        "Russia",
        "Rwanda",
        "Saint Kitts and Nevis",
        "Saint Lucia",
        "Saint Vincent and the Grenadines",
        "Samoa",
        "San Marino",
        "Sao Tome and Principe",
        "Saudi Arabia",
        "Senegal",
        "Serbia",
        "Seychelles",
        "Sierra Leone",
        "Singapore",
        "Slovakia",
        "Slovenia",
        "Solomon Islands",
        "Somalia",
        "South Africa",
        "South Korea",
        "South Sudan",
        "Spain",
        "Sri Lanka",
        "Sudan",
        "Suriname",
        "Sweden",
        "Switzerland",
        "Syria",
        "Tajikistan",
        "Tanzania",
        "Thailand",
        "Timor-Leste",
        "Togo",
        "Tonga",
        "Trinidad and Tobago",
        "Tunisia",
        "Turkey",
        "Turkmenistan",
        "Tuvalu",
        "Uganda",
        "Ukraine",
        "United Arab Emirates",
        "United Kingdom",
        "United States of America",
        "Uruguay",
        "Uzbekistan",
        "Vanuatu",
        "Venezuela",
        "Vietnam",
        "Yemen",
        "Zambia",
        "Zimbabwe"
    };

            ViewBag.Countries = countries;
            ViewBag.Roles = _dbContext.Roles.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee obj) 
        {

            try
            {
                int? storeId = HttpContext.Session.GetInt32("StoreId");

                if(storeId == null) 
                {

                    ViewBag.EMessage = "some error occured please try again lator !";
                    return View();


                }
                else
                {

                    if (obj != null)
                    {
                        obj.Image = UploadedFile(obj);
                        obj.CreatedAt = DateTime.Now;
                        obj.FirstName = obj.FirstName.ToUpper();
                        obj.LastName = obj.LastName.ToUpper();
                        obj.Email = obj.Email.ToUpper();
                        obj.Address = obj.Address.ToUpper();
                        obj.City = obj.City.ToUpper();
                        obj.StoreId = storeId;
                        obj.UpdatedAt = DateTime.Now;
                        _dbContext.Employees.Add(obj);
                        
                        _dbContext.SaveChanges();
                        ViewBag.SMessage = "Success";
                        //return View();
                      return RedirectToAction("Index", "Employee");

                    }
                    ViewBag.EMessage = "some error occured please try again lator !";
                }



            }
            catch(Exception)
            {

            }

            return View();
        }

        #endregion Create Employee

        #region Detail
        [HttpGet]
        public IActionResult Detail(int id)
        {
            try
            {


                    Employee obj = _dbContext.Employees.Find(id);


                    if(obj!=null)
                    {
                        return View(obj);
                    }


                

            }
            catch(Exception)
            {
                ViewBag.EMessage = " some error occured please try again lator !";
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
                if(id!=null)
                {
                    return View(_dbContext.Employees.Find(id));
                }
                else
                {
                    ViewBag.EMessage = " some error occured please try again lator !";
                }
            }
            catch(Exception)
            {
                ViewBag.EMessage = " some error occured please try again lator !";
            }


            return View();
        }

        [HttpPost]
        public IActionResult Update(Employee obj,string originalImage)
        {

            try
            {


                if (obj != null)
                {
                    if (obj.ImageFile == null)
                    {
                        obj.Image = originalImage;
                    }
                    else
                    {
                        obj.Image = UploadedFile(obj);
                    }


                    obj.CreatedAt = DateTime.Now;
                    obj.FirstName = obj.FirstName.ToUpper();
                    obj.LastName = obj.LastName.ToUpper();
                    obj.Email = obj.Email.ToUpper();
                    obj.Address = obj.Address.ToUpper();
                    obj.City = obj.City.ToUpper();
                    _dbContext.Employees.Add(obj);
                    _dbContext.SaveChanges();
                    ViewBag.SMessage = "Success";
                    //return View();
                    return RedirectToAction("Index", "Employee");


                    TempData["SMessage"] = "Data Updated Successfully";
                }




            }
            catch (Exception)
            {

            }

            return View();
        
        }

        #endregion Update




        private string UploadedFile(Employee model)
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
    }






    
}

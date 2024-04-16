﻿using Asp.NetProject.Controllers;
using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Cryptography;
using System.Text;

namespace Asp.NetProject.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PosContext _dbcontext;
        public OwnerController(PosContext context, IWebHostEnvironment hostEnvironment)
        {
            _dbcontext = context;
            webHostEnvironment = hostEnvironment;
        }

        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
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
            
            return View();

        }
        [HttpPost]
        public IActionResult SignUp(Owner owner)
        {

            try
            {

                var existingOwner = _dbcontext.Owners.FirstOrDefault(o => o.Email.ToUpper() == owner.Email.ToUpper());
                if (existingOwner != null)
                {
                    ViewBag.EMessage = "User with this email already exists!";
                    return View();
                }


                        owner.Password = HashPassword(owner.Password);
                        owner.Address = owner.Address.ToUpper();
                        owner.FirstName = owner.FirstName.ToUpper();
                        owner.LastName = owner.LastName.ToUpper();
                        owner.Email = owner.Email.ToUpper();
                        owner.City = owner.City.ToUpper();
                        owner.Country = owner.Country.ToUpper();
                        owner.Gender = owner.Gender.ToUpper();
                        owner.CreatedAt = DateTime.Now;

                        owner.Image = UploadedFile(owner);

                        _dbcontext.Owners.Add(owner);
                        _dbcontext.SaveChanges();
                        ViewBag.SMessage = "Account created";
                    

                




            }catch(Exception)
            {

                ViewBag.EMessage = "Some error occured please try again lator !";

            }
            return View();
        }

        #endregion SignUp
 
        #region Owner Login 

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Owner model)
        {
            try
            {
                Employee employee = _dbcontext.Employees.FirstOrDefault(emp =>emp.Email.ToUpper()==model.Email.ToUpper());
                var user = _dbcontext.Owners.FirstOrDefault(u => u.Email.ToUpper() == model.Email.ToUpper());
                if (employee != null)
                {
                    var pass = HashPassword(model.Password);

                    if (employee.Password == pass)
                    {
                        int empId = employee.EmployeeId;
                        // Set the Employee ID in session
                        HttpContext.Session.SetInt32("EmployeeId", empId);
                        return RedirectToAction("AddSale", "Sale");
                    }

                    ViewBag.EMessage = "Invalid email or password";
                    return View();


                }
                else if(user != null)
                {
                    var pass = HashPassword(model.Password);


                        if (user.Password == pass)
                        {
                            int ownerId = user.OwnerId;
                            // Set the owner ID in session
                            HttpContext.Session.SetInt32("OwnerId", ownerId);
                            return RedirectToAction("Index", "Store");
                        }

                        ViewBag.EMessage = "Invalid email or password";
                        return View();

                    


                }

                ViewBag.EMessage = "user with "+model.Email+" does not exists ";
                return View();



            }
            catch (Exception)
            {
                ViewBag.EMessage = "Some error occurred. Please try again later.";
               
            }
            return View();
        }

      


        #endregion Owner Login

        #region Owner Detail

        [HttpGet]
        public IActionResult OwnerDetail(int id)
        {
            try
            {
                Owner obj = _dbcontext.Owners.Find(id);
                if (obj != null)
                {
                    return View(obj);
                }

            }
            catch (Exception)
            {

            }


            return View();
        }
        #endregion Owner Detail



        public IActionResult Index()
        {

            ViewBag.SMeesage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];


            return View(_dbcontext.Owners.ToList());
        }

        #region delete Owner


        [HttpGet]
        public IActionResult DeleteOwner(int id)
        {
            try
            {
                Owner owner = _dbcontext.Owners.Find(id);
                if (owner != null)
                {
                    _dbcontext.Owners.Remove(owner);
                    _dbcontext.SaveChanges();
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
            return RedirectToAction(nameof(OwnerController.SignUp));


        }



        #endregion delete Owner



        


        #region Owner update

        [HttpGet]
        public IActionResult UpdateOwner(int id)
        {
            try
            {

                ViewBag.SMeesage = TempData["SMessage"];
                ViewBag.EMessage = TempData["EMessage"];

                Owner obj = _dbcontext.Owners.Find(id);
                if (obj == null)
                {
                    return View("No Record Found");
                }
                return View(obj);



            }
            catch (Exception)
            {
                ViewBag.EMessage = "some error occured !";
            }

            return View();

        }

        [HttpPost]
        public IActionResult UpdateOwner(Owner obj, string originalImage)
        {

            try
            {


                if (obj != null)
                {



                    if (obj.LogoFile == null)
                    {
                        obj.Image = originalImage;
                    }
                    else
                    {
                        obj.Image = UploadedFile(obj);
                    }




                    obj.Password = HashPassword(obj.Password);
                    
                    obj.UpdatedAt = DateTime.Now;
                    _dbcontext.Owners.Update(obj);
                    _dbcontext.SaveChanges();
                    TempData["SMessage"] = "Data Updated Successfully";
                }


            }
            catch (Exception)
            {

                TempData["EMessage"] = "Some error occured. please try again!";

            }


            return RedirectToAction("Index", "Store", new { id = obj.OwnerId });
        }
        #endregion store update

        private static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string UploadedFile(Owner model)
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
    }
}

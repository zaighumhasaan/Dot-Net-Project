using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Asp.NetProject.Controllers
{
    public class StoreController : Controller
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PosContext _dbcontext;
        public StoreController(PosContext dbcontext, IWebHostEnvironment hostEnvironment)
        {

            _dbcontext = dbcontext;
            webHostEnvironment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.SMeesage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            int? ownerId = HttpContext.Session.GetInt32("OwnerId");
            ViewBag.OwnerId = ownerId;
            var stores = (from s in _dbcontext.Stores
                          where s.OwnerId == ownerId
                          select s).ToList();

            return View(stores);
        }
        [HttpGet]
        public IActionResult Home(int id)
        {
            ViewBag.SMeesage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            ViewBag.OwnerId = id;
            var stores = (from s in _dbcontext.Stores
                          where s.OwnerId == id
                          select s).ToList();

            return View(stores);
        }


        #region store creation 
        [HttpGet]
        public IActionResult AddStore()
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
        public IActionResult AddStore(Store store)
        {
            try
            {

                int? ownerId = HttpContext.Session.GetInt32("OwnerId");
                if (ownerId != null)
                {

                    store.OwnerId = ownerId;
                    store.Logo = UploadedFile(store);
                    store.StoreName = store.StoreName.ToUpper();
                    store.Description = store.Description.ToUpper();
                    store.Location = store.Location.ToUpper();
                    store.City = store.City.ToUpper();
                    store.Email = store.Email.ToUpper();
                    store.Website = store.Website.ToUpper();

                    _dbcontext.Stores.Add(store);
                    _dbcontext.SaveChanges();
                    ViewBag.SMessage = "Data saved successfully";
                    TempData["SMessage"] = "Data saved successfully";
                    return View();
                }


                ViewBag.EMessage = "some error occured login again and retry !";



            }
            catch (Exception)
            {
                ViewBag.EMessage = "Some Error Occurred";
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
        #endregion store creation

        #region store detail

        [HttpGet]
        public IActionResult StoreDetail(int id)
        {
            try
            {
                Store store = _dbcontext.Stores.Find(id);
                if (store != null)
                {
                    int storeId = store.StoreId;

                    HttpContext.Session.SetInt32("StoreId", storeId);

                    return View(store);
                }

            }
            catch (Exception)
            {

            }


            return View();
        }



        #endregion store detail


        #region store update

        [HttpGet]
        public IActionResult UpdateStore(int id)
        {
            try
            {

                ViewBag.SMeesage = TempData["SMessage"];
                ViewBag.EMessage = TempData["EMessage"];

                Store obj = _dbcontext.Stores.Find(id);
                if (obj == null)
                {
                    return View("No Record Found");
                }
                return View(obj);



            }
            catch (Exception)
            {
                return View();
            }



        }
        [HttpPost]
        public IActionResult UpdateStore(Store obj, string originalImage)
        {
            try
            {
                if (ModelState.IsValid) // Ensure model state is valid
                {
                    // Retrieve the existing store object from the database
                    Store existingStore = _dbcontext.Stores.Find(obj.StoreId);
                    if (existingStore == null)
                    {
                        return NotFound(); // Return 404 if store not found
                    }

                    // Update properties of the existing store with values from the form
                    existingStore.StoreName = obj.StoreName;
                    existingStore.Description = obj.Description;
                    existingStore.Location = obj.Location;
                    existingStore.City = obj.City;
                    existingStore.Country = obj.Country;
                    existingStore.Phone = obj.Phone;
                    existingStore.Email = obj.Email;
                    existingStore.Website = obj.Website;
                    existingStore.OpeningHours = obj.OpeningHours;
                    existingStore.UpdatedAt = DateTime.Now;

                    // Update logo only if a new file is uploaded
                    if (obj.LogoFile != null)
                    {
                        existingStore.Logo = UploadedFile(obj);
                    }

                    // Save changes to the database
                    _dbcontext.SaveChanges();

                    TempData["SMessage"] = "Data Updated Successfully";
                    return RedirectToAction("Index", "Store");
                }
            }
            catch (Exception)
            {
                TempData["EMessage"] = "Some error occurred. Please try again!";
            }

            return View(obj); // Return the form with errors if ModelState is not valid
        }

        /*[HttpPost]
        public IActionResult UpdateStore(Store obj, string originalImage)
        {
            try
            {

                if (obj != null)
                {
                    if (obj.LogoFile == null)
                    {
                        obj.Logo = originalImage;
                    }
                    else
                    {
                        obj.Logo = UploadedFile(obj);
                    }



                    obj.UpdatedAt = DateTime.Now;
                    Store updatedStore = obj;
                    _dbcontext.Stores.Update(updatedStore);
                    _dbcontext.SaveChanges();
                    TempData["SMessage"] = "Data Updated Successfully";
                    return View();
                }
            }
            catch (Exception)
            {
                TempData["EMessage"] = "Some error occurred. Please try again!";
            }

            return RedirectToAction("Index", "Store");

        }
        */
        #endregion store update



        #region delete Store


        [HttpGet]
        public IActionResult DeleteStore(int id)
        {
            try
            {
                Store store = _dbcontext.Stores.Find(id);
                if (store != null)
                {
                    _dbcontext.Stores.Remove(store);
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
            return RedirectToAction(nameof(StoreController.Index));


        }



        #endregion delete Store 

    }
}
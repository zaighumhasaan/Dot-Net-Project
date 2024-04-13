﻿using Asp.NetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Asp.NetProject.Controllers
{
    public class SaleController : Controller
    {

        private readonly PosContext _dbContext;

       public SaleController(PosContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Create

        [HttpGet]
        public IActionResult AddSale()
        {

            ViewBag.SMeesage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            ViewBag.ListProducts = _dbContext.Products.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddSale(string objData)
        {



            
            try
            {
                int? employeeId = HttpContext.Session.GetInt32("EmployeeId");

                if(employeeId!=null)
                {
                    ViewSales objMain = JsonConvert.DeserializeObject<ViewSales>(objData, new IsoDateTimeConverter());
                    var exSale = _dbContext.Sales.OrderByDescending(s => s.SaleId).FirstOrDefault();
                    objMain.objSale.CreatedAt = DateTime.Now;
                    objMain.objSale.EmployeeId = employeeId;
                    _dbContext.Sales.Add(objMain.objSale);
                    _dbContext.SaveChanges();
                    var obj = objMain.objSale;
                    var totalPrice = 0;
                    var saleid = objMain.objSale.SaleId;
                    foreach (SaleLine product in objMain.ListSaleLine)
                    {

                        product.SaleId = saleid;
                        product.CreatedAt = DateTime.Now;
                        Product objProduct = _dbContext.Products.Find(product.ProductId);
                        product.UnitPrice = objProduct.Price;

                        product.Quantity = product.Quantity;
                        product.TotalPrice = product.UnitPrice * product.Quantity;

                        _dbContext.SaleLines.Add(product);
                        _dbContext.SaveChanges();


                        objProduct.StockQuantity = objProduct.StockQuantity - product.Quantity;
                        totalPrice = (int)(objProduct.Price * product.Quantity);
                        _dbContext.Products.Update(objProduct);
                        _dbContext.SaveChanges();

                    }
                    objMain.objSale.TotalAmount = totalPrice;
                    _dbContext.Sales.Update(objMain.objSale);
                    _dbContext.SaveChanges();

                    TempData["SMessage"] = "Data Updated Successfully";
                    return RedirectToAction("AddSale");
                }
                TempData["SMessage"] = "some error occured please login again";
                return RedirectToAction(nameof(OwnerController.Login));
            }
            catch (Exception)
            {
                TempData["EMessage"] = "Some error occured. please try again";
            }


            return RedirectToAction(nameof(SaleController.AddSale));
        }


        #endregion Create

        #region Ajax

        public ActionResult GetProductPrice(int productId)
        {

            Product product = _dbContext.Products.Find(productId);
            var productPrice =product.Price;
    return Json(productPrice);
        }

        public ActionResult GetProductQty(int productId)
        {

            Product product = _dbContext.Products.Find(productId);
            var productqty = product.StockQuantity;
            return Json(productqty);
        }

        #endregion Ajax 


        public IActionResult Index()
        {
            return View();
        }
    }
}

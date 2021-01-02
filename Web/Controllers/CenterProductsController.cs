using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TemplateNBL.Helpers;
using TemplateNBL.Models;
using TemplateNBL.ViewModel;

namespace TemplateNBL.Controllers
{
    public class CenterProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string ModuleDescription = "Menu Principal";
        public string WindowDescription = "Stock";
        public string className = "CenterProduct";

        public ActionResult Index()
        {
            ViewBag.Title = "Stock de Centros ";
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());            
            //Si el usuario es administrador se muestra la grilla, sino solo se muestra
            if (!usuario.Rol.IsAdmin)
            {
                return RedirectToAction("Details", new { id = usuario.CenterId });
            }
            else               
                return View();
        }

            // GET: CenterProducts
        public ActionResult Details(int id)
        {

            CenterProductStockViewModel stockvm = new CenterProductStockViewModel();
            stockvm.ProductStockViewModels = new List<ProductStockViewModel>();
            stockvm.CenterId = id;
            Usuario usuario = db.Usuarios.Find(SessionHelper.GetUser());

            ViewBag.Title = "Stock del Centro " + usuario.Center?.Descripcion;
            ViewBag.isAdmin = usuario.Rol.IsAdmin;
            List <Product> products = db.Products.Where(p => p.Enable).ToList();
            List<CenterProduct> centerProducts = db.CenterProducts.Where(x => x.CenterId==id).ToList();
            
            foreach (var item in products)
            {
                ProductStockViewModel productStockViewModel = new ProductStockViewModel();
                CenterProduct centerProduct = centerProducts.Where(x => x.ProductId == item.Id).FirstOrDefault();
                if (centerProducts.Any(x => x.ProductId == item.Id))
                {
                    productStockViewModel.Stock = centerProduct.Stock;
                }
                else
                {
                    productStockViewModel.Stock = 0;
                }

                productStockViewModel.ProductId = item.Id;
                productStockViewModel.Product = item;

                stockvm.ProductStockViewModels.Add(productStockViewModel);

            }


            return View(stockvm);
        }


        [HttpPost]
        public JsonResult GetCenters()
        {
            try
            {
                List<CenterProductsStockIndexViewModel> list = new List<CenterProductsStockIndexViewModel>();
                List<Center> centers = db.Centers.Where(x=>x.Enable).Take(1000).ToList();



                foreach (var item in centers)
                {
                    CenterProductsStockIndexViewModel centerIndexViewModel = new CenterProductsStockIndexViewModel
                    {
                        Centro = item.Descripcion,
                        Id = item.Id

                    };
                    list.Add(centerIndexViewModel);
                }


                return Json(list.OrderByDescending(x => x.Centro), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public JsonResult Create (CenterProductStockViewModel order)
        {
            var responseObject = new
            {
                responseCode = -1
            };

            if (order == null)
            {
                return Json(new { responseCode = "-10" });
            }

            try
            {
                int centerId = order.CenterId;
                List<CenterProduct> centerProducts_add = new List<CenterProduct>();
                List<CenterProduct> centerProducts_upd = new List<CenterProduct>();
                List<CenterProduct> centerProducts_dtl = new List<CenterProduct>();
                List<CenterProduct> centerProductsDB = db.CenterProducts.Where(c => c.CenterId == centerId).ToList();               

                //Estos son los que se modificaron o agregaron
                foreach (var item in order.ProductStockViewModels)
                {
                    var centerProductoDB = centerProductsDB.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

                    CenterProduct centerProduct = new CenterProduct
                    {
                        CenterId = centerId,
                        ProductId = item.ProductId,
                        Stock = item.Stock
                    };

                    //Si el producto ya estaba dado de alta lo modifico
                    if (centerProductoDB!= null)
                    {
                        centerProduct.Id = centerProductoDB.Id;
                        centerProducts_upd.Add(centerProduct);
                    }
                    else
                        //Sino lo doy de alta
                        centerProducts_add.Add(centerProduct);

                }
                //Ahora faltan los que ya no vienieron

                List<int> tempIdList = centerProducts_upd.Select(x => x.Id).ToList();

                centerProducts_dtl = centerProductsDB.Where(q => !tempIdList.Contains(q.Id)).ToList();

                foreach (var item in centerProducts_dtl)
                {
                    CenterProduct centerProduct = new CenterProduct
                    {
                        CenterId = centerId,
                        ProductId = item.ProductId,
                        Stock = 0,
                        Id = item.Id

                    };
                    centerProducts_upd.Add(centerProduct);
                }


                db.CenterProducts.AddRange(centerProducts_add);
                
                foreach (var model in centerProducts_upd)
                {
                    CenterProduct centerProduct = db.CenterProducts.Find(model.Id);
                    centerProduct.Stock = model.Stock;
                    db.Entry(centerProduct).State = EntityState.Modified;
                }

                db.SaveChanges();

                responseObject = new
                {
                    responseCode = 0
                };
            }
            catch (Exception e)
            {
                responseObject = new
                {
                    responseCode = -1
                };

            }

            return Json(responseObject);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

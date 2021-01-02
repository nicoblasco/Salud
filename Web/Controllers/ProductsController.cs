using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TemplateNBL.Helpers;
using TemplateNBL.Models;
using TemplateNBL.ViewModel;

namespace TemplateNBL.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();       

        public string ModuleDescription = "ABM Maestros";
        public string WindowDescription = "Productos";
        public string className = "Products";
        // GET: Products
        public ActionResult Index()
        {
            if (!PermissionViewModel.TienePermisoAcesso(WindowHelper.GetWindowId(ModuleDescription, WindowDescription)))
                return View("~/Views/Shared/AccessDenied.cshtml");
            ViewBag.AltaModificacion = PermissionViewModel.TienePermisoAlta(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
            ViewBag.Baja = PermissionViewModel.TienePermisoBaja(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
            List<ProductType> lProductTypes = new List<ProductType>();
            List<SupplieMedical> lSupplieMedical = new List<SupplieMedical>();
            lProductTypes = db.ProductTypes.Where(x=>x.Enable).ToList();
            lSupplieMedical = db.SupplieMedicals.Where(x => x.Enable).ToList();
            ViewBag.listaProductType = lProductTypes;
            ViewBag.listaSupplieMedical = lSupplieMedical;
            return View(db.Products.ToList());
        }

        [HttpPost]
        public JsonResult Gets()
        {
            List<Product> list = new List<Product>();
            try
            {
                list = db.Products.ToList();

                List<ProductViewModel> modelViewModels = new List<ProductViewModel>();
                foreach (var item in list)
                {
                    ProductViewModel vm = new ProductViewModel
                    {
                        Descripcion = item.Descripcion,
                        OutStock = item.OutStock == true ? "SI" : "NO",
                        Id = item.Id,
                        Presentation = item.Presentation,
                        ProductTypeId = item.ProductTypeId,
                        Stock= item.Stock,
                        StockMin= item.StockMin,
                        SupplieMedicalId = item.SupplieMedicalId,
                        ProductType = item.ProductType,
                        SupplieMedical = item.SupplieMedical

                    };
                    modelViewModels.Add(vm);
                }

                return Json(modelViewModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;

            }
        }


        [HttpPost]
        public JsonResult Get(int id)
        {

            try
            {
                var modelo = db.Products.Where(x => x.Id == id).Select(c => new { c.Id, c.Descripcion, c.Presentation, c.Stock, c.StockMin, c.ProductType, c.SupplieMedical, c.SupplieMedicalId, c.ProductTypeId, c.OutStock }).FirstOrDefault();

                Product types = new Product
                {
                    Id = modelo.Id,
                    Descripcion = modelo.Descripcion,
                    Presentation = modelo.Presentation,
                    ProductTypeId = modelo.ProductTypeId,
                    ProductType = modelo.ProductType,
                    SupplieMedicalId = modelo.SupplieMedicalId,
                    SupplieMedical = modelo.SupplieMedical,
                    Stock = modelo.Stock,
                    StockMin = modelo.StockMin,
                    OutStock = modelo.OutStock
                };

                return Json(types, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDuplicates(int id, string descripcion)
        {

            try
            {
                var result = from c in db.Products
                             where c.Id != id
                             && c.Descripcion.ToUpper() == descripcion.ToUpper() && c.Enable == true
                             select c;

                var responseObject = new
                {
                    responseCode = result.Count()
                };

                return Json(responseObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult Create(Product modelo)
        {
            if (modelo == null)
            {
                return Json(new { responseCode = "-10" });
            }

            modelo.Enable = true;
            db.Products.Add(modelo);
            db.SaveChanges();

            //Audito
            AuditHelper.Auditar("Alta", modelo.Id.ToString(), className, ModuleDescription, WindowDescription);

            var responseObject = new
            {
                responseCode = 0
            };

            return Json(responseObject);
        }


        public JsonResult Edit(Product modelo)
        {
            if (modelo == null)
            {
                return Json(new { responseCode = "-10" });
            }
            modelo.Enable = true;
            db.Entry(modelo).State = EntityState.Modified;
            db.SaveChanges();

            //Audito
            AuditHelper.Auditar("Modificacion", modelo.Id.ToString(), className, ModuleDescription, WindowDescription);

            var responseObject = new
            {
                responseCode = 0
            };

            return Json(responseObject);

        }

        public JsonResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(new { responseCode = "-10" });
            }

            Product modelo = db.Products.Find(id);
            modelo.Enable = false;
            db.Entry(modelo).State = EntityState.Modified;
            db.SaveChanges();

            //Audito
            AuditHelper.Auditar("Baja", id.ToString(), className, ModuleDescription, WindowDescription);

            var responseObject = new
            {
                responseCode = 0
            };

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

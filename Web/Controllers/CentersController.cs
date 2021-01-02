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
    public class CentersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string ModuleDescription = "ABM Maestros";
        public string WindowDescription = "Centros";
        public string className = "Centers";

        // GET: SupplieMedicals
        public ActionResult Index()
        {
            if (!PermissionViewModel.TienePermisoAcesso(WindowHelper.GetWindowId(ModuleDescription, WindowDescription)))
                return View("~/Views/Shared/AccessDenied.cshtml");
            ViewBag.AltaModificacion = PermissionViewModel.TienePermisoAlta(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
            ViewBag.Baja = PermissionViewModel.TienePermisoBaja(WindowHelper.GetWindowId(ModuleDescription, WindowDescription));
            return View(db.Centers.ToList());
        }

        [HttpPost]
        public JsonResult Gets()
        {
            try
            {
                var list = db.Centers.Where(x => x.Enable == true).Select(c => new { c.Id, c.Descripcion }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
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
                var modelo = db.Centers.Where(x => x.Id == id).Select(c => new { c.Id, c.Descripcion }).FirstOrDefault();

                Center types = new Center
                {
                    Id = modelo.Id,
                    Descripcion = modelo.Descripcion
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
                var result = from c in db.Centers
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

        public JsonResult Create(Center modelo)
        {
            if (modelo == null)
            {
                return Json(new { responseCode = "-10" });
            }

            modelo.Enable = true;
            db.Centers.Add(modelo);
            db.SaveChanges();

            //Audito
            AuditHelper.Auditar("Alta", modelo.Id.ToString(), className, ModuleDescription, WindowDescription);

            var responseObject = new
            {
                responseCode = 0
            };

            return Json(responseObject);
        }


        public JsonResult Edit(Center modelo)
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

            Center modelo = db.Centers.Find(id);
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

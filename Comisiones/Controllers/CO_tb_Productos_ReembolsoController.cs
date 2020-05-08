using Comisiones.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class CO_tb_Productos_ReembolsoController : Controller
    {
        private herbaxEntities db = new herbaxEntities();

        // GET: CO_tb_Productos_Reembolso        
        public ActionResult Index(string IdSocio, string Nombre, int? VentaSocio, int? ReembolsoSocio)
        {
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 33 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            //ViewData["PlantaId"]
            ViewBag.Socio = String.IsNullOrEmpty(IdSocio) ? "IdSocio" : "";
            ViewBag.Nombre = String.IsNullOrEmpty(Nombre) ? "Nombre" : "";
            ViewBag.FechaPedido = DateTime.Now;
            ViewBag.Ventas = VentaSocio;
            ViewBag.Reembolso = ReembolsoSocio;
            ViewBag.Diferencia = 0;

            return View(db.CO_tb_Productos_Reembolso.ToList());
        }


        // GET: CO_tb_Productos_Reembolso/Details/5
        public ActionResult Details(string id)
        {
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 26 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso = db.CO_tb_Productos_Reembolso.Find(id);
            if (cO_tb_Productos_Reembolso == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Productos_Reembolso);
        }

        // GET: CO_tb_Productos_Reembolso/Create
        public ActionResult Create()
        {
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 19 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            return View();
        }

        // POST: CO_tb_Productos_Reembolso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sku,warehouse,Id_Empresa,Warehouse_ID,cve_art,Descripcion,DescripcionCorta,PrecioPublico,PrecioMinimo,PrecioCosto,PrecioSocio,ActivoSAE")] CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso)
        {
            if (ModelState.IsValid)
            {
                db.CO_tb_Productos_Reembolso.Add(cO_tb_Productos_Reembolso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_tb_Productos_Reembolso);
        }

        // GET: CO_tb_Productos_Reembolso/Edit/5
        public ActionResult Edit(string id)
        {
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 19 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso = db.CO_tb_Productos_Reembolso.Find(id);
            if (cO_tb_Productos_Reembolso == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Productos_Reembolso);
        }

        // POST: CO_tb_Productos_Reembolso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sku,warehouse,Id_Empresa,Warehouse_ID,cve_art,Descripcion,DescripcionCorta,PrecioPublico,PrecioMinimo,PrecioCosto,PrecioSocio,ActivoSAE")] CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cO_tb_Productos_Reembolso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_tb_Productos_Reembolso);
        }

        // GET: CO_tb_Productos_Reembolso/Delete/5
        public ActionResult Delete(string id)
        {
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 19 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso = db.CO_tb_Productos_Reembolso.Find(id);
            if (cO_tb_Productos_Reembolso == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Productos_Reembolso);
        }

        // POST: CO_tb_Productos_Reembolso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso = db.CO_tb_Productos_Reembolso.Find(id);
            db.CO_tb_Productos_Reembolso.Remove(cO_tb_Productos_Reembolso);
            db.SaveChanges();
            return RedirectToAction("Index");
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

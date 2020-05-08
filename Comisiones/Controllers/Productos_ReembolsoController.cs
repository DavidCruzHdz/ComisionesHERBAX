using Comisiones.Models;
using Comisiones.Models.ModelsEvolution;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class Productos_ReembolsoController : Controller
    {
        private herbaxEntities db = new herbaxEntities();

        // GET: Productos_Reembolso
        public ActionResult Index()
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
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                EvolutionEntities Evo = new EvolutionEntities();
                ViewBag.dropdownCia = new SelectList(Evo.CL_tb_Pais.Where(u => u.Warehouse != null).ToList(), "Id_Pais", "Pais");

                return View(db.CO_tb_Productos_Reembolso.ToList());
            }

        }

        // GET: Productos_Reembolso/Details/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 19 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
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


        }

        // GET: Productos_Reembolso/Create
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
            else
            {
                EvolutionEntities Evo = new EvolutionEntities();
                ViewBag.dropdownCia = new SelectList(Evo.CL_tb_Pais.Where(u => u.Warehouse != null).ToList(), "Id_Pais", "Pais");

                return View();
            }
        }

        // POST: Productos_Reembolso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sku,warehouse,Warehouse_ID,cve_art,Descripcion,DescripcionCorta,PrecioPublico,PrecioMinimo,PrecioCosto,PrecioSocio,ActivoSAE")] CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso)
        {
            if (ModelState.IsValid)
            {
                EvolutionEntities Evo = new EvolutionEntities();
                var Empresa = int.Parse(cO_tb_Productos_Reembolso.warehouse.ToString());
                var Cia = Evo.CL_tb_Pais.Where(x => x.Id_Pais == Empresa).FirstOrDefault();


                var IdCia = "";
                var CveWarehouse = "";
                var IdWarehouse = 0;

                if (Cia != null)
                {
                    IdCia = Cia.Folio;
                    CveWarehouse = Cia.Warehouse;
                    IdWarehouse = int.Parse(Cia.DistBusinessCenter.ToString());
                }


                cO_tb_Productos_Reembolso.Id_Empresa = IdCia;
                cO_tb_Productos_Reembolso.Warehouse_ID = IdWarehouse;
                cO_tb_Productos_Reembolso.warehouse = CveWarehouse;
                cO_tb_Productos_Reembolso.ActivoSAE = "Activo";
                cO_tb_Productos_Reembolso.SubTotal = 0;
                cO_tb_Productos_Reembolso.Id_Socio = "";
                cO_tb_Productos_Reembolso.CantidadProducto = 0;
                db.CO_tb_Productos_Reembolso.Add(cO_tb_Productos_Reembolso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_tb_Productos_Reembolso);
        }

        // GET: Productos_Reembolso/Edit/5
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
            else
            {
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
        }

        // POST: Productos_Reembolso/Edit/5
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

        // GET: Productos_Reembolso/Delete/5
        public ActionResult Delete(string id)
        {
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

        // POST: Productos_Reembolso/Delete/5
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

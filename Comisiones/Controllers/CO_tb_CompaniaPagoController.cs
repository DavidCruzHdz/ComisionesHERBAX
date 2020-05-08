using Comisiones.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class CO_tb_CompaniaPagoController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        // GET: CO_tb_CompaniaPago
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 27 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {

                return View(db.CO_tb_CompaniaPago.ToList());
            }
        }

        // GET: CO_tb_CompaniaPago/Details/5
        public ActionResult Details(int? id)
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 15 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_CompaniaPago cO_tb_CompaniaPago = db.CO_tb_CompaniaPago.Find(id);
                if (cO_tb_CompaniaPago == null)
                {
                    return HttpNotFound();
                }
                return View(cO_tb_CompaniaPago);
            }
        }

        // GET: CO_tb_CompaniaPago/Create
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 15 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                var contar = 0;
                List<SelectListItem> Porciento = new List<SelectListItem>();

                //foreach (var i = 0; i < Maximo; i++)
                for (int i = 0; i <= 100; i++)
                {
                    Porciento.Add(new SelectListItem() { Text = contar.ToString(), Value = contar.ToString() });
                    contar++;
                }


                ViewBag.porcentaje = Porciento;

                return View();
            }
        }

        // POST: CO_tb_CompaniaPago/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_CompaniaPago,nombreCompania,porcentaje,usuario,fechaMovimiento")] CO_tb_CompaniaPago cO_tb_CompaniaPago)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_tb_CompaniaPago.usuario = NomUsuario;
                cO_tb_CompaniaPago.fechaMovimiento = System.DateTime.Now;


                db.CO_tb_CompaniaPago.Add(cO_tb_CompaniaPago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_tb_CompaniaPago);
        }

        // GET: CO_tb_CompaniaPago/Edit/5
        public ActionResult Edit(int? id)
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 15 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_CompaniaPago cO_tb_CompaniaPago = db.CO_tb_CompaniaPago.Find(id);
                if (cO_tb_CompaniaPago == null)
                {
                    return HttpNotFound();
                }


                var contar = 0;
                List<SelectListItem> Porciento = new List<SelectListItem>();

                //foreach (var i = 0; i < Maximo; i++)
                for (int i = 0; i <= 100; i++)
                {
                    Porciento.Add(new SelectListItem() { Text = contar.ToString(), Value = contar.ToString() });
                    contar++;
                }


                ViewBag.porcenta = Porciento;
                return View(cO_tb_CompaniaPago);

            }
        }

        // POST: CO_tb_CompaniaPago/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_CompaniaPago,nombreCompania,porcentaje,usuario,fechaMovimiento")] CO_tb_CompaniaPago cO_tb_CompaniaPago)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();

                //var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                //var VarUsuario = Usuarios.IdUsuario;

                cO_tb_CompaniaPago.usuario = NomUsuario;
                cO_tb_CompaniaPago.fechaMovimiento = System.DateTime.Now;

                db.Entry(cO_tb_CompaniaPago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_tb_CompaniaPago);
        }

        // GET: CO_tb_CompaniaPago/Delete/5
        public ActionResult Delete(int? id)
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 15 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_CompaniaPago cO_tb_CompaniaPago = db.CO_tb_CompaniaPago.Find(id);
            if (cO_tb_CompaniaPago == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_CompaniaPago);
        }

        // POST: CO_tb_CompaniaPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_CompaniaPago cO_tb_CompaniaPago = db.CO_tb_CompaniaPago.Find(id);
            db.CO_tb_CompaniaPago.Remove(cO_tb_CompaniaPago);
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

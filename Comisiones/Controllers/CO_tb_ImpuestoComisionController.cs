using Comisiones.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class CO_tb_ImpuestoComisionController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        // GET: CO_tb_ImpuestoComision
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 29 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                return View(db.CO_tb_ImpuestoComision.ToList());
            }
        }

        // GET: CO_tb_ImpuestoComision/Details/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 17 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_ImpuestoComision cO_tb_ImpuestoComision = db.CO_tb_ImpuestoComision.Find(id);
                if (cO_tb_ImpuestoComision == null)
                {
                    return HttpNotFound();
                }
                return View(cO_tb_ImpuestoComision);
            }
        }

        // GET: CO_tb_ImpuestoComision/Create
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 17 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                return View();
            }
        }

        // POST: CO_tb_ImpuestoComision/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Impuesto,limiteInferior,limiteSuperior,porcentaje,coutaFija,usuario,fechaMovimiento")] CO_tb_ImpuestoComision cO_tb_ImpuestoComision)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_tb_ImpuestoComision.usuario = NomUsuario;
                cO_tb_ImpuestoComision.fechaMovimiento = System.DateTime.Now;

                cO_tb_ImpuestoComision.usuario = NomUsuario;
                cO_tb_ImpuestoComision.fechaMovimiento = System.DateTime.Now;

                db.CO_tb_ImpuestoComision.Add(cO_tb_ImpuestoComision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_tb_ImpuestoComision);
        }

        // GET: CO_tb_ImpuestoComision/Edit/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 17 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_ImpuestoComision cO_tb_ImpuestoComision = db.CO_tb_ImpuestoComision.Find(id);
                if (cO_tb_ImpuestoComision == null)
                {
                    return HttpNotFound();
                }
                return View(cO_tb_ImpuestoComision);
            }
        }

        // POST: CO_tb_ImpuestoComision/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Impuesto,limiteInferior,limiteSuperior,porcentaje,coutaFija,usuario,fechaMovimiento")] CO_tb_ImpuestoComision cO_tb_ImpuestoComision)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                //var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                //var VarUsuario = Usuarios.IdUsuario;

                cO_tb_ImpuestoComision.usuario = NomUsuario;
                cO_tb_ImpuestoComision.fechaMovimiento = System.DateTime.Now;

                db.Entry(cO_tb_ImpuestoComision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_tb_ImpuestoComision);
        }

        // GET: CO_tb_ImpuestoComision/Delete/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 17 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_ImpuestoComision cO_tb_ImpuestoComision = db.CO_tb_ImpuestoComision.Find(id);
            if (cO_tb_ImpuestoComision == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_ImpuestoComision);
        }

        // POST: CO_tb_ImpuestoComision/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_ImpuestoComision cO_tb_ImpuestoComision = db.CO_tb_ImpuestoComision.Find(id);
            db.CO_tb_ImpuestoComision.Remove(cO_tb_ImpuestoComision);
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

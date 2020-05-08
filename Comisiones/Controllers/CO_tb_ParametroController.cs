using Comisiones.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class CO_tb_ParametroController : Controller
    {
        private herbaxEntities db = new herbaxEntities();

        // GET: CO_tb_Parametro
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 32 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View(db.CO_tb_Parametro.ToList());
        }

        // GET: CO_tb_Parametro/Details/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 18 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Parametro cO_tb_Parametro = db.CO_tb_Parametro.Find(id);
            if (cO_tb_Parametro == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Parametro);
        }

        // GET: CO_tb_Parametro/Create
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 18 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            return View();
        }

        // POST: CO_tb_Parametro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Parametro,Key_parametro,parametro,descripcion,valor,valor_str,valor_dec,estatus,usuario,fechaMovimiento")] CO_tb_Parametro cO_tb_Parametro)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_tb_Parametro.usuario = NomUsuario;
                cO_tb_Parametro.fechaMovimiento = System.DateTime.Now;

                db.CO_tb_Parametro.Add(cO_tb_Parametro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_tb_Parametro);
        }

        // GET: CO_tb_Parametro/Edit/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 18 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Parametro cO_tb_Parametro = db.CO_tb_Parametro.Find(id);
            if (cO_tb_Parametro == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Parametro);
        }

        // POST: CO_tb_Parametro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Parametro,Key_parametro,parametro,descripcion,valor,valor_str,valor_dec,estatus,usuario,fechaMovimiento")] CO_tb_Parametro cO_tb_Parametro)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_tb_Parametro.usuario = NomUsuario;
                cO_tb_Parametro.fechaMovimiento = System.DateTime.Now;

                db.Entry(cO_tb_Parametro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_tb_Parametro);
        }

        // GET: CO_tb_Parametro/Delete/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 18 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Parametro cO_tb_Parametro = db.CO_tb_Parametro.Find(id);
            if (cO_tb_Parametro == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Parametro);
        }

        // POST: CO_tb_Parametro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_Parametro cO_tb_Parametro = db.CO_tb_Parametro.Find(id);
            db.CO_tb_Parametro.Remove(cO_tb_Parametro);
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

using Comisiones.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class CO_tb_ConceptosController : Controller
    {
        private herbaxEntities db = new herbaxEntities();

        // GET: CO_tb_Conceptos
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 28 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            //List<SelectListItem> tipo = new List<SelectListItem>()
            //        {
            //            new SelectListItem { Text = "Percepcion", Value = "1" },
            //            new SelectListItem { Text = "Deduccion", Value = "2" },
            //        };
            //tipo.Add(new SelectListItem() { Text = "Seleccione un Concepto", Value = "0" });

            //ViewBag.dropdownTipo = tipo;

            return View(db.CO_tb_Conceptos.ToList());
        }

        // GET: CO_tb_Conceptos/Details/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 16 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Conceptos cO_tb_Conceptos = db.CO_tb_Conceptos.Find(id);
            if (cO_tb_Conceptos == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Conceptos);
        }

        // GET: CO_tb_Conceptos/Create
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 16 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }


            List<SelectListItem> tipo = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Percepcion", Value = "P" },
                new SelectListItem { Text = "Deduccion", Value = "D" },
            };
            ViewBag.dropdownTipo = tipo;


            List<SelectListItem> gravable = new List<SelectListItem>()
            {
               new SelectListItem { Text = "Si", Value = "1" },
                new SelectListItem { Text = "No", Value = "0" },
            };
            ViewBag.dropdownGravable = gravable;


            List<SelectListItem> pagoEspecie = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Si", Value = "1" },
                new SelectListItem { Text = "No", Value = "0" },
            };
            ViewBag.dropdownPagoEspecie = pagoEspecie;


            List<SelectListItem> estatus = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Activo", Value = "1" },
                new SelectListItem { Text = "Inactivo", Value = "0" },
            };
            ViewBag.dropdownEstatus = estatus;


            return View();
        }

        // POST: CO_tb_Conceptos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Concepto,descripcion,tipo,gravable,pagoEspecie,estatus,usuario,fechaMovimiento")] CO_tb_Conceptos cO_tb_Conceptos)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                //var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                //var VarUsuario = Usuarios.IdUsuario;

                cO_tb_Conceptos.usuario = NomUsuario;
                cO_tb_Conceptos.fechaMovimiento = System.DateTime.Now;
                cO_tb_Conceptos.tipo = "D";
                db.CO_tb_Conceptos.Add(cO_tb_Conceptos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_tb_Conceptos);
        }

        // GET: CO_tb_Conceptos/Edit/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 16 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Conceptos cO_tb_Conceptos = db.CO_tb_Conceptos.Find(id);
            if (cO_tb_Conceptos == null)
            {
                return HttpNotFound();
            }




            List<SelectListItem> tipo = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Percepcion", Value = "P" },
                new SelectListItem { Text = "Deduccion", Value = "D" },
            };
            ViewBag.dropdownTipo = tipo;
      

            List<SelectListItem> gravable = new List<SelectListItem>()
            {
               new SelectListItem { Text = "Si", Value = "1" },
                new SelectListItem { Text = "No", Value = "0" },
            };
            ViewBag.dropdownGravable = gravable;


            List<SelectListItem> pagoEspecie = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Si", Value = "1" },
                new SelectListItem { Text = "No", Value = "0" },
            };
            ViewBag.dropdownPagoEspecie = pagoEspecie;


            List<SelectListItem> estatus = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Activo", Value = "1" },
                new SelectListItem { Text = "Inactivo", Value = "0" },
            };
            ViewBag.dropdownEstatus = estatus;







            return View(cO_tb_Conceptos);
        }

        // POST: CO_tb_Conceptos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Concepto,descripcion,tipo,gravable,pagoEspecie,estatus,usuario,fechaMovimiento")] CO_tb_Conceptos cO_tb_Conceptos)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_tb_Conceptos.usuario = NomUsuario;
                cO_tb_Conceptos.fechaMovimiento = System.DateTime.Now;

                db.Entry(cO_tb_Conceptos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_tb_Conceptos);
        }

        // GET: CO_tb_Conceptos/Delete/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 16 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Conceptos cO_tb_Conceptos = db.CO_tb_Conceptos.Find(id);
            if (cO_tb_Conceptos == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Conceptos);
        }

        // POST: CO_tb_Conceptos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_Conceptos cO_tb_Conceptos = db.CO_tb_Conceptos.Find(id);
            db.CO_tb_Conceptos.Remove(cO_tb_Conceptos);
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

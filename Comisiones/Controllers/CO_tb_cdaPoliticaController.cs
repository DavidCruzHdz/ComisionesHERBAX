using Comisiones.Models;
using Comisiones.Models.ModelsEvolution;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Comisiones.Controllers
{
    public class CO_tb_cdaPoliticaController : Controller
    {
        private EvolutionEntities Evo = new EvolutionEntities();
        private herbaxEntities db = new herbaxEntities();

        // GET: CO_tb_cdaPolitica
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 35 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }


            return View(Evo.CO_tb_cdaPolitica.ToList());
        }

        // GET: CO_tb_cdaPolitica/Details/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 24 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_cdaPolitica cO_tb_cdaPolitica = Evo.CO_tb_cdaPolitica.Find(id);
            if (cO_tb_cdaPolitica == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_cdaPolitica);
        }

        // GET: CO_tb_cdaPolitica/Create
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 24 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            List<SelectListItem> estatus = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Activo", Value = "1" },
                new SelectListItem { Text = "Inactivo", Value = "0" },
            };
            ViewBag.dropdownEstatus = estatus;
            

            var query = from o in db.CO_tb_Conceptos.ToList()                       
                        where o.tipo == "P" && o.estatus == 1
                        select new SelectListItem
                        {
                            Text = o.descripcion,
                            Value = o.id_Concepto.ToString(),
                        };

            ViewBag.dropdownPercepcion = query.ToList();
            



            return View();
        }

        // POST: CO_tb_cdaPolitica/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Politica,Descripcion,Descripcion_corta,Monto,Cantidad,Cantidad_Topada,id_percepcion,id_estatus,f_Vigencia,UltMod_Usuario,UltMod_F")] CO_tb_cdaPolitica cO_tb_cdaPolitica)
        {
            if (ModelState.IsValid)
            {

                var Busca = Evo.CO_tb_cdaPolitica.OrderByDescending(x => x.Id_Politica).FirstOrDefault();
                if (Busca != null)
                {
                    cO_tb_cdaPolitica.Id_Politica = Busca.Id_Politica + 1;
                }

                var NomUsuario = Session["IdNombre"].ToString();
                cO_tb_cdaPolitica.UltMod_Usuario = NomUsuario;
                cO_tb_cdaPolitica.UltMod_F = System.DateTime.Now;

                Evo.CO_tb_cdaPolitica.Add(cO_tb_cdaPolitica);
                Evo.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_tb_cdaPolitica);
        }

        // GET: CO_tb_cdaPolitica/Edit/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 24 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_cdaPolitica cO_tb_cdaPolitica = Evo.CO_tb_cdaPolitica.Find(id);

            

            if (cO_tb_cdaPolitica == null)
            {
                return HttpNotFound();
            }


            List<SelectListItem> estatus = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Activo", Value = "1" },
                new SelectListItem { Text = "Inactivo", Value = "0" },
            };
            ViewBag.dropdownEstatus = estatus;

            var query = from o in db.CO_tb_Conceptos.ToList()
                        where o.tipo == "P" && o.estatus == 1
                        select new SelectListItem
                        {
                            Text = o.descripcion,
                            Value = o.id_Concepto.ToString(),
                            Selected = (o.id_Concepto == cO_tb_cdaPolitica.id_percepcion)
                        };

            ViewBag.dropdownPercepcion = query.ToList();
            ViewBag.Vigencia = cO_tb_cdaPolitica.f_Vigencia;  //.ToString("yy/MM/dd");
            cO_tb_cdaPolitica.f_Vigencia = Convert.ToDateTime(ViewBag.Vigencia); 

            //cO_tb_cdaPolitica.f_Vigencia = FormatDateTime(cO_tb_cdaPolitica.f_Vigencia, Constants.vbShortDate);
            //cO_tb_cdaPolitica.f_Vigencia = cO_tb_cdaPolitica.f_Vigencia.ToShortDateString;
            return View(cO_tb_cdaPolitica);
        }

        // POST: CO_tb_cdaPolitica/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Politica,Descripcion,Descripcion_corta,Monto,Cantidad,Cantidad_Topada,id_percepcion,id_estatus,f_Vigencia,UltMod_Usuario,UltMod_F")] CO_tb_cdaPolitica cO_tb_cdaPolitica)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_tb_cdaPolitica.UltMod_Usuario = NomUsuario;
                cO_tb_cdaPolitica.UltMod_F = System.DateTime.Now;

                Evo.Entry(cO_tb_cdaPolitica).State = EntityState.Modified;
                Evo.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_tb_cdaPolitica);
        }

        // GET: CO_tb_cdaPolitica/Delete/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 24 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_cdaPolitica cO_tb_cdaPolitica = Evo.CO_tb_cdaPolitica.Find(id);
            if (cO_tb_cdaPolitica == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_cdaPolitica);
        }

        // POST: CO_tb_cdaPolitica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_cdaPolitica cO_tb_cdaPolitica = Evo.CO_tb_cdaPolitica.Find(id);
            Evo.CO_tb_cdaPolitica.Remove(cO_tb_cdaPolitica);
            Evo.SaveChanges();
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

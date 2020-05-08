using Comisiones.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class RolesController : Controller
    {
        private herbaxEntities db = new herbaxEntities();


        // GET: Roles
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 44 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View(db.CO_tb_Roles.ToList());
        }

        // GET: Roles/Details/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 45 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_Roles cO_tb_Roles = db.CO_tb_Roles.Find(id);
                if (cO_tb_Roles == null)
                {
                    return HttpNotFound();
                }
                return View(cO_tb_Roles);
            }

        }

        // GET: Roles/Create
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 46 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }

        // POST: Roles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRol,NombreRol,FechaAlta,UsuarioAlta,FechaCambio,UsuarioCambio")] CO_tb_Roles cO_tb_Roles)
        {
            if (ModelState.IsValid)
            {
                //CO_tb_Usuarios VarUsuario = db.CO_tb_Usuarios.Where(c => c.NombreUsuario == User.Identity.IsAuthenticated.ToString()).FirstOrDefault();
                var VarUsuario = 3; /// este dato fijo debe variar
                cO_tb_Roles.Estatus = true;
                cO_tb_Roles.FechaAlta = System.DateTime.Now;
                cO_tb_Roles.UsuarioAlta = VarUsuario;
                cO_tb_Roles.FechaCambio = System.DateTime.Now;
                cO_tb_Roles.UsuarioCambio = VarUsuario;

                db.CO_tb_Roles.Add(cO_tb_Roles);
                db.SaveChanges();
                return Redirect("Index");
            }

            return View(cO_tb_Roles);
        }

        // GET: Roles/Edit/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 47 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_Roles cO_tb_Roles = db.CO_tb_Roles.Find(id);
                if (cO_tb_Roles == null)
                {
                    return HttpNotFound();
                }

                List<SelectListItem> IdRoles = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Activo", Value = "True" },
                     new SelectListItem { Text = "Inactivo", Value = "False" },
               };

                ViewBag.Estatus = new SelectList(IdRoles, "Value", "Text", cO_tb_Roles.Estatus);
                return View(cO_tb_Roles);
            }

        }

        // POST: Roles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IdRol,NombreRol,FechaAlta,UsuarioAlta,FechaCambio,UsuarioCambio,Estatus")] CO_tb_Roles cO_tb_Roles)
        public ActionResult Edit(CO_tb_Roles entity)
        {
            if (ModelState.IsValid)
            {

                CO_tb_Roles BuscaRegistro = db.CO_tb_Roles.Find(entity.IdRol);
                if (BuscaRegistro != null)
                {
                    var VarUsuario = 3; /// este dato fijo debe variar               
                    BuscaRegistro.FechaCambio = System.DateTime.Now;
                    BuscaRegistro.UsuarioCambio = VarUsuario;
                    BuscaRegistro.Estatus = entity.Estatus;
                    BuscaRegistro.NombreRol = entity.NombreRol;

                }

                db.CO_tb_Roles.Attach(BuscaRegistro);
                db.Entry(BuscaRegistro).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entity);
        }

        // GET: Roles/Delete/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 48 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_Roles cO_tb_Roles = db.CO_tb_Roles.Find(id);
                if (cO_tb_Roles == null)
                {
                    return HttpNotFound();
                }
                return View(cO_tb_Roles);
            }
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_Roles cO_tb_Roles = db.CO_tb_Roles.Find(id);
            db.CO_tb_Roles.Remove(cO_tb_Roles);
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





        public ActionResult Pantallas(int id)
        {
            Session["RolPermisos"] = id;
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }


            if (!db.CO_tb_PermisosRoles.Any(c => c.IdPantalla == 49 && c.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                herbaxEntities context = new herbaxEntities();
                var BuscaRol = db.CO_tb_Roles.Where(x => (x.IdRol == id)).FirstOrDefault();

                if (BuscaRol != null)
                {
                    var entity = db.CO_tb_Pantallas.Where(x => x.PadreId != null).ToList();
                    var permisos = db.CO_tb_PermisosRoles.Where(x => x.IdRol == BuscaRol.IdRol);

                    ViewBag.VarNombreRol = BuscaRol.NombreRol;

                    foreach (var item in permisos)
                    {
                        entity.Find(x => x.IdPantalla == item.IdPantalla && x.PadreId != null).checkeado = true;

                    }

                    //return View(db.CO_tb_Pantallas.ToList());
                    ViewBag.IsOK = 1;
                    ViewBag.Message = "Seleccione las pantallas que desea accesar con este Rol";
                    return View(entity.ToList());
                }
                else
                {
                    ViewBag.IsOK = 0;
                    ViewBag.Message = "no existe Pantalla asignadas a este Rol";
                    return View();
                }

            }
        }


        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Pantallas(IList<CO_tb_Pantallas> entity)
        {
            herbaxEntities context = new herbaxEntities();
            herbaxEntities context2 = new herbaxEntities();
            //if (Session["RolPermisos"]  = "")
            //{

            //}

            int Rol = int.Parse(Session["RolPermisos"].ToString());
            var aa = context.CO_tb_PermisosRoles.Where(x => x.IdRol == Rol);
            if (aa.Count() >= 1)
            {
                context.CO_tb_PermisosRoles.RemoveRange(aa);
                context.SaveChanges();
            }

            CO_tb_PermisosRoles entity2 = new CO_tb_PermisosRoles();
            var cuantos = context2.CO_tb_Pantallas.Count();
            //var VarIdRol = 0;
            
            //if (Session["idRol"] != null)
            //{
            //    VarIdRol = int.Parse(Session["idRol"].ToString());
            //}

            foreach (var item in entity)            
            {
                if (item.checkeado == true) // si esta chequeado
                                              //.item.checkeado == true) // si esta chequeado
                {
                    var idPantalla = item.IdPantalla;
                    entity2.IdPantalla = idPantalla;
                    entity2.IdRol = Rol;

                    var VarUsuario = 3; /// este dato fijo debe variar
                    entity2.FechaAlta = System.DateTime.Now;
                    entity2.UsuarioAlta = VarUsuario;
                    entity2.FechaCambio = System.DateTime.Now;
                    entity2.UsuarioCambio = VarUsuario;

                    context2.CO_tb_PermisosRoles.Add(entity2);
                    context2.SaveChanges();
                }


            }



            return RedirectToAction("Index");

        }


    }
}

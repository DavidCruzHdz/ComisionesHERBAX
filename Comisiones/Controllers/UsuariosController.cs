using Comisiones.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace Comisiones.Controllers
{
    //[Authorize]
    public class UsuariosController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        // GET: Usuarios
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 39 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                var cO_tb_Usuarios = db.CO_tb_Usuarios.Include(c => c.CO_tb_Roles);
                return View(cO_tb_Usuarios.ToList());
            }

        }



        // GET: Usuarios/Details/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 40 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_Usuarios cO_tb_Usuarios = db.CO_tb_Usuarios.Find(id);
                if (cO_tb_Usuarios == null)
                {
                    return HttpNotFound();
                }
                return View(cO_tb_Usuarios);
            }
        }

        // GET: Usuarios/Create
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 41 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                ViewBag.IdRol = new SelectList(db.CO_tb_Roles, "IdRol", "NombreRol");
                //ViewBag.Id_Pais = new SelectList(db.CO_tb_Pais, "Id_Pais", "Pais");
                return View();
            }
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,IdRol,NombreUsuario,Correo,Password,Id_Pais,Estatus,FechaAlta,UsuarioAlta,FechaCambio,UsuarioCambio")] CO_tb_Usuarios cO_tb_Usuarios)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();

                var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                var VarUsuario = Usuarios.IdUsuario;

                cO_tb_Usuarios.Estatus = true;
                cO_tb_Usuarios.FechaAlta = System.DateTime.Now;
                cO_tb_Usuarios.UsuarioAlta = VarUsuario;
                cO_tb_Usuarios.FechaCambio = System.DateTime.Now;
                cO_tb_Usuarios.UsuarioCambio = VarUsuario;
                cO_tb_Usuarios.Id_Pais = 0;
                db.CO_tb_Usuarios.Add(cO_tb_Usuarios);
                db.SaveChanges();
                return Redirect("Index");
            }

            ViewBag.IdRol = new SelectList(db.CO_tb_Roles, "IdRol", "NombreRol", cO_tb_Usuarios.IdRol);
            //ViewBag.Id_Pais = new SelectList(db.CO_tb_Pais, "Id_Pais", "Pais");
            return View(cO_tb_Usuarios);
        }

        // GET: Usuarios/Edit/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 42 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_Usuarios cO_tb_Usuarios = db.CO_tb_Usuarios.Find(id);
                if (cO_tb_Usuarios == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IdRol = new SelectList(db.CO_tb_Roles, "IdRol", "NombreRol", cO_tb_Usuarios.IdRol);
                //ViewBag.Id_Pais = new SelectList(db.CO_tb_Pais, "Id_Pais", "Pais", cO_tb_Usuarios.Id_Pais);


                List<SelectListItem> IdUsuarios = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Activo", Value = "True" },
                    new SelectListItem { Text = "Inactivo", Value = "False" },
                };

                ViewBag.Estatus = new SelectList(IdUsuarios, "Value", "Text", cO_tb_Usuarios.Estatus);

                return View(cO_tb_Usuarios);
            }
        }


        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CO_tb_Usuarios entity)
        {
            if (ModelState.IsValid)
            {

                CO_tb_Usuarios BuscaRegistro = db.CO_tb_Usuarios.Find(entity.IdUsuario);
                if (BuscaRegistro != null)
                {
                    var VarUsuario = 3; /// este dato fijo debe variar               
                    BuscaRegistro.IdRol = entity.IdRol;
                    BuscaRegistro.NombreUsuario = entity.NombreUsuario;
                    BuscaRegistro.Correo = entity.Correo;
                    BuscaRegistro.Password = entity.Password;
                    BuscaRegistro.Id_Pais = 0;
                    BuscaRegistro.FechaCambio = System.DateTime.Now;
                    BuscaRegistro.UsuarioCambio = VarUsuario;
                    BuscaRegistro.Estatus = entity.Estatus;
                }

                db.CO_tb_Usuarios.Attach(BuscaRegistro);
                db.Entry(BuscaRegistro).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Usuarios/Index");
            }

            ViewBag.IdRol = new SelectList(db.CO_tb_Roles, "IdRol", "NombreRol", entity.IdRol);
            //ViewBag.Id_Pais = new SelectList(db.CO_tb_Pais, "Id_Pais", "Pais");
            return View(entity);
        }



        // GET: Usuarios/Delete/5
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 43 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CO_tb_Usuarios cO_tb_Usuarios = db.CO_tb_Usuarios.Find(id);
                if (cO_tb_Usuarios == null)
                {
                    return HttpNotFound();
                }
                return View(cO_tb_Usuarios);
            }
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_Usuarios cO_tb_Usuarios = db.CO_tb_Usuarios.Find(id);
            //db.CO_tb_Usuarios.Remove(cO_tb_Usuarios);
            //db.SaveChanges();

            //CO_tb_Usuarios VarUsuario = db.CO_tb_Usuarios.Where(c => c.NombreUsuario == User.Identity.IsAuthenticated.ToString()).FirstOrDefault();
            var VarUsuario = 3; /// este dato fijo debe variar   
            cO_tb_Usuarios.UsuarioCambio = VarUsuario;
            cO_tb_Usuarios.Estatus = false;
            db.Entry(cO_tb_Usuarios).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect("~/Usuarios/Index");
        }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Comisiones.Data;
using Comisiones.Models;
using Comisiones.Models.ModelsEvolution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using SpreadsheetLight;
using System.IO;
using Comisiones.Models.ViewModels;

namespace Comisiones.Controllers
{
    public class CambioPasswordController : Controller
    {
        private string nameController = "ReembolsosCanjeController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private herbaxEntities db = new herbaxEntities();

        // GET: CambioPassword
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 37 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                //var cO_tb_Usuarios = db.CO_tb_Usuarios.Include(c => c.CO_tb_Roles);
                //return View(cO_tb_Usuarios.ToList());

                var NomUsuario = Session["IdNombre"].ToString();

            

                if (NomUsuario == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return View();
                }
                else
                {
                    var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                    var varUsuario = Usuarios.IdUsuario;

                    //CO_tb_Usuarios cO_tb_Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                        if (Usuarios == null)
                        {
                            return HttpNotFound();
                        }

                        ViewBag.IdRol = new SelectList(db.CO_tb_Roles, "IdRol", "NombreRol", Usuarios.IdRol);

                        List<SelectListItem> IdUsuarios = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Activo", Value = "True" },
                            new SelectListItem { Text = "Inactivo", Value = "False" },
                        };

                        ViewBag.Estatus = new SelectList(IdUsuarios, "Value", "Text", Usuarios.Estatus);
                        return View(Usuarios);
                }
            }

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string ConfirmaPass, [Bind(Include = "IdUsuario,IdRol,NombreUsuario,Correo,Password,Id_Pais,Estatus,FechaAlta,UsuarioAlta,FechaCambio,UsuarioCambio")] CO_tb_Usuarios cO_tb_Usuarios)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                var VarUsuario = Usuarios.IdUsuario;

                CO_tb_Usuarios BuscaRegistro = db.CO_tb_Usuarios.Find(VarUsuario);
                if (BuscaRegistro != null)
                {                    
                    BuscaRegistro.Password = ConfirmaPass;
                    BuscaRegistro.FechaCambio = System.DateTime.Now;
                    BuscaRegistro.UsuarioCambio = VarUsuario;                    
                    db.CO_tb_Usuarios.Attach(BuscaRegistro);
                    db.Entry(BuscaRegistro).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("~/Home/Deslogear");
                }
            }

            ViewBag.IdRol = new SelectList(db.CO_tb_Roles, "IdRol", "NombreRol", cO_tb_Usuarios.IdRol);
            return View(cO_tb_Usuarios);
        }


    }
}





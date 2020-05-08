namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        public ActionResult Index()
        {
            ViewBag.Message = "";
            ViewBag.IsOK = 0;

            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());


                //var listaMenu = from p in db.CO_tb_Pantallas.ToList()
                //                from a in db.CO_tb_PermisosRoles.ToList()
                //                where p.IdPantalla == a.IdPantalla && a.IdRol == VarRol
                //                select new
                //                {
                //                    IdPantalla = p.IdPantalla,
                //                    PadreId = p.PadreId,
                //                    NombrePantalla = p.NombrePantalla,
                //                    checkeado = p.checkeado,
                //                    Archivo = p.Archivo,
                //                    Carpeta = p.Carpeta,
                //                    Tipo = p.Tipo,
                //                    Entre = p.Entre,
                //                    DescripcionMenu = p.DescripcionMenu,
                //                    FechaAlta = p.FechaAlta,
                //                    UsuarioAlta = p.UsuarioAlta,
                //                    FechaCambio = p.FechaCambio,
                //                    UsuarioCambio = p.UsuarioCambio,
                //                };


                //var Menu = listaMenu.OrderBy(u => u.IdPantalla).ToList();


                //var db2 = new herbaxEntities();
                //var result = from p in db2.CO_tb_Pantallas
                //             join a in db2.CO_tb_PermisosRol  es on p.IdPantalla equals a.IdPantalla
                //             select new MenuDefinidoPorRol()
                //             {
                //                 Id = p.IdPantalla,
                //                 Level = p.PadreId,
                //                 Nombre = p.NombrePantalla,
                //                 Liga = p.Archivo,
                //                 Vista = p.Carpeta,
                //            };

                //return View(result.OrderBy(u => u.Id).ToList());





                //var db = new MyDbContext();
                //var result = from name in db.Names
                //             join category in db.Categories on name.CategoryId equals category.CategoryId
                //             join level in db.Levels on category.LevelId equals level.LevelId
                //             select new ComplexViewModel()
                //             {
                //                 Name = name.Name,
                //                 Category = category.CategoryName,
                //                 Level = level.LevelName,
                //             };
                //return result.ToList();



            }
            else
            {

                return Redirect("~/Home/Login");
            }




            //if (db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 1 && x.IdRol == VarIdRol))
            //{

            //    if (string.IsNullOrEmpty(sesion.urlMain))
            //    {
            //        sesion.urlMain = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            //        string strSlash = sesion.urlMain.Substring(sesion.urlMain.Length - 1);
            //        if (strSlash == "/")
            //        {
            //            sesion.urlMain = sesion.urlMain.Remove(sesion.urlMain.Length - 1);
            //        }
            //    }
            //}
            //else
            //{
            //    return Redirect("~/Home/Login");
            //}



            // using (EvolutionEntities evoEntities = new EvolutionEntities())
            //{
            // sesion.periodoActivo = (from periodo in evoEntities.CU_tb_Periodo
            //                         where periodo.Estatus == 33  /*Estatus ABIERTO*/
            //                         select periodo).FirstOrDefault();
            //}


            return View();
        }


        // GET: Usuarios
        public ActionResult Login()
        {
            return View();
        }


        public ActionResult SinPermiso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(CO_tb_Usuarios entity)
        {
            var exist = db.CO_tb_Usuarios.Where(x => x.NombreUsuario == entity.NombreUsuario && x.Password == entity.Password && x.Estatus == true).FirstOrDefault();
            if (exist != null)
            {
                Session["idRol"] = exist.IdRol;


                if (Session["idRol"] != null)
                {

                    //FormsAuthentication.SetAuthCookie(exist.NombreUsuario, false);
                    Session["logeado"] = true;
                    Session["idUsuario"] = exist.IdUsuario;

                    Session["IdNombre"] = entity.NombreUsuario;
                    Session.Timeout = 50000;
                    var VarRol = int.Parse(Session["idRol"].ToString());
                    var Pantallas = db.CO_tb_Pantallas.ToList();

                    foreach (var item in Pantallas)
                    {
                        if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == item.IdPantalla && x.IdRol == VarRol))
                        {
                            sesion.urlMain = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                            string strSlash = sesion.urlMain.Substring(sesion.urlMain.Length - 1);
                            if (strSlash == "/")
                            {
                                sesion.urlMain = sesion.urlMain.Remove(sesion.urlMain.Length - 1);
                            }
                        }
                    }

                    return Redirect("~/Home/Index");
                }
                else
                {
                    Session["idUsuario"] = 0;
                    Session["logeado"] = false;
                    Session["idRol"] = 0;
                    ViewBag.Message = "Usuario o Contraseña Incorrectos internte nuevamente";
                    ViewBag.IsPedido = 0;
                    ViewBag.IsOK = 0;

                    ViewBag.error = "Usuario o Contraseña Incorrectos internte nuevamente";
                    
                    return Redirect("~/Home/Login");
                }

            }

            ViewBag.Message = "Usuario o Contraseña Incorrectos internte nuevamente";
            ViewBag.IsOK = 0;
            return Redirect("~/Home/Login");
        }

        public ActionResult DesLogear()
        {
            Session["logeado"] = false;
            Session["idUsuario"] = 0;
            Session["idRol"] = 0;
            return Redirect("~/Home/Login");
        }

        public ActionResult About()
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 1 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                ViewBag.Message = "Your application description page.";
            }

            return View();
        }

        public ActionResult Contact()
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 1 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                ViewBag.Message = "Your contact page.";
            }

            return View();
        }



        /*private Boolean getEstatusProcCalculo()
        {
            bool isEnable = false;

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                if (hbxEntities.CO_st_validaEstDelProcCalculo() != null)
                {
                    if (hbxEntities.CO_st_validaEstDelProcCalculo().FirstOrDefault() == 1) { isEnable = true; }
                }
            }
            return isEnable;
        }*/




        public JsonResult CreaMenu(int IdRol = 0)
        {
            var VarRol = 0;

            try
            {

                if (IdRol != 0)
                {
                    VarRol = IdRol;

                    var listaMenu = db.CO_tb_Pantallas.Where(x => x.PadreId == null).ToList();
                    return Json(listaMenu, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {

                return Json(e);
            }

            //var Clave = IdRol;

            //Session["IdUserAutoriza"] = "";

            //try
            //{
            //    var Busca = db.CPUsuario.Where(x => (x.CPIdUsuario == Usuario && x.CPPassword == Clave && x.CPRol_id != 2)).FirstOrDefault();

            //    if (Busca != null)
            //    {

            //        return Json(db.CPUsuario.ToList().Where(x => (x.CPIdUsuario == IdAutorizacion)).Select(x => new { CPIdUsuario = x.CPIdUsuario }), JsonRequestBehavior.AllowGet);

            //    }
            //    else
            //    {

            //    }

            //    return Json(IdAutorizacion);
            //}
            //catch (Exception e)
            //{

            return Json(VarRol);



        }


    }
}



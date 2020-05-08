namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using Comisiones.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class PrestamoSocioController : Controller
    {
        // GET: PrestamoSocio
        public ActionResult Index()
        {
            herbaxEntities db = new herbaxEntities();
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 12 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }

        //objParameter = "{ 'Partner_id': '" + $("#Partner_id").val() + "','FirstName': '" + $("#FirstName").val() + "','LastName': '" + $("#LastName").val() + "' }";

        [HttpPost]
        public JsonResult buscarSocio(SociosViewModel_v1 socioRequest)
        {
            /*
            var _socios = Data.Prestamo.dataPrestamo.instance.obtieneSocios(socioRequest.Partner_id, socioRequest.FirstName, socioRequest.LastName);

            var json = Json(_socios, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;*/

            try
            {
                List<CO_st_BuscarSocioMovimientos_Result> _socios = new List<CO_st_BuscarSocioMovimientos_Result>();
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    _socios = hbxEntities.CO_st_BuscarSocioMovimientos(socioRequest.Partner_id, socioRequest.FirstName, socioRequest.LastName).ToList();
                    //sesion.periodoActivo = null;
                }
                //return Json("success");
                var json = Json(_socios, JsonRequestBehavior.AllowGet);
                return json;
            }
            catch (Exception e)
            {
                return Json("Error:Error al ejecutar proceso " + e);
            }
        }





    }
}

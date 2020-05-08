namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class CalculaComisionController : Controller
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 5 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {

                List<CO_st_InfoPeriodoActivo_Result> infoPeriodoActivo;

                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    infoPeriodoActivo = hbxEntities.CO_st_InfoPeriodoActivo().ToList();
                }

                //Json(tblBonos, JsonRequestBehavior.AllowGet);
                return View(infoPeriodoActivo);

            }
        }



        public ActionResult VistaPrevia()
        {
            try
            {
                List<CO_st_calculoImpuesto_VistaPrevia2_Result> tblCalculo = new List<CO_st_calculoImpuesto_VistaPrevia2_Result>();

                //List<CO_st_calculoImpuesto_VistaPrevia2_Result> tblCalculo;

                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    tblCalculo = hbxEntities.CO_st_calculoImpuesto_VistaPrevia2().ToList();
                }

                return Json(tblCalculo, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("Error:Error al ejecutar proceso " + e);
            }
        }


        public ActionResult CalculaComision()
        {
            try
            {
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    hbxEntities.CO_st_calculoImpuesto_pr();
                    //sesion.periodoActivo = null;
                }
                return Json("success");

            }
            catch (Exception e)
            {
                return Json("Error:Error al ejecutar proceso " + e);
            }
        }
    }
}

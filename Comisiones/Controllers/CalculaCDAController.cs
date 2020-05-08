namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class CalculaCDAController : Controller
    {
        // GET: CalculaCDA
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 6 && x.IdRol == VarRol))
            {              
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }
        public ActionResult GenerarMovSocio()
        {

            List<CO_st_InsertMovimientosSocio_CDA_Result> tblBonos;
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {

               

                try
                {
                    //Procedimiento que retorna monto,partner_id,id_Periodo,estatus par amostrarlo en la tabla de comisiones cargadas
                    tblBonos = hbxEntities.CO_st_InsertMovimientosSocio_CDA().ToList();

                    if (Convert.ToInt32(tblBonos[0].numReg) > Convert.ToInt32(0))
                    {
                        return Json("success");
                        //return Json(tblBonos, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("error");


                    }


                }
                catch (Exception ex)
                {
                    var IsOk = 0;
                    var Mensaje = ex.InnerException.Message.ToString();
                    return this.Json(new { IsOk, Mensaje }, JsonRequestBehavior.AllowGet);
                };


            }
            //return Json(tblBonos, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CalcularCDA()
        {

            List<CO_st_CalculaBonosCDA_Result> tblBonos;
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {

                //Procedimiento que calcula las bonos correspondientes a cada encargado del CDA
                tblBonos = hbxEntities.CO_st_CalculaBonosCDA().ToList();

            }
            return Json(tblBonos, JsonRequestBehavior.AllowGet);
        }

    }
}
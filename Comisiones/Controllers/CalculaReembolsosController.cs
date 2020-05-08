namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Comisiones.Models.ModelsEvolution;
    using System.Data;
    using Comisiones.Data;
    using System.IO;
    using Comisiones.Models.ViewModels;

    public class CalculaReembolsosController : Controller
    {
        private string nameController = "CalculaReembolsosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private herbaxEntities db = new herbaxEntities();
        // GET: CalculaReembolso
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 18 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }


            ViewBag.Message = "";
            ViewBag.IsPedido = 0;
            ViewBag.IsOK = 0;

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_getReembolsos_Result> tblReembolsos;
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Procedimiento que calcula los reembolsos que se le dara a cada persona de acuerdoa sus ventas
                    tblReembolsos = hbxEntities.CO_st_getReembolsos2().ToList();

                    if (tblReembolsos != null)
                    {
                        ViewBag.IsOK = 1;
                        return View(tblReembolsos.ToList());
                    }
                    else
                    {
                        ViewBag.IsOK = 0;
                        ViewBag.Message = "No se encontraron registro al calcular";
                        return View();
                    }
                    
                }
                catch (Exception ex)
                {
                    ViewBag.IsOK = 0;
                    var Col = 0;
                    var resultado = "";

                    resultado = ex.InnerException.Message.ToString();
                    Col = resultado.IndexOf("\r");

                    if (Col.ToString() == "")
                    {
                        var NewString = resultado.Substring(0, Col);
                        ViewBag.Message = NewString;
                    }
                    else
                    {
                        ViewBag.Message = resultado;
                    }

                    return View();
                };
            }

        }


        public ActionResult GenerarMovSocio()
        {

            List<CO_st_CO_st_InsertaMovimientosSocio_Reembolsos_Result> tblReembolsos;
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Procedimiento que retorna monto,partner_id,id_Periodo,estatus par amostrarlo en la tabla de comisiones cargadas
                    tblReembolsos = hbxEntities.CO_st_InsertaMovimientosSocio_Reembolsos().ToList();

                    if (Convert.ToInt32(tblReembolsos[0].numReg) > Convert.ToInt32(0))
                    {



                        return Json("success");
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
            //return Json(tblReembolsos, JsonRequestBehavior.AllowGet);
        }



        public ActionResult CalculaReembolsos()
        {

            List<CO_st_getReembolsos_Result> tblReembolsos;
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                { 
                      //Procedimiento que calcula los reembolsos que se le dara a cada persona de acuerdoa sus ventas
                        tblReembolsos = hbxEntities.CO_st_getReembolsos2().ToList();
                        return Json(tblReembolsos, JsonRequestBehavior.AllowGet);
                }
                 catch (Exception ex)
                {
                    var IsOk = 0;
                    var Mensaje = ex.InnerException.Message.ToString();
                    return this.Json(new { IsOk, Mensaje }, JsonRequestBehavior.AllowGet);
                };
            }

        }

        public ActionResult EnviaReembolsosAlHistorial()
        {
            try
            {
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    hbxEntities.CO_st_CierreDePeriodoReembolso();
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
namespace Comisiones.Controllers
{
    /*using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;*/

    using Comisiones.Models;
    using Comisiones.Models.ViewModels;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class ConsultaPremiosController : Controller
    {
        private string nameController = "ConsultaPremiosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 16 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }


        //[HttpPost]
        //public JsonResult getPaises(PaisViewModel pais)
        //{
        //    //var revisar = prestamoData.idFormaPago; 
        //    List<CO_st_ListaPaises_Result> comboPaises = new List<CO_st_ListaPaises_Result>();
        //    using (herbaxEntities hbxEntities = new herbaxEntities())
        //    {
        //        comboPaises = hbxEntities.CO_st_ListaPaises().ToList();
        //    }

        //    return Json(comboPaises, JsonRequestBehavior.AllowGet);
        //}



        //obtiene los Paises para cargar combo País
        [HttpPost]
        public JsonResult getPaises(PaisViewModel pais)
        {
            //var revisar = prestamoData.idFormaPago; 
            List<CO_st_ListaPaises_Result> comboPaises = new List<CO_st_ListaPaises_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                comboPaises = hbxEntities.CO_st_ListaPaises().ToList();
            }

            return Json(comboPaises, JsonRequestBehavior.AllowGet);
        }



        public JsonResult ListarPremios(ConsultaPremiosViewModel modelConsulta)
        {
            //CO_st_BuscarDatosPagoSocio
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaPedidosPremios2_Result> listaPremiosHist = new List<CO_st_BuscaPedidosPremios2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //campos que Retorna: partner_id, tax_id, social_security, bank_id, account, Paymethod_com
                    listaPremiosHist = hbxEntities.CO_st_BuscaPedidosPremios2(modelConsulta.partner_id, modelConsulta.idPais, modelConsulta.fInicio, modelConsulta.fFin).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_getPremiosHist  DETALLE: " + ex);
                }


            }
            return Json(listaPremiosHist, JsonRequestBehavior.AllowGet);

            //return View();
        }

    }//fin de la clase




}


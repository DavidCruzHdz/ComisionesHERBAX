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

    public class CatSociosController : Controller
    {
        private string nameController = "CatSociosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 26 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }


        public JsonResult ListarSocios(UpdateSociosViewModel modelSocios)
        {
            //CO_st_BuscarDatosPagoSocio
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscarDatosPagoSocio_Result> listaSocios = new List<CO_st_BuscarDatosPagoSocio_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    string partner_id = modelSocios.partner_id;
                    //campos que Retorna: partner_id, tax_id, social_security, bank_id, account, Paymethod_com
                    listaSocios = hbxEntities.CO_st_BuscarDatosPagoSocio(partner_id).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_BuscarDatosPagoSocio  DETALLE: " + ex);
                }


            }
            return Json(listaSocios, JsonRequestBehavior.AllowGet);

            //return View();
        }

        [HttpPost]
        public JsonResult AddOrEdit(UpdateSociosViewModel modelSocios)
        {
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscarDatosPagoSocio_Result> listaSocios = new List<CO_st_BuscarDatosPagoSocio_Result>();
            var mensaje_ = "Fail";
            //List<CO_st_BuscarPeriodos_Result> listaPeridos = new List<CO_st_BuscarPeriodos_Result>();

            oLog.Add(nameController, "  MENSAJE:  Entra al Metodo  AddOrEdit Accion: " + modelSocios.AccionSend + "__" +
                        "__DATOS: " + modelSocios.partner_id + "__" +
                        modelSocios.rfc + "__" +
                        modelSocios.curp + "__" +
                        modelSocios.bank_id + "__" +
                        modelSocios.clabe + "__" +
                        modelSocios.regimenFiscal);

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {




                if (modelSocios.AccionSend == "Edit")
                {
                    try
                    {
                        //llama al SP que edita el registrio de Acuerdo a los datos pasados 
                        var R = hbxEntities.CO_st_ActualizaDatosPagoSocio(
                            modelSocios.partner_id,
                            modelSocios.rfc,
                            modelSocios.curp,
                            modelSocios.bank_id,
                            modelSocios.clabe,
                            modelSocios.regimenFiscal
                        // "EvoComisiones"
                        );

                    }
                    catch (Exception ex)
                    {
                        oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_ActualizaPeriodos  DETALLE: " + ex);
                    }
                }
                else if (modelSocios.AccionSend == "Add")
                {
                    oLog.Add(nameController, "Se deja bloque por sidespues se requiere Agregar Socio ya nomas es cuestion de agregar aqui el SP que realizara el insert");
                }
                else
                {
                    // No hace nada e imprime en el Archvo  log
                    oLog.Add(nameController, "  MENSAJE:  No llego la  AccionSend");
                }


            }



            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    //campos que Retorna: partner_id, tax_id, social_security, bank_id, account, Paymethod_com
                    listaSocios = hbxEntities.CO_st_BuscarDatosPagoSocio(modelSocios.partner_id).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_BuscarDatosPagoSocio  DETALLE: " + ex);
                }


            }


            return Json(listaSocios, JsonRequestBehavior.AllowGet);
            //return Json(mensaje_, JsonRequestBehavior.AllowGet);

            //return View();
        }



        //obtiene los Bancos para cargar combo bancos
        [HttpPost]
        public JsonResult getBancos(BancoViewModel banco)
        {
            //var revisar = prestamoData.idFormaPago; 
            List<CO_st_Listabancos_Result> comboBancos = new List<CO_st_Listabancos_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                comboBancos = hbxEntities.CO_st_Listabancos().ToList();
            }

            return Json(comboBancos, JsonRequestBehavior.AllowGet);
        }

        //obtiene los regimimes fiscales para cargar combo Regimen fiscal
        [HttpPost]
        public JsonResult getRegimenesFiscales(int pais_id)
        {
            //var revisar = prestamoData.idFormaPago; 
            List<CO_st_ListaRegimenFiscal_Result> comboRegimenesFisc = new List<CO_st_ListaRegimenFiscal_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {

                comboRegimenesFisc = hbxEntities.CO_st_ListaRegimenFiscal(pais_id).ToList();
            }

            return Json(comboRegimenesFisc, JsonRequestBehavior.AllowGet);
        }

    }//fin de la clase




}


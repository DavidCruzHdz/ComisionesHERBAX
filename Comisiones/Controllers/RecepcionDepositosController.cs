namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using Data;
    using SpreadsheetLight;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class RecepcionDepositosController : Controller
    {
        private string nameController = "RecepcionDepositosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // GET: RecepcionDepositos
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 8 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }


        [HttpPost]
        public ActionResult GuardarArchivo(HttpPostedFileBase archivoComisiones)
        {
            Log oLog = new Log(rutaArchivoLog);
            //indicadorEjecucion  en False indica que todo esta Bien
            //indicadorEjecucion  en True  indica que fallo 
            Boolean indicadorEjecucion = false;
            try
            {

                List<CO_tb_PagosRespuesta> pagosArray = new List<CO_tb_PagosRespuesta>();
                string pathArchivoServer = Path.Combine(@"C:\herbax\archivos\", Path.GetFileName(archivoComisiones.FileName));
                archivoComisiones.SaveAs(pathArchivoServer);

                try
                {
                    String rutaArchivo = @"C:\herbax\archivos\" + archivoComisiones.FileName;
                    SLDocument sl = new SLDocument(rutaArchivo);

                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        int iRow = 2;

                        while (!string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 1)))
                        {
                            string lsTipoRegistro = sl.GetCellValueAsString(iRow, 1).Trim();
                            string cuentaDestino = sl.GetCellValueAsString(iRow, 2).Trim();
                            string lsPartnerId = sl.GetCellValueAsString(iRow, 3).Trim();
                            string beneficiario = sl.GetCellValueAsString(iRow, 4).Trim();
                            string importe = sl.GetCellValueAsString(iRow, 5).Trim();
                            string lsNumeroReferencia = sl.GetCellValueAsString(iRow, 6).Trim();
                            string lsFechaAplicacion = sl.GetCellValueAsString(iRow, 7).Trim();
                            string claveRastreo = sl.GetCellValueAsString(iRow, 8).Trim();
                            string estatus = sl.GetCellValueAsString(iRow, 9).Trim();
                            string motivoDevolucion = sl.GetCellValueAsString(iRow, 10).Trim();
                            string lsPeriodo = sl.GetCellValueAsString(iRow, 11).Trim();


                            if (lsTipoRegistro.Equals("2"))
                            {
                                var pagos = new CO_tb_PagosRespuesta();

                                pagos.tiporeRegistro = Convert.ToInt32(lsTipoRegistro);
                                pagos.cuentaDestino = cuentaDestino;
                                pagos.partner_id = lsPartnerId;
                                pagos.beneficiario = beneficiario;

                                //Convert.ToDecimal(importe)
                                pagos.importe = Convert.ToDecimal(importe);

                                pagos.numeroReferencia = Convert.ToDecimal(lsNumeroReferencia);

                                if (!string.IsNullOrEmpty(lsFechaAplicacion))
                                {
                                    Int32 anno = Convert.ToInt32(lsFechaAplicacion.Substring(0, 4));
                                    Int32 mes = Convert.ToInt32(lsFechaAplicacion.Substring(4, 2));
                                    Int32 dia = Convert.ToInt32(lsFechaAplicacion.Substring(6, 2));

                                    DateTime fechaAplicacion = new DateTime(anno, mes, dia);
                                    pagos.fechaAplicacion = Convert.ToDateTime(fechaAplicacion);
                                }

                                pagos.claveRastreo = claveRastreo;
                                pagos.estatus = estatus;
                                pagos.motivoDevolucion = motivoDevolucion;
                                pagos.id_Periodo = Convert.ToInt32(lsPeriodo);

                                hbxEntities.CO_tb_PagosRespuesta.Add(pagos);
                                hbxEntities.SaveChanges();


                                logger.Info("PAGOS -> " + pagos.numeroReferencia + " [BENEFICIARIO]-> " + pagos.beneficiario + " [IMPORTE] -> " + pagos.importe);

                                pagosArray.Add(pagos);


                            } //end if tipoRegisto igual a 2


                            iRow++;
                        }//End while
                    }
                }
                catch (NullReferenceException ex)  // could happen if POST is interrupted
                {
                    indicadorEjecucion = true;
                    oLog.Add(nameController, "  MENSAJE: fallo al grabar datos en Tabla: CO_tb_PagosRespuesta  DETALLE: " + ex);
                    logger.Error("ERROR Carga Depositos-> " + ex);
                    //return Content("ERROR Carga Depositos " + ex); 
                }

                //si no hay error ejecuta el SP Evo_comisoiones.dbo.CO_st_verificarPagoComision_pr
                if (indicadorEjecucion == false)
                {
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        var respuesta = hbxEntities.CO_st_verificarPagoComision_pr();
                        oLog.Add(nameController, "  MENSAJE:  Respuesta del SP CO_st_verificarPagoComision_pr " + respuesta);
                    }
                }


                //elimina archivo si existe
                if (System.IO.File.Exists(pathArchivoServer))
                {
                    System.IO.File.Delete(pathArchivoServer);
                }

                /*
                var listModelComisionesSoc = Session["listComisionesSocio"] as List<Comisiones.Models.CO_tb_ComisionesSocio>;


                logger.Info("VALOR SESION -> " + listModelComisionesSoc);

                if (listModelComisionesSoc != null)
                {

                    foreach (var comisionesSocio in listModelComisionesSoc)
                    {
                        logger.Info("RECORRE SESION -> " + comisionesSocio.socio + " [MONTO]-> " + comisionesSocio.monto + " [OBSERVACIONES] -> " + comisionesSocio.observaciones);

                    }
                }
                */

                ///   return Content("1");
                ///return View("~/Views/ArchivoComisiones/");
                return Json(pagosArray);

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                oLog.Add(nameController, "  MENSAJE: " + dbEx);
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }//finMetodo


        /*Metodo que no se esta Utilizando por ahora   
           [HttpPost]
           public ActionResult GuardarArchivo2Old(HttpPostedFileBase archivoComisiones)
           {
               try
               {

                   List<CO_tb_RecepcionDepositos> depositosArray = new List<CO_tb_RecepcionDepositos>();


                   string pathArchivoServer = Path.Combine(@"C:\herbax\archivos\", Path.GetFileName(archivoComisiones.FileName));

                   //elimina archivo si existe
                   if (System.IO.File.Exists(pathArchivoServer))
                   {
                       System.IO.File.Delete(pathArchivoServer);
                   }

                   archivoComisiones.SaveAs(pathArchivoServer);


                   try
                   {
                       String rutaArchivo = @"C:\herbax\archivos\" + archivoComisiones.FileName;

                       logger.Info("LEER ARCHIVO " + rutaArchivo);

                       SLDocument sl = new SLDocument(rutaArchivo);


                       using (herbaxEntities hbxEntities = new herbaxEntities())
                       {

                           int iRow = 2;

                           while (!string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 1)))
                           {
                               string numeroReferencia = sl.GetCellValueAsString(iRow, 1).Trim();
                               string cuentaOrigen = sl.GetCellValueAsString(iRow, 2).Trim();
                               string cuentaDestino = sl.GetCellValueAsString(iRow, 3).Trim();
                               string beneficiario = sl.GetCellValueAsString(iRow, 4).Trim();
                               string importe = sl.GetCellValueAsString(iRow, 5).Trim();
                               string fecha = sl.GetCellValueAsString(iRow, 6).Trim();
                               string estatus = sl.GetCellValueAsString(iRow, 7).Trim();
                               string conceptoPagoTransf = sl.GetCellValueAsString(iRow, 8).Trim();
                               string referenciaInterban = sl.GetCellValueAsString(iRow, 9).Trim();
                               string formaAplicacion = sl.GetCellValueAsString(iRow, 10).Trim();
                               string fechaAplicacion = sl.GetCellValueAsString(iRow, 11).Trim();
                               string claveRastreo = sl.GetCellValueAsString(iRow, 12).Trim();
                               string motivoDevolucion = sl.GetCellValueAsString(iRow, 13).Trim();
                               string iva = sl.GetCellValueAsString(iRow, 14).Trim();
                               string rfc = sl.GetCellValueAsString(iRow, 15).Trim();
                               string comprobanteElectronico = sl.GetCellValueAsString(iRow, 16).Trim();
                               string divisa = sl.GetCellValueAsString(iRow, 17).Trim();


                               logger.Info("DEPOSITOS -> " + numeroReferencia +
                                           " [cuentaOrigen]-> " + cuentaOrigen +
                                           " [cuentaDestino] -> " + cuentaDestino +
                                           " [beneficiario] -> " + beneficiario +
                                           " [importe] -> " + importe +
                                           " [fecha] -> " + fecha +
                                           " [estatus] -> " + estatus +
                                           " [conceptoPagoTransf] -> " + conceptoPagoTransf +
                                           " [referenciaInterban] -> " + referenciaInterban +
                                           " [formaAplicacion] -> " + formaAplicacion +
                                           " [fechaAplicacion] -> {" + fechaAplicacion + "}  [" + fechaAplicacion.Length + "] " +
                                           " [claveRastreo] -> " + claveRastreo +
                                           " [motivoDevolucion] -> " + motivoDevolucion +
                                           " [iva] -> " + iva +
                                           " [rfc] -> " + rfc +
                                           " [comprobanteElectronico] -> " + comprobanteElectronico +
                                           " [cuentaDestino] -> " + cuentaDestino +
                                           " [divisa] -> " + divisa

                                           );


                               using (herbaxEntities evoEntities = new herbaxEntities())
                               {

                                   var depositos = new CO_tb_RecepcionDepositos();
                                   depositos.numeroReferencia = numeroReferencia;
                                   depositos.cuentaOrigen = cuentaOrigen;
                                   depositos.cuentaDestino = cuentaDestino;
                                   depositos.beneficiario = beneficiario;
                                   depositos.importe = Convert.ToDecimal(importe);

                                   if (!string.IsNullOrEmpty(fecha))
                                   {
                                       depositos.fecha = Convert.ToDateTime(fecha);
                                   }

                                   depositos.estatus = estatus;
                                   depositos.conceptoPagoTransf = conceptoPagoTransf;
                                   depositos.referenciaInterban = referenciaInterban;
                                   depositos.formaAplicacion = formaAplicacion;

                                   if (!string.IsNullOrEmpty(fechaAplicacion))
                                   {
                                       depositos.fechaAplicacion = Convert.ToDateTime(fechaAplicacion);
                                   }

                                   depositos.claveRastreo = claveRastreo;
                                   depositos.motivoDevolucion = motivoDevolucion;

                                   if (!string.IsNullOrEmpty(iva))
                                   {
                                       depositos.iva = Convert.ToDecimal(iva);
                                   }

                                   depositos.rfc = rfc;
                                   depositos.comprobanteElectronico = comprobanteElectronico;
                                   depositos.divisa = divisa;

                                   hbxEntities.CO_tb_RecepcionDepositos.Add(depositos);
                                   hbxEntities.SaveChanges();

                                   logger.Info("DEPOSITOS -> " + depositos.numeroReferencia + " [BENEFICIARIO]-> " + depositos.beneficiario + " [IMPORTE] -> " + depositos.importe);

                                   depositosArray.Add(depositos);
                               }

                               iRow++;
                           }
                       }

                   }
                   catch (NullReferenceException ex)  // could happen if POST is interrupted
                   {

                       logger.Error("ERROR Carga Depositos-> " + ex);

                       // perhaps log a warning here
                       return Content("ERROR Carga Depositos " + ex); ;
                   }


                   /*
                   var listModelComisionesSoc = Session["listComisionesSocio"] as List<Comisiones.Models.CO_tb_ComisionesSocio>;


                   logger.Info("VALOR SESION -> " + listModelComisionesSoc);

                   if (listModelComisionesSoc != null)
                   {

                       foreach (var comisionesSocio in listModelComisionesSoc)
                       {
                           logger.Info("RECORRE SESION -> " + comisionesSocio.socio + " [MONTO]-> " + comisionesSocio.monto + " [OBSERVACIONES] -> " + comisionesSocio.observaciones);

                       }
                   }
                   **

                   ///   return Content("1");
                   ///return View("~/Views/ArchivoComisiones/");
                   return Json(depositosArray);

               }
               catch (Exception e)
               {
                   logger.Error("Ocurrio un error -> " + e.Message);

                   return Content("Ocurrio un error :(" + e.Message + ")");
               }

           }
           */


    } //finClase
}//fin nameSPACE
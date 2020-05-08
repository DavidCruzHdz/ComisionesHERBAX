﻿namespace Comisiones.Controllers
{

    using Comisiones.Models;
    using Comisiones.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Web.Mvc;


    public class PrestamoController : Controller
    {
        // GET: CO_tb_Prestamo
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 4 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }
        //obtiene los numeros de pagos ya sean meses o quincenas esto dependera de la forma de pago
        [HttpPost]
        public JsonResult getNumerodePagos(PrestamoViewModel prestamoData)
        {
            //var revisar = prestamoData.idFormaPago; 
            List<CO_st_comboNumerodePagos_Result> comboNumerodePagos = new List<CO_st_comboNumerodePagos_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                comboNumerodePagos = hbxEntities.CO_st_comboNumerodePagos(prestamoData.idFormaPago).ToList();
            }

            return Json(comboNumerodePagos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getFormasdePago()
        {
            List<CO_st_Forma_de_pago_Result> formasPago = new List<CO_st_Forma_de_pago_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                formasPago = hbxEntities.CO_st_Forma_de_pago().ToList();
            }

            return Json(formasPago, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult crearPrestamo(PrestamoViewModel prestamoData)
        {
            string _respuesta = string.Empty;
            try
            {
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var prestamo = new CO_tb_Prestamo();

                            prestamo.partner_id = prestamoData.Partner_id.Trim(); //[varchar](10) NOT NULL,
                            prestamo.importe = prestamoData.Importe; // [decimal](18, 2) NOT NULL,

                            prestamo.numeroPagos = prestamoData.NumeroPagos;// [int] NOT NULL,
                            prestamo.id_Concepto = 12; //12,82   [int] NOT NULL,
                            prestamo.id_Periodo = prestamoData.id_Periodo;  // [int] NOT NULL,
                            prestamo.saldo = prestamoData.Importe; // [decimal](18, 2) NOT NULL, campo que probablemente se borre de bd

                            prestamo.fechaInicio = DateTime.Now;//prestamoData.fechaInicio; // [date]NULL,
                            prestamo.fechaFin = prestamoData.fechaFin; //[date]NULL,

                            prestamo.usuario = prestamoData.usuario; //[varchar](150) NULL,
                            prestamo.fechaMovimiento = DateTime.Now; //[smalldatetime]NULL,
                            prestamo.Motivo = prestamoData.motivo;//[varchar](100) NULL,
                            prestamo.idFormaPago = prestamoData.idFormaPago; //[int] 

                            hbxEntities.CO_tb_Prestamo.Add(prestamo);
                            hbxEntities.SaveChanges();


                            int _idPrestamoInsertado = prestamo.id_Prestamo;
                            int _idPeriodo = prestamoData.id_Periodo;
                            string _usuario = "usercomisiones";

                            var retorno = hbxEntities.CO_st_generaMovimientosPrestamo2(_idPrestamoInsertado, _idPeriodo, _usuario);

                            _respuesta = "ok";

                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
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

                    }
                    else
                    {
                        var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();


                        //return Redirect("Movimientos", "MovimientosSocio");
                    }
                } //fin using


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }

            return Json(_respuesta, JsonRequestBehavior.AllowGet);
        }//fin metodo

        /*
        [HttpPost]
        public JsonResult enviarSolicitudPrestamo(PrestamoViewModel solicitud)
        {
            solicitudPrestamo _validaCapacidadPago;
            //CO_st_validaCapacidadPago_Result _validaCapacidadPago;
            string _respuesta = string.Empty;

            _validaCapacidadPago = Data.Prestamo.dataPrestamo.instance.enviarSolicitudPrestamo(solicitud.Partner_id).FirstOrDefault();  //hbxEntities.CO_st_validaCapacidadPago(solicitud.Partner_id).FirstOrDefault();
            if (_validaCapacidadPago != null)
            {
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {

                    //if (_validaCapacidadPago.monto >= solicitud.Importe)
                    //{
                        var prestamo = new CO_tb_Prestamo();
                        prestamo.partner_id = solicitud.Partner_id;
                        prestamo.importe = solicitud.Importe;
                        prestamo.numeroPagos = solicitud.NumeroPagos;
                        prestamo.tipoAplicacion = solicitud.TipoAplicacion;
                        prestamo.id_Concepto = solicitud.IdConcepto;
                        prestamo.fechaMovimiento = DateTime.Now;
                        prestamo.Motivo = solicitud.motivo;
                        prestamo.idFormaPago = solicitud.idFormaPago;
                        prestamo.estatus = 0; // NUEVO
                        prestamo.id_Periodo = sesion.periodoActivo.Id_Periodo; // Periodo Activo
                        hbxEntities.CO_tb_Prestamo.Add(prestamo);
                        hbxEntities.SaveChanges();

                        var iIdPrestamo = (from p in hbxEntities.CO_tb_Prestamo
                                           where (p.partner_id == solicitud.Partner_id && p.importe == solicitud.Importe)
                                           select p.id_Prestamo
                                           ).FirstOrDefault();

                        _respuesta = "ok";

                    

                    string usrAutorizdor = "Mario Arcadio Hernandez Carrasco";
                        string strPeriodoPago = prestamo.numeroPagos + " pagos ";
                        if (solicitud.idFormaPago == 1)
                        { strPeriodoPago += "mensuales"; }
                        else if (solicitud.idFormaPago == 2)
                        {
                            strPeriodoPago += "quincenales";
                        }


                        var urlBuilderAutorizar =
                      new System.UriBuilder(Request.Url.AbsoluteUri)
                      {
                          Path = Url.Action("AutorizarPrestamo", "Prestamo"),
                          Query = null,
                      };

                        var urlBuilderDenegar =
                     new System.UriBuilder(Request.Url.AbsoluteUri)
                     {
                         Path = Url.Action("DenegarPrestamo", "Prestamo"),
                         Query = null,
                     };

                        string bodyMessage = "<table style='font-family: Arial, Helvetica, sans-serif; font-size: 12px;' > <tbody> <tr> <td> <p style='text-align: center;'> <img src='https://herbaxmexico.com/wp-content/uploads/2017/04/logo-pagina-vn2.png' alt='' width='120px' height='130px' /></p> </td> <td> <p style='text-align: center;'><strong>SOLICITUD DE PR&Eacute;STAMO </strong></p> </td> </tr> </tbody> </table> <p>&nbsp;</p> <p>Estimado <strong><u>" + usrAutorizdor + "</u></strong></p> <p>&nbsp;</p> <p>Por medio del presente hago llegar ante usted una solicitud para que me sea otorgado un pr&eacute;stamo de manera urgente para cubrir las necesidades de un imprevisto.</p> <p>Solicito el pr&eacute;stamo por la cantidad de&nbsp;<strong> " + prestamo.importe.ToString("C", new System.Globalization.CultureInfo("es-MX")) + " pesos</strong> comprometi&eacute;ndome a pagar la totalidad del mismo en un periodo de <strong>" + strPeriodoPago + ".</strong></p> <p>Si usted le parece conveniente le otorgo el derecho de descontarme la ganancia de mis ventas mensuales de productos Herbax.</p> <p>Esperando poder contar con su ayuda y compresi&oacute;n quedo atento a sus &oacute;rdenes.</p> <p>&nbsp;De antemano muchas gracias.</p> <p>&nbsp;</p> <p style='text-align: center;'><strong> SR(a) " + _validaCapacidadPago.nombreCompleto + " (" + solicitud.Partner_id + ")" + "</strong></p> <p style='text-align: center;'>&nbsp;</p> <p style='text-align: center;'><strong><table> <tr> <td> <a href='http://192.168.1.13/servicioComisiones/puenteConexion.aspx?webMethod=AutorizarPrestamo&idPrestamo=" + iIdPrestamo + "&idPartner=" + solicitud.Partner_id + "&accionSolicitud=1' >Autorizar</a> </td> <td>  </td><a href='http://192.168.1.13/servicioComisiones/puenteConexion.aspx?webMethod=AutorizarPrestamo&idPrestamo=" + iIdPrestamo + "&idPartner=" + solicitud.Partner_id + "&accionSolicitud=2' >Denegar</a> </tr> </table></strong></p> <p>&nbsp;</p>";
                        sendMail(_validaCapacidadPago.nombreCompleto, bodyMessage);
                /*    }
                    else
                    {
                        _respuesta = _validaCapacidadPago.Mensaje + _validaCapacidadPago.monto.ToString();
                    }
                    *
                    //Redirect(sesion.urlMain+ "/PrestamoSocio/");

                }
            }

            return Json(_respuesta, JsonRequestBehavior.AllowGet);

        }
 */
        /*
            [HttpPost]
            public ActionResult AutorizarPrestamo(PrestamoViewModel model)
            {

                return null;
            }

            [HttpPost]
            public ActionResult DenegarPrestamo(PrestamoViewModel model)
            {

                return null;
            }


        */
        [HttpPost]
        public ActionResult Nuevo(PrestamoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        var prestamo = new CO_tb_Prestamo();
                        /*prestamo.partner_id = model.Partner_id;
                        prestamo.importe = model.Importe;
                        prestamo.numeroPagos = model.NumeroPagos;
                        prestamo.tipoAplicacion = model.TipoAplicacion;
                        prestamo.id_Concepto = model.IdConcepto;
                        prestamo.fechaMovimiento = DateTime.Now;*/

                        hbxEntities.CO_tb_Prestamo.Add(prestamo);
                        hbxEntities.SaveChanges();

                        return Redirect("~/Prestamo/");


                    }
                }
                return View(model);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /*
                public ActionResult Editar(int Id)
                {
                    PrestamoViewModel prestamoViewModel = new PrestamoViewModel();

                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {

                        var prestamo = hbxEntities.CO_tb_Prestamo.Find(Id);

                        prestamoViewModel.Partner_id = prestamo.partner_id;
                        prestamoViewModel.Importe = (decimal)prestamo.importe;
                        prestamoViewModel.NumeroPagos = (int)prestamo.numeroPagos;
                        prestamoViewModel.TipoAplicacion = (int)prestamo.tipoAplicacion;
                        //prestamoViewModel.Id_prestamo = prestamo.id_Prestamo;
                    }

                    return View(prestamoViewModel);
                }

        */

        /*
                [HttpPost]
                public ActionResult Editar(PrestamoViewModel model)
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            using (herbaxEntities hbxEntities = new herbaxEntities())
                            {
                                var prestamo = hbxEntities.CO_tb_Prestamo.Find(model.Id_prestamo);

                                prestamo.partner_id = model.Partner_id;
                                prestamo.importe = model.Importe;
                                prestamo.numeroPagos = model.NumeroPagos;
                                prestamo.tipoAplicacion = model.TipoAplicacion;
                                prestamo.id_Concepto = model.IdConcepto;
                                prestamo.fechaMovimiento = DateTime.Now;

                                hbxEntities.Entry(prestamo).State = System.Data.Entity.EntityState.Modified;
                                hbxEntities.SaveChanges();

                                return Redirect("~/Prestamo/");
                            }
                        }
                        return View(model);

                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
        */

        [HttpGet]
        public ActionResult Eliminar(int Id)
        {
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                var prestamo = hbxEntities.CO_tb_Prestamo.Find(Id);
                hbxEntities.CO_tb_Prestamo.Remove(prestamo);

                hbxEntities.SaveChanges();
            }


            return Redirect("~/Prestamo/");


        }

        private void sendMail(string pNombreSocio, string pHtmlBody)
        {

            //Archivos a adjuntar
            List<String> strFiles = default(List<String>);
            Attachment data = null;

            //////Correo central de envios de solitudes
            var fromAddress = new MailAddress("hector.hernandez@grupovitek.com", "HERBAX de Mexico");
            string fromPassword = "1Vitek1961";

            ///Obtener el correo del autorizador de la solicitud
            var toAddress = new MailAddress("mhernandez@herbax.com.mx");


            ///Asunto del correo
            string subject = "Solicitud de préstamo";

            //Contenido
            string body = pHtmlBody;

            //Adjunto
            if (strFiles != null)
            {
                if (strFiles.Count > 0)
                {
                    data = new Attachment("pathFile", System.Net.Mime.MediaTypeNames.Application.Octet);
                    System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime("pathFile");
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime("pathFile");
                    disposition.ReadDate = System.IO.File.GetLastAccessTime("pathFile");
                }
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            //CC
            message.CC.Add(new MailAddress("mhernandez@herbax.com.mx"));
            message.CC.Add(new MailAddress("ammartinez@herbax.com.mx"));
            message.CC.Add(new MailAddress("valentin.santos@grupovitek.com.mx"));
            //message.CC.Add(new MailAddress("hecmar87@gmail.com"));
            //message.CC.Add(new MailAddress("valentin.vale8490@gmail.com"));

            if (strFiles != null)
            {
                if (strFiles.Count > 0)
                {
                    message.Attachments.Add(data);
                }
            }

            smtp.Send(message);

            //using (var message = new MailMessage(fromAddress, toAddress)
            //{
            //    Subject = subject,
            //    Body = body
            //})
            //{
            //    smtp.Send(message);
            //}

        }
    }

}
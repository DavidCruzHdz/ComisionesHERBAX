namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using Comisiones.Models.ViewModels;
    using Data;
    using Models.ModelsEvolution;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Web.Mvc;

    public class MovimientosSocioController : Controller
    {
        private string nameController = "MovimientosSocioController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private EvolutionEntities Evo = new EvolutionEntities();

        // GET: MovimientosSocio

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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 11 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }


        [HttpPost]
        public JsonResult buscarSocios(SociosViewModel_v1 socios)
        {
            Log oLog = new Log(rutaArchivoLog);


            List<CO_st_BuscarSocioMovimientos_Result> listaSocios = new List<CO_st_BuscarSocioMovimientos_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Procedimiento que retorna monto,partner_id,id_Periodo,estatus par amostrarlo en la tabla de comisiones cargadas
                    string codigoSocio = "";

                    if (!String.IsNullOrEmpty(socios.Partner_id))
                    {
                        codigoSocio = socios.Partner_id.PadLeft(10, '0');
                    }
                    listaSocios = hbxEntities.CO_st_BuscarSocioMovimientos(codigoSocio, socios.FirstName, socios.LastName).ToList();

                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE: fallo al buscarSocios en SP: CO_st_BuscarSocioMovimientos  DETALLE: " + ex);
                }
            }

            // return View(listaSocios);
            return Json(listaSocios, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getConceptosPorTipo(String idTipo)
        {
            Log oLog = new Log(rutaArchivoLog);
            List<ListConceptoViewModel> conceptosList = new List<ListConceptoViewModel>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    conceptosList = (from conceptos in hbxEntities.CO_tb_Conceptos
                                     where conceptos.tipo == idTipo
                                     && conceptos.estatus == 1
                                     select new ListConceptoViewModel
                                     {
                                         Id = conceptos.id_Concepto,
                                         descripcion = conceptos.descripcion
                                     }).OrderBy(c => c.descripcion).ToList();
                }
                catch (Exception ex) { oLog.Add(nameController, "  MENSAJE: fallo al consultar tabla by LinQ: CO_tb_Conceptos  DETALLE: " + ex); }
            }

            return Json(conceptosList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(MovimientosSocioViewModel model)
        {
            Log oLog = new Log(rutaArchivoLog);
            oLog.Add(nameController, "DATOS de entrada: "
            + "__partner: " + model.Partner_id
            + "__concepto: " + model.Id_Concepto
            + "__monto: " + Convert.ToDecimal(model.monto)
            + "__comentario: " + model.comentario
            + "__periodo: " + model.Id_Periodo);

            String DataModel = " Entra a Registrar Nuevo Movimiento DATOS"
            + "__partner: " + model.Partner_id
            + "__concepto: " + model.Id_Concepto
            + "__monto: " + model.monto
            + "__comentario: " + model.comentario
            + "__periodo: " + model.Id_Periodo;

            //this.enviarCorreo(DataModel);

            Int16 estatusNuevo = 1;
            string usuarioSistema = "usercomisiones";
            Int64 idMovimiento = 0;

            try
            {
                String lErrores = "sin Errores";
                var ArrayErrores = ModelState.Where(x => x.Value.Errors.Count > 0).
                    Select(x => new { x.Key, x.Value.Errors }).ToList();
                //var names = new List<string>() { "John", "Tom", "Peter" };
                foreach (var name in ArrayErrores)
                {
                    //Console.WriteLine(name);
                    lErrores = lErrores + "Campo: " + name.Key + "_,";
                }
                oLog.Add(nameController, " Campos con Error: " + lErrores);


                if (ModelState.IsValid)
                {
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {

                        //Validar si es grabable movimiento
                        var concepto = hbxEntities.CO_tb_Conceptos.Find(model.Id_Concepto);

                        var movimientosSocio = new CO_tb_MovimientosSocio();

                        movimientosSocio.partner_id = model.Partner_id;
                        movimientosSocio.id_Concepto = model.Id_Concepto;
                        movimientosSocio.monto = Convert.ToDecimal(model.monto);
                        movimientosSocio.comentario = model.comentario;
                        movimientosSocio.fechaRegistro = DateTime.Now;
                        movimientosSocio.estatus = estatusNuevo;
                        movimientosSocio.usuario = usuarioSistema;
                        movimientosSocio.fechaMovimiento = DateTime.Now; // fecha actual
                        movimientosSocio.Id_Periodo = model.Id_Periodo;
                        movimientosSocio.isDelete = estatusNuevo;

                        hbxEntities.CO_tb_MovimientosSocio.Add(movimientosSocio);
                        hbxEntities.SaveChanges();


                        idMovimiento = movimientosSocio.id_MovimientosSocio;

                        oLog.Add(nameController, " Registrado idMovimiento : " + idMovimiento + "   Partner_id: " + model.Partner_id);
                        //return Redirect("~/MovimientosSocio/Movimientos");
                        return RedirectToAction("Movimientos", "MovimientosSocio", new { PartnerId = movimientosSocio.partner_id, Id_Periodo = movimientosSocio.Id_Periodo, nombre = model.nombreSocio });
                    }
                }
                else
                {
                    oLog.Add(nameController, " ModelState.IsValid  FALSE");
                    var errors = ModelState.Select(x => x.Value.Errors)
                      .Where(y => y.Count > 0)
                      .ToList();


                    oLog.Add(nameController, " ModelState.IsValid  FALSE  : " + errors);
                    this.enviarCorreo(errors.ToString());
                    //return Redirect("Movimientos", "MovimientosSocio");
                }
            }
            catch (Exception ex)
            {
                oLog.Add(nameController, " mensaje:> " + ex.Message + " Excepcion:>> " + ex);
            }

            return View(model);
        }


        public ActionResult Movimientos(String PartnerId, String Id_Periodo, String Nombre)
        {

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_MovientosDetalle_Result> listaMovimientosDetalle = new List<CO_st_MovientosDetalle_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Procedimiento que retorna monto,partner_id,id_Periodo,estatus par amostrarlo en la tabla de comisiones cargadas
                    String codigoSocio = PartnerId.PadLeft(10, '0');
                    listaMovimientosDetalle = hbxEntities.CO_st_MovientosDetalle(codigoSocio).ToList();

                    List<Object> listaResult = new List<object>();

                    //listaResult[0] = listaMovimientosDetalle;
                    //listaResult[1] = Id_Periodo;
                    //listaResult[2] = PartnerId;
                    //listaResult[3] = listaMovimientosDetalle;

                    ViewBag.Id_Periodo = Id_Periodo;
                    ViewBag.codigoSocio = PartnerId;
                    ViewBag.nombreSocio = Nombre;

                    

                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE: fallo  al consultar: CO_st_MovientosDetalle  DETALLE: " + ex);
                }

            }


            // return Json(listaMovimientosDetalle, JsonRequestBehavior.AllowGet);

            return View(listaMovimientosDetalle);
        }

        public ActionResult Editar(int Id)
        {
            Log oLog = new Log(rutaArchivoLog);
            MovimientosSocioViewModel movSocViewModel = new MovimientosSocioViewModel();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    movSocViewModel = (from movimientosSoc in hbxEntities.CO_tb_MovimientosSocio
                                           ///join clientes in hbxEntities.CL_tb_Customer on movimientosSoc.partner_id equals clientes.partner_id
                                       join concepto in hbxEntities.CO_tb_Conceptos on movimientosSoc.id_Concepto equals concepto.id_Concepto
                                       where movimientosSoc.id_MovimientosSocio == Id
                                       select new MovimientosSocioViewModel
                                       {
                                           Partner_id = movimientosSoc.partner_id,
                                           // NombreDistribuidor = clientes.first_name,
                                           Id_MovimientosSocio = movimientosSoc.id_MovimientosSocio,
                                           Id_Concepto = movimientosSoc.id_Concepto,
                                           tipoConcepto = concepto.tipo,
                                           DescripcionConcepto = concepto.descripcion,
                                           monto = movimientosSoc.monto
                                       }).First();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE: fallo  al consultar  LinQ Editar by Id : CO_tb_MovimientosSocio  DETALLE: " + ex);
                }
            }

            return View(movSocViewModel);
        }


        [HttpPost]
        public ActionResult Editar(MovimientosSocioViewModel model)
        {
            Log oLog = new Log(rutaArchivoLog);
            try
            {
                if (ModelState.IsValid)
                {
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        var movimientosSocio = hbxEntities.CO_tb_MovimientosSocio.Find(model.Id_MovimientosSocio);

                        movimientosSocio.partner_id = model.Partner_id;
                        movimientosSocio.id_Concepto = model.Id_Concepto;
                        movimientosSocio.monto = model.monto;

                        hbxEntities.Entry(movimientosSocio).State = System.Data.Entity.EntityState.Modified;
                        hbxEntities.SaveChanges();

                        return RedirectToAction("Movimientos", "MovimientosSocio", new { PartnerId = movimientosSocio.partner_id, Id_Periodo = movimientosSocio.Id_Periodo });

                    }

                }
            }
            catch (Exception ex)
            {
                oLog.Add(nameController, "  MENSAJE: fallo  al consultar  LinQ  Editar by Model: CO_tb_MovimientosSocio  DETALLE: " + ex);
                throw new Exception(ex.Message);
            }


            return View(model);
        }

        [HttpGet]
        public ActionResult Eliminar(int Id)
        {
            Log oLog = new Log(rutaArchivoLog);
            string codigoSocio = "";
            int periodoActual = 0;

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    var movimientoSocio = hbxEntities.CO_tb_MovimientosSocio.Find(Id);

                    codigoSocio = movimientoSocio.partner_id;
                    periodoActual = movimientoSocio.Id_Periodo.HasValue ? movimientoSocio.Id_Periodo.Value : 0;

                    hbxEntities.CO_tb_MovimientosSocio.Remove(movimientoSocio);
                    hbxEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE: fallo  al Eliminar  LinQ  Elliminar by Id: CO_tb_MovimientosSocio  DETALLE: " + ex);
                    throw new Exception(ex.Message);
                }

            }

            var Socio = Evo.CL_tb_Customer.Where(x => (x.partner_id == codigoSocio)).FirstOrDefault();
            //ViewBag.Id_Periodo = periodoActual;
            //ViewBag.codigoSocio = codigoSocio;
            //ViewBag.nombreSocio = Socio.last_name + " " + Socio.first_name;
            var NomSocio = Socio.first_name + " " + Socio.last_name;

            return RedirectToAction("Movimientos", "MovimientosSocio", new { PartnerId = codigoSocio, Id_Periodo = periodoActual, Nombre = NomSocio });
        }

        public Boolean enviarCorreo(String Mensaje)
        {
            //para poder enviar correo desde gmail se habilita 
            //Acceso de aplicaciones poco seguras en https://myaccount.google.com/security
            Boolean envioCorreo = false;
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress("valentin.vale8490@gmail.com"));
            email.From = new MailAddress("valentin.vale8490@gmail.com");
            email.Subject = "nuevo Movimiento"; //"Asunto ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            email.Body = "Mensaje: " + Mensaje;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;



            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("valentin.vale8490@gmail.com", "");

            string output = null;

            try
            {
                smtp.Send(email);
                email.Dispose();
                output = "Corre electrónico fue enviado satisfactoriamente.";
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }

            Console.WriteLine(output);

            return envioCorreo;
        }


    }
}
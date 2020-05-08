namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using Comisiones.Models.ModelsEvolution;
    using Comisiones.Models.ViewModels;
    using Data;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.tool.xml;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    //using SelectPdf;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;


    public class ReporteEdodeCtaController : Controller
    {
        private string nameController = "ReporteEdodeCtaController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";

        // GET: ReporteEdodeCta
        public ActionResult Index(string partner_id, string Nombre, string Direccion, int? Anio, int? Mes)
        {
            herbaxEntities db = new herbaxEntities();
            var VarRol = 0;
            //if (Session["idRol"] != null)
            //{
            //    VarRol = int.Parse(Session["idRol"].ToString());
            //}
            //else
            //{
            //    return Redirect("~/Home/Login");
            //}


            //if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 15 && x.IdRol == VarRol))
            //{
            //    return Redirect("~/Home/Index");
            //}

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_EdoDeCuenta_get_Movimientos1_Result> EdoCtaMovimientos = new List<CO_st_EdoDeCuenta_get_Movimientos1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //EdoDeCuentaMovimientosViewModel modelConsulta = new EdoDeCuentaMovimientosViewModel();
                    EdoCtaMovimientos = hbxEntities.CO_st_EdoDeCuenta_get_Movimientos1(partner_id, Mes, Anio).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_EdoDeCuenta_get_Movimientos  DETALLE: " + ex);
                }


            }



            ViewBag.Message = "";
            ViewBag.IsPedido = 0;


            ViewBag.dropdownMes = Mes;


            EvolutionEntities Evo = new EvolutionEntities();

            var IdSocio = partner_id;
            var contar = 1;
            var AnioActual = DateTime.Now.Year;
            var AnioAntes = AnioActual - 10;


            //ViewBag.dropdownAnio = Anio;



            //var result = from p in Evo.CL_tb_Customer
            //             join a in Evo.CL_tb_Pais on p.country_id equals a.NombreCorto
            //             join r in Evo.CO_tb_Rank on p.RankLastPeriod_ID equals r.Rank_ID 
            //             where p.partner_id == IdSocio
            //             select new Cliente_EdoDeCuenta()
            //             {
            //                 partner_id = p.partner_id.ToString(),
            //                 Nombre = p.last_name + " " + p.first_name,
            //                 Direccion = p.Address_1 + " " + p.Address_3,
            //                 Ciudad = p.city_id,
            //                 Estado = p.state_id.ToString(),
            //                 Pais = a.Pais,
            //                 CP = p.PostalCode,
            //                 RFC = p.tax_id,
            //                 CDA = p.Address_2,
            //                 Grado = r.Rank
            //             };

            //var Datos = result.OrderBy(u => u.partner_id).ToList();


            //ViewBag.Socio = Datos.partner_id);
            //ViewBag.Nombre = Nombre;
            //ViewBag.Direccion = Direccion;
            //ViewBag.Ciudad = Ciudad;
            //ViewBag.Ciudad = Ciudad;
            //ViewBag.Ciudad = Ciudad;
            //ViewBag.Ciudad = Ciudad;
            //ViewBag.Ciudad = Ciudad;
            //ViewBag.Ciudad = Ciudad;



            //ViewBag.Socio = String.IsNullOrEmpty(partner_id) ? "IdSocio" : "";
            //ViewBag.Nombre = String.IsNullOrEmpty(p.partner_id) ? "IdSocio" : "";
            //ViewBag.Direccion = String.IsNullOrEmpty(partner_id) ? "IdSocio" : "";
            //ViewBag.Socio = String.IsNullOrEmpty(partner_id) ? "IdSocio" : ""
            //ViewBag.Anio = String.IsNullOrEmpty(Anio.ToString()) ? "Anio" : "";
            //ViewBag.Mes = String.IsNullOrEmpty(Mes.ToString()) ? "Mes" : "";
            //ViewBag.Periodo = String.IsNullOrEmpty(Periodo.ToString()) ? "Mes" : "";
            //ViewBag.FechaPedido = DateTime.Now;
            //ViewBag.Reembolso = ReembolsoSocio;
            //ViewBag.Diferencia = 0;




            //partner_id = p.partner_id.ToString(),
            //                 Nombre = p.last_name + " " + p.first_name,
            //                 Direccion = p.Address_1 + " " + p.Address_3,
            //                 Ciudad = p.city_id,
            //                 Estado = p.state_id.ToString(),
            //                 Pais = a.Pais,
            //                 CP = p.PostalCode,
            //                 RFC = p.tax_id,
            //                 CDA = p.Address_2,
            //                 Grado = r.Rank

            //ViewBag.Anio = String.IsNullOrEmpty(Anio.ToString()) ? "Anio" : "";
            //ViewBag.Mes = String.IsNullOrEmpty(Mes.ToString()) ? "Mes" : "";
            //ViewBag.Periodo = String.IsNullOrEmpty(Periodo.ToString()) ? "Mes" : "";
            //ViewBag.FechaPedido = DateTime.Now;
            //ViewBag.Reembolso = ReembolsoSocio;
            //ViewBag.Diferencia = 0;

            return View(EdoCtaMovimientos.ToList());
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                //return File(stream.ToArray(), "application/pdf", "Grid.pdf");
                return File(stream.ToArray(), "Archivos", "Grid.pdf");
            }
        }

        public object DocumentRetrieve(Dictionary<string, string> jsonResult)
        {
            string documentID = jsonResult["documentName"];
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["PdfDocument"].ConnectionString;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(constr);
            //searches for the PDF document from the Database
            var query = "select Data from PdfDocuments where DocumentName = '" + documentID + "'";
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query);
            cmd.Connection = con;
            con.Open();
            System.Data.SqlClient.SqlDataReader read = cmd.ExecuteReader();
            read.Read();
            return "data:application/pdf;base64," + read["Data"];
        }





        public JsonResult BuscaDistribuidor(string IdSocio)
        {
            herbaxEntities db = new herbaxEntities();
            EvolutionEntities Evo = new EvolutionEntities();
            Log oLog = new Log(rutaArchivoLog);
            var varFolio = "";
            var NumPartida = 0;

            try
            {
                //var DatosSocio =Evo.CL_tb_Customer.Where(s => (s.partner_id == IdSocio)).FirstOrDefault();
                var result = from p in Evo.CL_tb_Customer
                             join a in Evo.CL_tb_Pais on p.country_id equals a.NombreCorto
                             where p.partner_id == IdSocio
                             select new Cliente_EdoDeCuenta()
                             {
                                 partner_id = p.partner_id.ToString(),
                                 Nombre = p.last_name + " " + p.first_name,
                                 Direccion = p.Address_1 + " " + p.Address_3,
                                 Ciudad = p.city_id,
                                 Estado = p.state_id.ToString(),
                                 Pais = a.Pais,
                                 CP = p.PostalCode,
                                 RFC = p.tax_id,
                                 CDA = p.Address_2
                             };

                return Json(result.OrderBy(u => u.partner_id).ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_BuscaEstadosDeCuenta  DETALLE: " + ex);
            }


            return Json("", JsonRequestBehavior.AllowGet);

            //return View();
        }



        public JsonResult EdoDeCtaMovimientos(EdoDeCuentaMovimientosViewModel modelConsulta)
        {
            //CO_st_BuscarDatosPagoSocio
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_EdoDeCuenta_get_Movimientos1_Result> EdoCtaMovimientos = new List<CO_st_EdoDeCuenta_get_Movimientos1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //campos que Retorna: partner_id, tax_id, social_security, bank_id, account, Paymethod_com
                    EdoCtaMovimientos = hbxEntities.CO_st_EdoDeCuenta_get_Movimientos1(modelConsulta.partner_id, modelConsulta.Mes, modelConsulta.Anio).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_EdoDeCuenta_get_Movimientos  DETALLE: " + ex);
                }


            }
            return Json(EdoCtaMovimientos.ToList(), JsonRequestBehavior.AllowGet);

            //return View();
        }




        public byte[] GeneratePdfOutput(ControllerContext context, object model = null, string viewName = null,
       Action<PdfWriter, Document> configureSettings = null)
        {
            if (viewName == null)
            {
                viewName = context.RouteData.GetRequiredString("action");
            }

            context.Controller.ViewData.Model = model;

            byte[] output;
            using (var document = new Document())
            {
                using (var workStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                    writer.CloseStream = false;

                    if (configureSettings != null)
                    {
                        configureSettings(writer, document);
                    }
                    document.Open();


                    using (var reader = new StringReader(RenderRazorView(context, viewName)))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);

                        document.Close();
                        output = workStream.ToArray();
                    }
                }
            }
            return output;
        }


        public string RenderRazorView(ControllerContext context, string viewName)
        {
            IView viewEngineResult = ViewEngines.Engines.FindView(context, viewName, null).View;
            var sb = new StringBuilder();


            using (TextWriter tr = new StringWriter(sb))
            {
                var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                    context.Controller.TempData, tr);
                viewEngineResult.Render(viewContext, tr);
            }
            return sb.ToString();
        }

    }//fin de la clase
}

using Comisiones.Data;
using Comisiones.Models;
using Comisiones.Models.ModelsEvolution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using SpreadsheetLight;
using System.IO;
using Comisiones.Models.ViewModels;
//using System.Text;


namespace Comisiones.Controllers
{    
    public class ComisionesController : Controller
    {
        private string nameController = "ComisionesController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();

        // GET: Comisiones
        public ActionResult Index()
        {
            //herbaxEntities db = new herbaxEntities();
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 7 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }


            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

            var IdPais = 1;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");

            ViewBag.dropdownCia = new SelectList(db.CO_tb_CompaniaPago.ToList(), "Id_CompaniaPago", "nombreCompania");

            return View();
        }

        public ActionResult DescargaArchivoAsimilado(string tipoConsulta, int compania, string tipoArchivo)
        {
            //tipoArchivo = Asimilados o Factura
            Log oLog = new Log(rutaArchivoLog);
            string filePath = @"C:\herbax\archivos\";
            //var strBuilder = new StringBuilder();

            //logger.Info("RUTA DOMINIO DIRECTORIO ->" + AppDomain.CurrentDomain.BaseDirectory);

            Int32 folioCalculo = 0;
            Int32 countReg = 2;
            string tipoRegEncabezado = "1";
            string tipoRegDatos = "2";
            string tipoRegControl = "3";
            string versionLayout = "A1";

            Int32 countMovimientos = 0;
            decimal importeTotalAbonos = 0;

            DateTime currentDate = DateTime.Now;
            String fechaActual = currentDate.ToString("yy/MM/dd");
            String fechaArchivo = currentDate.ToString("yyyyMMdd_hhmmss");


            // MemoryStream ms = new MemoryStream();
            SLDocument oSLDocument = new SLDocument();
                oSLDocument.SetCellValue(1, 1, "SOCIO");
                oSLDocument.SetCellValue(1, 2, "NOMBRE");
                oSLDocument.SetCellValue(1, 3, "PAIS");
                oSLDocument.SetCellValue(1, 4, "FORMA DE PAGO");
                oSLDocument.SetCellValue(1, 5, "RFC");
                oSLDocument.SetCellValue(1, 6, "CURP");
                oSLDocument.SetCellValue(1, 7, "CLABE");
                oSLDocument.SetCellValue(1, 8, "IMPORTE");
                oSLDocument.SetCellValue(1, 9, "IVA");
                oSLDocument.SetCellValue(1, 10, "NETO A PAGAR");
            try
            {
                //Se cambio la manera de consultar ya que EntityFramework no obtenia los datos
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    List<CO_st_NombreArchivoLComisiones_Result> objNombre = hbxEntities.CO_st_NombreArchivoLComisiones3(tipoConsulta, compania, tipoArchivo).ToList();
                    String nombreArchivo = objNombre[0].ArchivoXLS;
                    //nombreArchivo = nombreArchivo.Replace("YYYYMMDD_HHMMSS", fechaArchivo);

                    if (Directory.Exists(filePath))
                    {
                        filePath = filePath + nombreArchivo;
                    }
                    else
                    {
                        DirectoryInfo di = Directory.CreateDirectory(filePath);
                        filePath = filePath + nombreArchivo;
                    }

                    //if (System.IO.File.Exists(filePath))
                    //{
                    //    System.IO.File.Delete(filePath);
                    //}

                    List<CO_st_registrosPago_pr1_Result> listRegistrosPago = new List<CO_st_registrosPago_pr1_Result>();
                    listRegistrosPago = hbxEntities.CO_st_registrosPago_pr1(tipoConsulta, compania).ToList();

                    if (listRegistrosPago != null)
                    {
                        Int32 Nfila = 2;
                        System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                        System.Globalization.DateTimeFormatInfo format = c.DateTimeFormat;
                        decimal Sumaimporte = new decimal(0.0);
                        decimal iva = new decimal(0.0);


                        listRegistrosPago.ForEach(i =>
                        {
                            oSLDocument.SetCellValue(Nfila, 1, i.id_Distribuidor);
                            oSLDocument.SetCellValue(Nfila, 2, i.Nombre);
                            oSLDocument.SetCellValue(Nfila, 3, i.Pais);
                            oSLDocument.SetCellValue(Nfila, 4, i.RegimenFiscal);//forma de pago asimilado o factura
                            oSLDocument.SetCellValue(Nfila, 5, i.RFC);//
                            oSLDocument.SetCellValue(Nfila, 6, i.CURP);//
                            oSLDocument.SetCellValue(Nfila, 7, i.Clabe);// Clabe interbancaria
                            oSLDocument.SetCellValue(Nfila, 8, i.Importe);

                            //si es factura se agregan estas dos columnas al excel
                            if (tipoArchivo.Equals("Factura"))
                            {
                                oSLDocument.SetCellValue(Nfila, 9, Convert.ToDecimal(i.IvaDelimTotal)); //columna de IVA
                                oSLDocument.SetCellValue(Nfila, 10, i.Importe + Convert.ToDecimal(i.IvaDelimTotal)); //neto a Pagar
                            }
                            Sumaimporte = Sumaimporte + i.Importe;
                            iva = Convert.ToDecimal(i.IvaDelimTotal);

                            Nfila++;
                        });

                        string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                        for (int _index = 0; _index < _lstColumns.Length; _index++)
                        {
                            SLStyle style1 = oSLDocument.CreateStyle();
                            style1.Font.FontName = "Arial";
                            style1.Font.FontSize = 11;
                            style1.Font.FontColor = System.Drawing.Color.White;
                            //style1.SetGradientFill(SLGradientShadingStyleValues.Horizontal2, SLThemeColorIndexValues.Accent3Color, SLThemeColorIndexValues.Accent3Color);
                            style1.SetGradientFill(SLGradientShadingStyleValues.Horizontal2, System.Drawing.Color.LightSeaGreen, System.Drawing.Color.MediumAquamarine);
                            style1.Font.Bold = true;
                            //style.Font.Italic = true;
                            //style.Font.Strike = true;
                            //style.Font.Underline = UnderlineValues.Double;
                            oSLDocument.SetCellStyle(_lstColumns[_index] + "1", style1);
                            style1 = null;

                            ///Detalle
                            for (int _indexDet = 2; _indexDet < listRegistrosPago.Count + 2; _indexDet++)
                            {
                                SLStyle style = oSLDocument.CreateStyle();
                                style.Font.FontName = "Arial";
                                style.Font.FontSize = 9;
                                style.Font.FontColor = System.Drawing.Color.DimGray;
                                style.Font.Bold = true;
                                oSLDocument.SetCellStyle(_lstColumns[_index] + _indexDet, style);
                                style = null;
                            }
                        }


                        CO_tb_Parametro parametro = new CO_tb_Parametro();
                        // buscar folio actual del proceso de calculo y generación archivos de pago
                        using (herbaxEntities herbaxEntities = new herbaxEntities())
                        {

                            parametro = (from param in herbaxEntities.CO_tb_Parametro
                                         where param.parametro == "FOLIO_CALCULO"
                                         select param).SingleOrDefault();
                        }


                        //PARA CUANDO ES ASIMILADO
                        if (tipoArchivo.Equals("Asimilados"))
                        {
                            decimal montoiva = Sumaimporte * Convert.ToDecimal(iva);
                            oSLDocument.SetCellValue(Nfila + 1, 7, "SUBTOTAL");
                            oSLDocument.SetCellValue(Nfila + 1, 8, Sumaimporte);
                            oSLDocument.SetCellValue(Nfila + 2, 7, "IVA");
                            oSLDocument.SetCellValue(Nfila + 2, 8, montoiva);//new Decimal(0.16));
                            oSLDocument.SetCellValue(Nfila + 3, 7, "TOTAL");
                            oSLDocument.SetCellValue(Nfila + 3, 8, Sumaimporte + montoiva); //* new Decimal(0.16));
                        }
                        //PARA CUANDO ES FACTURA
                        //la validacion se hizo al momento de iterar los registros ya que se calcula de manera individual


                        ///HHM
                        //SLConditionalFormatting cf = new SLConditionalFormatting(2, 1, countReg, 19);
                        //cf.SetDataBar(SLConditionalFormatDataBarValues.LightBlue);
                        //oSLDocument.AddConditionalFormatting(cf);

                        //SLPageSettings ps = new SLPageSettings();
                        //SLFont ft = oSLDocument.CreateFont();
                        //ft.SetFont("Arial", 16);
                        //ft.SetFontThemeColor(SLThemeColorIndexValues.Accent1Color);

                        
                        listRegistrosPago = null;
                        oSLDocument.SaveAs(filePath);

                    }//fin del si trae datos
                    //else
                    //{
                    //    oSLDocument.SetCellValue(2, 1, "No se encontraron datos");
                    //    oSLDocument.SaveAs(filePath);
                    //}
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());

                oLog.Add(nameController, "  MENSAJE:  fallo al ejecutar descarga listado de Reembolsos SP CO_st_ListaReembolsos DETALLE: " + ex);
                //Console.WriteLine(ex.InnerException.Message.ToString());
                ViewBag.Message = ex.InnerException.Message.ToString();

                oSLDocument.SetCellValue(1, 1, "Error al procesar");
                oSLDocument.SetCellValue(1, 2, "Error al procesar");
                oSLDocument.SetCellValue(1, 3, "Error al procesar");
                oSLDocument.SetCellValue(1, 4, "Error al procesar");
                oSLDocument.SetCellValue(1, 5, "Error al procesar");
                oSLDocument.SetCellValue(1, 6, "Error al procesar");
                oSLDocument.SetCellValue(1, 7, "Error al procesar");
                oSLDocument.SetCellValue(1, 8, "Error al procesar");
                oSLDocument.SetCellValue(1, 9, "Error al procesar");
                oSLDocument.SetCellValue(1, 10, "Error al procesar");
                oSLDocument.SaveAs(filePath);

            }

            ViewBag.IsOK = 0;

            return File(filePath, "application/force-download", Path.GetFileName(filePath));
            //return File(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(filePath));
            
            //return new FileContentResult(bindata, "application/vnd.ms-excel")
            //{
            //    FileDownloadName = "mytestfile.xls")
            //};
        }



        public JsonResult RegimenSelectList(int IdPais)
        {
            var RegimenList = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");
            return Json(RegimenList, JsonRequestBehavior.AllowGet);
        }

    }
}
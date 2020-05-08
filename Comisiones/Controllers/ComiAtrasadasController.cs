namespace Comisiones.Controllers
{
    /*using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;*/

    using Comisiones.Models;
    using Data;
    using SpreadsheetLight;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    public class ComiAtrasadasController : Controller
    {
        private string nameController = "ComiAtrasadasController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 9 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }


            return View();
        }


        public JsonResult ListarComiAtrasadas()
        {

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_registrosPagoAtrasadas_pr_Result> LComiAtrasadas = new List<CO_st_registrosPagoAtrasadas_pr_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    //Obtiene las comisiones atrasadas
                    LComiAtrasadas = hbxEntities.CO_st_registrosPagoAtrasadas_pr().ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_registrosPagoAtrasadas_pr  DETALLE: " + ex);
                }


            }
            return Json(LComiAtrasadas, JsonRequestBehavior.AllowGet);

            //return View();
        }


        public ActionResult descargaComiAtrasadas(string pais)
        {
            Log oLog = new Log(rutaArchivoLog);
            string filePath = @"C:\herbax\archivos\";
            //string nombreArchivo = "detalleComisiones.xlsx";

            DateTime currentDate = DateTime.Now;

            String fechaActual = currentDate.ToString("yy/MM/dd");
            String fechaArchivo = currentDate.ToString("yyyyMMdd_hhmmss");




            //Creando documento excel
            SLDocument oSLDocument = new SLDocument();

            //Definiendo encabezado (fila,columna, nombrecolumna)
            oSLDocument.SetCellValue(1, 1, "Tipo de Registro");
            oSLDocument.SetCellValue(1, 2, "No de Identificación del archivo");
            oSLDocument.SetCellValue(1, 3, "Fecha de Envio");
            oSLDocument.SetCellValue(1, 4, "Versión del Layout");
            oSLDocument.SetCellValue(1, 5, "Tipo de Registro");
            oSLDocument.SetCellValue(1, 6, "RFC");
            oSLDocument.SetCellValue(1, 7, "CURP");
            oSLDocument.SetCellValue(1, 8, "Email");
            oSLDocument.SetCellValue(1, 9, "Importe");
            oSLDocument.SetCellValue(1, 10, "Nombre");
            oSLDocument.SetCellValue(1, 11, "Clabe");
            oSLDocument.SetCellValue(1, 12, "Banco");
            oSLDocument.SetCellValue(1, 13, "Cuenta");
            oSLDocument.SetCellValue(1, 14, "Periodo");
            oSLDocument.SetCellValue(1, 15, "Id_Distribuidor");
            oSLDocument.SetCellValue(1, 16, "Tipo de Registro");
            oSLDocument.SetCellValue(1, 17, "Numero de Movimientos");
            oSLDocument.SetCellValue(1, 18, "Importe total de abonos");
            oSLDocument.SetCellValue(1, 19, "Iva del importe total");



            oLog.Add(nameController, "paso el encabezado Excel");

            try
            {


                using (herbaxEntities hbxEntities = new herbaxEntities())
                {

                    //string lstArchivo = 
                    List<CO_st_NombreArchivoComiAtrasadas_Result> objNombre = hbxEntities.CO_st_NombreArchivoComiAtrasadas().ToList();
                    String nombreArchivo = objNombre[0].nombreArchivo;
                    nombreArchivo = nombreArchivo.Replace("YYYYMMDD_HHMMSS", fechaArchivo);

                    filePath = filePath + nombreArchivo;

                    //revisando: si existe el archivo en el Directorio lo borra.
                    /*if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }*/

                    List<CO_st_registrosPagoAtrasadas_pr_Result> LComiAtrasadas = new List<CO_st_registrosPagoAtrasadas_pr_Result>();
                    LComiAtrasadas = hbxEntities.CO_st_registrosPagoAtrasadas_pr().ToList();

                    if (LComiAtrasadas != null)
                    {
                        oLog.Add(nameController, "LComiAtrasadas  es != null");
                        //listRegistrosPago = listRegistrosPago1.ToList();

                        Int32 Nfila = 2;
                        System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                        System.Globalization.DateTimeFormatInfo format = c.DateTimeFormat;

                        LComiAtrasadas.ForEach(i =>
                        {
                            /*oLog.Add(nameController, " fecha Envio: " + i.FechaEnvio.ToString());
                            oLog.Add(nameController,"dia:"+ i.FechaEnvio.ToString().Substring(0,2));
                            oLog.Add(nameController, "Mes:"+i.FechaEnvio.ToString().Substring(3, 2));
                            oLog.Add(nameController,"anio:"+ i.FechaEnvio.ToString().Substring(6, 4));*/
                            /*var anio = i.FechaEnvio.ToString().Substring(6, 4);
                            var mes = i.FechaEnvio.ToString().Substring(3, 2);
                            var dia = i.FechaEnvio.ToString().Substring(0, 2);*/
                            oSLDocument.SetCellValue(Nfila, 1, i.TipoRegistro);
                            oSLDocument.SetCellValue(Nfila, 2, i.NoIdentificaciónArchivo);
                            oSLDocument.SetCellValue(Nfila, 3, Convert.ToDateTime(i.FechaEnvio).ToString("yyyy/MM/dd"));//new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), Convert.ToInt32(dia)).ToString("yyyy/MM/dd"));// i.FechaEnvio);
                            oSLDocument.SetCellValue(Nfila, 4, i.VersionLayout);
                            oSLDocument.SetCellValue(Nfila, 5, i.TipoRegistroDet);
                            oSLDocument.SetCellValue(Nfila, 6, i.RFC);
                            oSLDocument.SetCellValue(Nfila, 7, i.CURP);
                            oSLDocument.SetCellValue(Nfila, 8, i.email);
                            oSLDocument.SetCellValue(Nfila, 9, i.Importe);
                            oSLDocument.SetCellValue(Nfila, 10, i.Nombre);
                            oSLDocument.SetCellValue(Nfila, 11, i.Clabe);
                            oSLDocument.SetCellValue(Nfila, 12, i.Banco);
                            oSLDocument.SetCellValue(Nfila, 13, i.Cuenta);
                            oSLDocument.SetCellValue(Nfila, 14, Convert.ToString(i.Periodo));
                            oSLDocument.SetCellValue(Nfila, 15, i.id_Distribuidor);
                            oSLDocument.SetCellValue(Nfila, 16, i.TipoRegistroCtrl);
                            oSLDocument.SetCellValue(Nfila, 17, Convert.ToString(i.NoMovimientos));
                            oSLDocument.SetCellValue(Nfila, 18, i.ImpTotalAbonos);
                            oSLDocument.SetCellValue(Nfila, 19, Convert.ToString(i.IvaDelimTotal));



                            Nfila++;
                        });


                        string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S" };
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
                            for (int _indexDet = 2; _indexDet < LComiAtrasadas.Count + 2; _indexDet++)
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

                        LComiAtrasadas = null;

                        oSLDocument.SaveAs(filePath);
                    }
                }


            }
            catch (Exception ex)
            {
                oLog.Add(nameController, "  MENSAJE:  fallo al ejecutar descarga Comisiones Atrasadas DETALLE: " + ex);
                Console.WriteLine(ex.ToString());

                oSLDocument.SetCellValue(1, 1, "Error al procesar");
                oSLDocument.SetCellValue(1, 2, "Error al procesar");
                oSLDocument.SetCellValue(1, 3, "Error al procesar");
                oSLDocument.SetCellValue(1, 4, "Error al procesar");
                oSLDocument.SetCellValue(1, 5, "Error al procesar");
                oSLDocument.SetCellValue(1, 6, "Error al procesar");
                oSLDocument.SetCellValue(1, 7, "Error al procesar");
                oSLDocument.SetCellValue(1, 8, "Error al procesar");


                oSLDocument.SaveAs(filePath);

            }

            return File(filePath, "application/force-download", Path.GetFileName(filePath));
        }


    }//fin de la clase




}


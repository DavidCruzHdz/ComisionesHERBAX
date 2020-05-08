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

    public class ListaPrestamosController : Controller
    {
        private string nameController = "ListaPrestamosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 13 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            return View();
        }


        public JsonResult ListarPrestamos()
        {

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_ListaPrestamos_Result> LPrestamos = new List<CO_st_ListaPrestamos_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    //Obtiene las comisiones atrasadas
                    LPrestamos = hbxEntities.CO_st_ListaPrestamos().ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_ListaPrestamos  DETALLE: " + ex);
                }


            }
            return Json(LPrestamos, JsonRequestBehavior.AllowGet);

            //return View();
        }


        public ActionResult descargaListaPrestamos(string pais)
        {
            Log oLog = new Log(rutaArchivoLog);
            string filePath = @"C:\herbax\archivos\";
            //string nombreArchivo = "detalleComisiones.xlsx";

            DateTime currentDate = DateTime.Now;

            String fechaActual = currentDate.ToString("yy/MM/dd");
            String fechaArchivo = currentDate.ToString("yyyyMMdd_hhmmss");

            //Campos que vienen en SP
            //periodoCreacion,partner_id,nombre,importe,saldo,Motivo,idFormaPago,descFormaPago

            //Creando documento excel
            SLDocument oSLDocument = new SLDocument();
            //Definiendo encabezado (fila,columna, nombrecolumna)
            oSLDocument.SetCellValue(1, 1, "Périodo Creación");
            oSLDocument.SetCellValue(1, 2, "Partner id");
            oSLDocument.SetCellValue(1, 3, "Nombre");
            oSLDocument.SetCellValue(1, 4, "Importe");
            oSLDocument.SetCellValue(1, 5, "Saldo");
            oSLDocument.SetCellValue(1, 6, "Motivo");
            oSLDocument.SetCellValue(1, 7, "Forma pago Id");
            oSLDocument.SetCellValue(1, 8, "Forma de Pago");


            try
            {


                using (herbaxEntities hbxEntities = new herbaxEntities())
                {

                    List<CO_st_NombreArchivoLPrestamos_Result> objNombre = hbxEntities.CO_st_NombreArchivoLPrestamos().ToList();
                    String nombreArchivo = objNombre[0].nombreArchivo;
                    nombreArchivo = nombreArchivo.Replace("YYYYMMDD_HHMMSS", fechaArchivo);

                    filePath = filePath + nombreArchivo;

                    //revisando: si existe el archivo en el Directorio lo borra.
                    /*if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }*/

                    List<CO_st_ListaPrestamos_Result> LPrestamos = new List<CO_st_ListaPrestamos_Result>();
                    LPrestamos = hbxEntities.CO_st_ListaPrestamos().ToList();

                    if (LPrestamos != null)
                    {

                        Int32 Nfila = 2;
                        System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                        System.Globalization.DateTimeFormatInfo format = c.DateTimeFormat;

                        LPrestamos.ForEach(i =>
                        {
                            //Campos que vienen en SP
                            //periodoCreacion,partner_id,nombre,importe,saldo,Motivo,idFormaPago,descFormaPago
                            oSLDocument.SetCellValue(Nfila, 1, i.periodoCreacion);
                            oSLDocument.SetCellValue(Nfila, 2, i.partner_id);
                            oSLDocument.SetCellValue(Nfila, 3, i.nombre);
                            oSLDocument.SetCellValue(Nfila, 4, i.importe);
                            oSLDocument.SetCellValue(Nfila, 5, i.saldo);
                            oSLDocument.SetCellValue(Nfila, 6, i.Motivo);
                            oSLDocument.SetCellValue(Nfila, 7, Convert.ToInt32(i.idFormaPago));
                            oSLDocument.SetCellValue(Nfila, 8, i.descFormaPago);



                            Nfila++;
                        });


                        string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F", "G", "H" };
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
                            for (int _indexDet = 2; _indexDet < LPrestamos.Count + 2; _indexDet++)
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

                        LPrestamos = null;

                        oSLDocument.SaveAs(filePath);
                    }
                }



            }
            catch (Exception ex)
            {
                oLog.Add(nameController, "  MENSAJE:  fallo al ejecutar descarga listado de Prestamos SP CO_st_ListaPrestamos DETALLE: " + ex);
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


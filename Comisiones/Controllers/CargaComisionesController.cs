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

    public class CargaComisionesController : Controller
    {
        private string nameController = "CargaComisionesController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // GET: ArchivoComisiones
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


        /*Metodo getDetalleComisiones Muestra el detalle
          de las comisones segun su estatus
        */
        public JsonResult getDetalleComisiones(string pais)
        {
            Log oLog = new Log(rutaArchivoLog);
            //partner_id.ToString().PadLeft(Convert.ToInt32(10), '0');

            //int Estatus = (int)idEstatus;

            List<CO_st_ComisionesDetallePais_Result> tbldetalleComisiones = new List<CO_st_ComisionesDetallePais_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Procedimiento que retorna monto,partner_id,id_Periodo,estatus par amostrarlo en la tabla de comisiones cargadas
                    tbldetalleComisiones = hbxEntities.CO_st_ComisionesDetallePais(pais).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Respuesta del SP CO_st_ComisionesDetallePais  DETALLE: " + ex);
                }
            }

            return Json(tbldetalleComisiones, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Cargar()
        [HttpPost]
        public ActionResult Cargar(HttpPostedFileBase archivoComisiones)
        {
            Log oLog = new Log(rutaArchivoLog);
            oLog.Add(nameController, "  MENSAJE: LLega Peticion");

            DateTime fechaActual = DateTime.Today;
            Int32 statusjecucion = 999;
            List<CO_st_GetComisionesPeriodoActivo1_Result> lista = new List<CO_st_GetComisionesPeriodoActivo1_Result>();

            try
            {
                string pathArchivoServer = Path.Combine(@"C:\herbax\archivos\", Path.GetFileName(archivoComisiones.FileName));
                archivoComisiones.SaveAs(pathArchivoServer);
                //string path = Path.Combine(Server.MapPath("~/Archivos"), Path.GetFileName(archivoComisiones.FileName));
                //archivoComisiones.SaveAs(path); 
                oLog.Add(nameController, "  MENSAJE: Ruta donde se guarda Temporal el Archivo: " + pathArchivoServer);
                //string pathArchivoServer = Path.GetFileName(archivoComisiones.FileName);
                String rutaArchivo = pathArchivoServer; //Path.Combine(Server.MapPath("~/Archivos"), Path.GetFileName(archivoComisiones.FileName));//@"C:\herbax\archivos\" + archivoComisiones.FileName;
                oLog.Add(nameController, "  MENSAJE: Ruta donde se esta leyendo el: " + rutaArchivo);


                SLDocument sl = new SLDocument(rutaArchivo);
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    int iRow = 2;

                    while (!string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 1)))
                    {
                        int distID = sl.GetCellValueAsInt32(iRow, 1);//DistID
                        string calcDetail = sl.GetCellValueAsString(iRow, 2).Trim();//CalcDetail
                        decimal amount = sl.GetCellValueAsDecimal(iRow, 3);//Amount
                        int sponsorID = sl.GetCellValueAsInt32(iRow, 4);//SponsorID
                        Decimal Porcentages = sl.GetCellValueAsDecimal(iRow, 5);//Percentages
                        Decimal volume = sl.GetCellValueAsDecimal(iRow, 6);//Volume
                        DateTime periodStartDate = sl.GetCellValueAsDateTime(iRow, 7);
                        DateTime periodEndDate = sl.GetCellValueAsDateTime(iRow, 8);//PeriodEndDate

                        var tblBonos = hbxEntities.CO_st_cargatmp_CO_tb_Comisiones(distID, calcDetail, amount, sponsorID, Porcentages, volume, periodStartDate, periodEndDate);

                        iRow++;
                    }//End while

                }



                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    statusjecucion = hbxEntities.CO_st_extraerComisones_pr();
                    oLog.Add(nameController, "  MENSAJE:estatus de ejecucion CO_st_extraerComisones_pr " + statusjecucion);

                    if (statusjecucion != 999)
                    {
                        if (statusjecucion != -1)
                        {
                            oLog.Add(nameController, "  MENSAJE: INICIA bloque CO_st_GetComisionesPeriodoActivo");
                            //lista =  hbxEntities.CO_st_extraerComisones_pr();
                            lista = hbxEntities.CO_st_GetComisionesPeriodoActivo1().ToList();
                            oLog.Add(nameController, "  MENSAJE:FIN bloque CO_st_GetComisionesPeriodoActivo" + lista);
                        }
                    }
                }

                //elimina archivo si existe
                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Delete(rutaArchivo);
                    oLog.Add(nameController, "  MENSAJE: Eliminando Archivo " + rutaArchivo);
                }

            }
            catch (Exception e)
            {
                //Log oLog = new Log(rutaArchivoLog);
                oLog.Add(nameController, "  MENSAJE: fallo al ejecutar Cargar()    DETALLE" + e);
            }

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        /*Metodo que carga las comisiones, despues de guardar las muestra una sumatoria por Estatus*/
        /*        [HttpPost]
                public ActionResult Cargar()
                {

                    DateTime fechaActual = DateTime.Today;
                    Int32 statusjecucion = 999;

                    try
                    {

                        List<CO_st_GetComisionesPeriodoActivo_Result> lista = new List<CO_st_GetComisionesPeriodoActivo_Result>();        

                        logger.Info("Antes de buscar periodo actual");


                            using (herbaxEntities hbxEntities = new herbaxEntities())
                            {

                                    statusjecucion = hbxEntities.CO_st_extraerComisones_pr();
                                statusjecucion = 1;

                                if (statusjecucion != 999)
                                {
                                    if (statusjecucion != -1)
                                    {
                                    //lista =  hbxEntities.CO_st_extraerComisones_pr();
                                    lista = hbxEntities.CO_st_GetComisionesPeriodoActivo().ToList();
                                }
                                }
                            } //end entity

                            return Json(lista, JsonRequestBehavior.AllowGet);
                            //return Json("success");



                    }
                    catch (Exception e)
                    {

                        logger.Error("ERROR -> " + e);
                        return Json("error", "No existe periodo activo para fecha " + fechaActual.ToString("dd/MM/yyyy"));
                    }
                }
*/

        public ActionResult descargaDetalleComisiones(string pais)
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
            oSLDocument.SetCellValue(1, 1, "Socio");
            oSLDocument.SetCellValue(1, 2, "Monto USD");
            oSLDocument.SetCellValue(1, 3, "Tipo Cambio");
            oSLDocument.SetCellValue(1, 4, "Monto Moneda Local");
            oSLDocument.SetCellValue(1, 5, "Estatus");

            try
            {

                //List<CO_st_registrosPago_pr_Result> listRegistrosPago = new List<CO_st_registrosPago_pr_Result>();

                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    List<string> lstArchivo = hbxEntities.CO_st_NombreArchivoDetcom(pais).ToList();
                    String nombreArchivo = lstArchivo.ElementAt(0);
                    nombreArchivo = nombreArchivo.Replace("YYYYMMDD_HHMMSS", fechaArchivo);

                    filePath = filePath + nombreArchivo;

                    //revisando: si existe el archivo en el Directorio lo borra.
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    List<CO_st_ComisionesDetallePais_Result> tbldetalleComisiones = hbxEntities.CO_st_ComisionesDetallePais(pais).ToList();

                    if (tbldetalleComisiones != null)
                    {
                        //listRegistrosPago = listRegistrosPago1.ToList();

                        Int32 Nfila = 2;
                        System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                        System.Globalization.DateTimeFormatInfo format = c.DateTimeFormat;

                        tbldetalleComisiones.ForEach(i =>
                        {
                            oSLDocument.SetCellValue(Nfila, 1, i.nombreCustomer);
                            oSLDocument.SetCellValue(Nfila, 2, i.MontoUSD);
                            oSLDocument.SetCellValue(Nfila, 3, i.TipoCambio.ToString());
                            oSLDocument.SetCellValue(Nfila, 4, i.MontoMonedaLocal);
                            oSLDocument.SetCellValue(Nfila, 5, i.estatus);

                            Nfila++;
                        });


                        string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F" };
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
                            for (int _indexDet = 2; _indexDet < tbldetalleComisiones.Count + 2; _indexDet++)
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

                        tbldetalleComisiones = null;

                        oSLDocument.SaveAs(filePath);
                    }
                }


            }
            catch (Exception ex)
            {
                oLog.Add(nameController, "  MENSAJE:  fallo al ejecutar descargaDetalleComisiones  DETALLE: " + ex);
                Console.WriteLine(ex.ToString());

                oSLDocument.SetCellValue(1, 1, "Error al procesar");
                oSLDocument.SetCellValue(1, 2, "Error al procesar");
                oSLDocument.SetCellValue(1, 3, "Error al procesar");
                oSLDocument.SetCellValue(1, 4, "Error al procesar");
                oSLDocument.SetCellValue(1, 5, "Error al procesar");
                oSLDocument.SaveAs(filePath);

            }

            return File(filePath, "application/force-download", Path.GetFileName(filePath));
        }
    }
}
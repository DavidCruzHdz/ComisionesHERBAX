using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Comisiones.Data;
using Comisiones.Models;
using System.Data;
using System.Data.Entity;
using System.Net;
using Comisiones.Models.ModelsEvolution;

using PagedList;
using System.Data.Entity.Infrastructure;
using System.Globalization;


using SpreadsheetLight;
using System.IO;
using Comisiones.Models.ViewModels;


namespace Comisiones.Controllers
{
    public class ReporteComisionesController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();
        private string nameController = "ReembolsosCanjeController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";


        // GET: ReporteComisiones
        public ActionResult Index(int? Anio, int? id_periodo, int? page)
        {
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 23 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            var IdPeriodo = 0;
            int pageSize = 0;
            int pageNumber = 0;
            int ReOrdena = 0;

            if (Anio != null)
            {
                var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                               where o.Anio == Anio
                               select new SelectListItem
                               {
                                   Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                   Value = o.Id_Periodo.ToString(),
                               };
                ViewBag.dropdownPeriodos = Periodos.ToList();


                int AnioActual = DateTime.Now.Year;
                int AnioInicial = AnioActual - 20;
                List<SelectListItem> Anios = new List<SelectListItem>();

                for (int i = AnioActual; i >= AnioInicial; i--)
                {
                    Anios.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                ViewBag.dropdownAnio = Anios;

                if (id_periodo != null)
                {
                    //    Log oLog = new Log(rutaArchivoLog);
                    //    List<CO_st_ReporteComisionesExcell2_Result> LProductos = new List<CO_st_ReporteComisionesExcell2_Result>();

                    //    using (herbaxEntities hbxEntities = new herbaxEntities())
                    //     {
                    //        LProductos = hbxEntities.CO_st_ReporteComisionesExcell2(id_periodo).ToList();

                    //        pageSize = 10;
                    //        pageNumber = (page ?? 1);
                    //        return View(LProductos.ToPagedList(pageNumber, pageSize));
                    //    }

                    DateTime currentDate = DateTime.Now;
                    String fechaActual = currentDate.ToString("yy/MM/dd");
                    String fechaArchivo = currentDate.ToString("yyyyMMdd_hhmmss");


                    Log oLog = new Log(rutaArchivoLog);
                    string filePath = @"C:\herbax\archivos\";

                    //Creando documento excel
                    SLDocument oSLDocument = new SLDocument();
                    //Definiendo encabezado (fila,columna, nombrecolumna)

                    oSLDocument.SetCellValue(1, 1, "partner_id");
                    oSLDocument.SetCellValue(1, 2, "Nombre");
                    oSLDocument.SetCellValue(1, 3, "Pais");
                    oSLDocument.SetCellValue(1, 4, "FormaPago");
                    oSLDocument.SetCellValue(1, 5, "C_Constructor");
                    oSLDocument.SetCellValue(1, 6, "C_Generacional");
                    oSLDocument.SetCellValue(1, 7, "C_InfinitoDiferencial");
                    oSLDocument.SetCellValue(1, 8, "C_InicioRapido");
                    oSLDocument.SetCellValue(1, 9, "C_Nivel");
                    oSLDocument.SetCellValue(1, 10, "C_Retail");
                    oSLDocument.SetCellValue(1, 11, "C_Elite");
                    oSLDocument.SetCellValue(1, 12, "C_Salto");
                    oSLDocument.SetCellValue(1, 13, "C_TotalGeneral");
                    oSLDocument.SetCellValue(1, 14, "P_ManejoCDA");
                    oSLDocument.SetCellValue(1, 15, "P_BonoRuta");
                    oSLDocument.SetCellValue(1, 16, "P_ReembolsoMeta");
                    oSLDocument.SetCellValue(1, 17, "P_TotalPercepciones");
                    oSLDocument.SetCellValue(1, 18, "D_ArrendamientoAuto");
                    oSLDocument.SetCellValue(1, 19, "D_ConvencionRiviera");
                    oSLDocument.SetCellValue(1, 20, "D_CreditoCrece");
                    oSLDocument.SetCellValue(1, 21, "D_DeducibleAuto");
                    oSLDocument.SetCellValue(1, 22, "D_EquipoComputo");
                    oSLDocument.SetCellValue(1, 23, "D_Impresora");
                    oSLDocument.SetCellValue(1, 24, "D_Mobiliario");
                    oSLDocument.SetCellValue(1, 25, "D_Placas_Seguros");
                    oSLDocument.SetCellValue(1, 26, "D_Prestamo");
                    oSLDocument.SetCellValue(1, 27, "D_Manutencion");
                    oSLDocument.SetCellValue(1, 28, "D_TotalDeducciones");
                    oSLDocument.SetCellValue(1, 29, "ComisionTotal");
                    oSLDocument.SetCellValue(1, 30, "I_Iva");
                    oSLDocument.SetCellValue(1, 31, "I_ISR");
                    oSLDocument.SetCellValue(1, 32, "Total");

                    try
                    {
                        using (herbaxEntities hbxEntities = new herbaxEntities())
                        {
                            //List<CO_st_NombreArchivoLReembolsos_Result> objNombre = hbxEntities.CO_st_NombreArchivoLReembolsos().ToList();
                            var tipoConsulta = "C99";
                            var compania = 1;
                            var tipoArchivo = "Comisiones";

                            var objNombre = db.CO_st_NombreArchivoLComisiones3(tipoConsulta, compania, tipoArchivo).ToList();
                            String nombreArchivo = objNombre[0].ArchivoXLS;
                            //String nombreArchivo = objNombre[0].nombreArchivo;

                            //String nombreArchivo = "LISTA_COMISIONES_YYYYMMDD_HHMMSS.XLSX";
                            nombreArchivo = nombreArchivo.Replace("YYYYMMDD_HHMMSS", fechaArchivo);

                            if (Directory.Exists(filePath))
                            {
                                filePath = filePath + nombreArchivo;
                            }
                            else
                            {
                                DirectoryInfo di = Directory.CreateDirectory(filePath);
                                filePath = filePath + nombreArchivo;
                            }

                            List<CO_st_ReporteComisionesExcell2_Result> LComisiones = new List<CO_st_ReporteComisionesExcell2_Result>();
                            LComisiones = hbxEntities.CO_st_ReporteComisionesExcell2(id_periodo).ToList();

                            if (LComisiones != null)
                            {
                                Int32 Nfila = 2;
                                System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                                System.Globalization.DateTimeFormatInfo format = c.DateTimeFormat;

                                LComisiones.ForEach(i =>
                                {
                                    //Campos que vienen en SP
                                    oSLDocument.SetCellValue(Nfila, 1, i.partner_id);
                                    oSLDocument.SetCellValue(Nfila, 2, i.Nombre);
                                    oSLDocument.SetCellValue(Nfila, 3, i.Pais);
                                    oSLDocument.SetCellValue(Nfila, 4, i.FormaPago);
                                    oSLDocument.SetCellValue(Nfila, 5, Convert.ToDecimal(i.C_Constructor));
                                    oSLDocument.SetCellValue(Nfila, 6, Convert.ToDecimal(i.C_Generacional));
                                    oSLDocument.SetCellValue(Nfila, 7, Convert.ToDecimal(i.C_InfinitoDiferencial));
                                    oSLDocument.SetCellValue(Nfila, 8, Convert.ToDecimal(i.C_InicioRapido));
                                    oSLDocument.SetCellValue(Nfila, 9, Convert.ToDecimal(i.C_Nivel));
                                    oSLDocument.SetCellValue(Nfila, 10, Convert.ToDecimal(i.C_Retail));
                                    oSLDocument.SetCellValue(Nfila, 11, Convert.ToDecimal(i.C_Elite));
                                    oSLDocument.SetCellValue(Nfila, 12, Convert.ToDecimal(i.C_Salto));
                                    oSLDocument.SetCellValue(Nfila, 13, Convert.ToDecimal(i.C_TotalGeneral));
                                    oSLDocument.SetCellValue(Nfila, 14, Convert.ToDecimal(i.P_ManejoCDA));
                                    oSLDocument.SetCellValue(Nfila, 15, Convert.ToDecimal(i.P_BonoRuta));
                                    oSLDocument.SetCellValue(Nfila, 16, Convert.ToDecimal(i.P_ReembolsoMeta));
                                    oSLDocument.SetCellValue(Nfila, 17, Convert.ToDecimal(i.P_TotalPercepciones));
                                    oSLDocument.SetCellValue(Nfila, 18, Convert.ToDecimal(i.D_ArrendamientoAuto));
                                    oSLDocument.SetCellValue(Nfila, 19, Convert.ToDecimal(i.D_ConvencionRiviera));
                                    oSLDocument.SetCellValue(Nfila, 20, Convert.ToDecimal(i.D_CreditoCrece));
                                    oSLDocument.SetCellValue(Nfila, 21, Convert.ToDecimal(i.D_DeducibleAuto));
                                    oSLDocument.SetCellValue(Nfila, 22, Convert.ToDecimal(i.D_EquipoComputo));
                                    oSLDocument.SetCellValue(Nfila, 23, Convert.ToDecimal(i.D_Impresora));
                                    oSLDocument.SetCellValue(Nfila, 24, Convert.ToDecimal(i.D_Mobiliario));
                                    oSLDocument.SetCellValue(Nfila, 25, Convert.ToDecimal(i.D_Placas_Seguros));
                                    oSLDocument.SetCellValue(Nfila, 26, Convert.ToDecimal(i.D_Prestamo));
                                    oSLDocument.SetCellValue(Nfila, 27, Convert.ToDecimal(i.D_Manutencion));
                                    oSLDocument.SetCellValue(Nfila, 28, Convert.ToDecimal(i.D_TotalDeducciones));
                                    oSLDocument.SetCellValue(Nfila, 29, Convert.ToDecimal(i.ComisionTotal));
                                    oSLDocument.SetCellValue(Nfila, 30, Convert.ToDecimal(i.I_Iva));
                                    oSLDocument.SetCellValue(Nfila, 31, Convert.ToDecimal(i.I_ISR));
                                    oSLDocument.SetCellValue(Nfila, 32, Convert.ToDecimal(i.Total));

                                    Nfila++;
                                });


                                string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF" };
                                for (int _index = 0; _index < _lstColumns.Length; _index++)
                                {
                                    SLStyle style1 = oSLDocument.CreateStyle();
                                    style1.Font.FontName = "Arial";
                                    style1.Font.FontSize = 11;
                                    style1.Font.FontColor = System.Drawing.Color.White;
                                    style1.SetGradientFill(SLGradientShadingStyleValues.Horizontal2, System.Drawing.Color.LightSeaGreen, System.Drawing.Color.MediumAquamarine);
                                    style1.Font.Bold = true;
                                    oSLDocument.SetCellStyle(_lstColumns[_index] + "1", style1);
                                    style1 = null;

                                    for (int _indexDet = 2; _indexDet < LComisiones.Count + 2; _indexDet++)
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

                                LComisiones = null;

                                oSLDocument.SaveAs(filePath);
                            }

                            ViewBag.IsOK = 0;
                            return File(filePath, "application/force-download", Path.GetFileName(filePath));
                            pageSize = 10;
                            pageNumber = (page ?? 1);
                            return View(LComisiones.ToPagedList(pageNumber, pageSize));


                        }


                    }
                    catch (Exception ex)
                    {
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
                        oSLDocument.SetCellValue(1, 11, "Error al procesar");
                        oSLDocument.SetCellValue(1, 12, "Error al procesar");
                        oSLDocument.SetCellValue(1, 13, "Error al procesar");
                        oSLDocument.SetCellValue(1, 14, "Error al procesar");
                        oSLDocument.SetCellValue(1, 15, "Error al procesar");
                        oSLDocument.SetCellValue(1, 16, "Error al procesar");
                        oSLDocument.SetCellValue(1, 17, "Error al procesar");
                        oSLDocument.SetCellValue(1, 18, "Error al procesar");
                        oSLDocument.SetCellValue(1, 19, "Error al procesar");
                        oSLDocument.SetCellValue(1, 20, "Error al procesar");
                        oSLDocument.SetCellValue(1, 21, "Error al procesar");
                        oSLDocument.SetCellValue(1, 22, "Error al procesar");
                        oSLDocument.SetCellValue(1, 23, "Error al procesar");
                        oSLDocument.SetCellValue(1, 24, "Error al procesar");
                        oSLDocument.SetCellValue(1, 25, "Error al procesar");
                        oSLDocument.SetCellValue(1, 26, "Error al procesar");
                        oSLDocument.SetCellValue(1, 27, "Error al procesar");
                        oSLDocument.SetCellValue(1, 28, "Error al procesar");
                        oSLDocument.SetCellValue(1, 29, "Error al procesar");
                        oSLDocument.SetCellValue(1, 30, "Error al procesar");
                        oSLDocument.SetCellValue(1, 31, "Error al procesar");
                        oSLDocument.SetCellValue(1, 32, "Error al procesar");
                        oSLDocument.SaveAs(filePath);

                        ViewBag.IsOK = 0;
                        return File(filePath, "application/force-download", Path.GetFileName(filePath));
                    }

                    
                }
                else
                {
                    Log oLog = new Log(rutaArchivoLog);
                    List<CO_st_ReporteComisionesExcell2_Result> LProductos = new List<CO_st_ReporteComisionesExcell2_Result>();

                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {

                        LProductos = hbxEntities.CO_st_ReporteComisionesExcell2(IdPeriodo).ToList();

                        pageSize = 10;
                        pageNumber = (page ?? 1);
                        return View(LProductos.ToPagedList(pageNumber, pageSize));
                    }
                }
                return View();
            }
            else 
            {
                var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                               where o.Estatus == 1
                               select new SelectListItem
                               {
                                   Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                   Value = o.Id_Periodo.ToString(),
                               };
                ViewBag.dropdownPeriodos = Periodos.ToList();


                int AnioActual = DateTime.Now.Year;
                int AnioInicial = AnioActual - 20;
                List<SelectListItem> Anios = new List<SelectListItem>();

                for (int i = AnioActual; i >= AnioInicial; i--)
                {
                    Anios.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                ViewBag.dropdownAnio = Anios;

                //if (id_periodo == null)
                //{
                //    IdPeriodo = 0;
                //}


                Log oLog = new Log(rutaArchivoLog);
                List<CO_st_ReporteComisionesExcell2_Result> LProductos = new List<CO_st_ReporteComisionesExcell2_Result>();

                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    LProductos = hbxEntities.CO_st_ReporteComisionesExcell2(IdPeriodo).ToList();

                    pageSize = 10;
                    pageNumber = (page ?? 1);
                    return View(LProductos.ToPagedList(pageNumber, pageSize));
                }
            }

        }


        // POST: AsignarReembolsos/Create
        [HttpPost]
        public ActionResult Index(int? TxtPeriodo)
        {

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_ReporteComisionesExcell2_Result> LProductos = new List<CO_st_ReporteComisionesExcell2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
               LProductos = hbxEntities.CO_st_ReporteComisionesExcell2(TxtPeriodo).ToList();

                return View(LProductos.ToList());
            }

        }



        public ActionResult descargaReporteComisiones(int? TxtPeriodo)
        {
            DateTime currentDate = DateTime.Now;
            String fechaActual = currentDate.ToString("yy/MM/dd");
            String fechaArchivo = currentDate.ToString("yyyyMMdd_hhmmss");


            Log oLog = new Log(rutaArchivoLog);
            string filePath = @"C:\herbax\archivos\";

            //Creando documento excel
            SLDocument oSLDocument = new SLDocument();
            //Definiendo encabezado (fila,columna, nombrecolumna)

            oSLDocument.SetCellValue(1, 1, "partner_id");
            oSLDocument.SetCellValue(1, 2, "Nombre");
            oSLDocument.SetCellValue(1, 3, "Pais");
            oSLDocument.SetCellValue(1, 4, "FormaPago");
            oSLDocument.SetCellValue(1, 5, "C_Constructor");
            oSLDocument.SetCellValue(1, 6, "C_Generacional");
            oSLDocument.SetCellValue(1, 7, "C_InfinitoDiferencial");
            oSLDocument.SetCellValue(1, 8, "C_InicioRapido");
            oSLDocument.SetCellValue(1, 9, "C_Nivel");
            oSLDocument.SetCellValue(1, 10, "C_Retail");
            oSLDocument.SetCellValue(1, 11, "C_Elite");
            oSLDocument.SetCellValue(1, 12, "C_Salto");
            oSLDocument.SetCellValue(1, 13, "C_TotalGeneral");
            oSLDocument.SetCellValue(1, 14, "P_ManejoCDA");
            oSLDocument.SetCellValue(1, 15, "P_BonoRuta");
            oSLDocument.SetCellValue(1, 16, "P_ReembolsoMeta");
            oSLDocument.SetCellValue(1, 17, "P_TotalPercepciones");
            oSLDocument.SetCellValue(1, 18, "D_ArrendamientoAuto");
            oSLDocument.SetCellValue(1, 19, "D_ConvencionRiviera");
            oSLDocument.SetCellValue(1, 20, "D_CreditoCrece");
            oSLDocument.SetCellValue(1, 21, "D_DeducibleAuto");
            oSLDocument.SetCellValue(1, 22, "D_EquipoComputo");
            oSLDocument.SetCellValue(1, 23, "D_Impresora");
            oSLDocument.SetCellValue(1, 24, "D_Mobiliario");
            oSLDocument.SetCellValue(1, 25, "D_Placas_Seguros");
            oSLDocument.SetCellValue(1, 26, "D_Prestamo");
            oSLDocument.SetCellValue(1, 27, "D_Manutencion");
            oSLDocument.SetCellValue(1, 28, "D_TotalDeducciones");
            oSLDocument.SetCellValue(1, 29, "ComisionTotal");
            oSLDocument.SetCellValue(1, 30, "I_Iva");
            oSLDocument.SetCellValue(1, 31, "I_ISR");
            oSLDocument.SetCellValue(1, 32, "Total");



            try
            {
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    //List<CO_st_NombreArchivoLReembolsos_Result> objNombre = hbxEntities.CO_st_NombreArchivoLReembolsos().ToList();
                    //String nombreArchivo = objNombre[0].nombreArchivo;
                    String nombreArchivo = "LISTA_COMISIONES_YYYYMMDD_HHMMSS.XLSX";
                    nombreArchivo = nombreArchivo.Replace("YYYYMMDD_HHMMSS", fechaArchivo);

                    if (Directory.Exists(filePath))
                    {
                        filePath = filePath + nombreArchivo;
                    }
                    else
                    {
                        DirectoryInfo di = Directory.CreateDirectory(filePath);
                        filePath = filePath + nombreArchivo;
                    }

                    //revisando: si existe el archivo en el Directorio lo borra.
                    //if (System.IO.File.Exists(filePath))
                    //{
                    //    System.IO.File.Delete(filePath);
                    //}


                    List<CO_st_ReporteComisionesExcell2_Result> LComisiones = new List<CO_st_ReporteComisionesExcell2_Result>();                    
                    LComisiones = hbxEntities.CO_st_ReporteComisionesExcell2(TxtPeriodo).ToList();


                    if (LComisiones != null)
                    {
                        Int32 Nfila = 2;
                        System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                        System.Globalization.DateTimeFormatInfo format = c.DateTimeFormat;

                        LComisiones.ForEach(i =>
                        {
                            //Campos que vienen en SP
                            oSLDocument.SetCellValue(Nfila, 1, i.partner_id);
                            oSLDocument.SetCellValue(Nfila, 2, i.Nombre);
                            oSLDocument.SetCellValue(Nfila, 3, i.Pais);
                            oSLDocument.SetCellValue(Nfila, 4, i.FormaPago);
                            oSLDocument.SetCellValue(Nfila, 5, Convert.ToDecimal(i.C_Constructor));
                            oSLDocument.SetCellValue(Nfila, 6, Convert.ToDecimal(i.C_Generacional));
                            oSLDocument.SetCellValue(Nfila, 7, Convert.ToDecimal(i.C_InfinitoDiferencial));
                            oSLDocument.SetCellValue(Nfila, 8, Convert.ToDecimal(i.C_InicioRapido));
                            oSLDocument.SetCellValue(Nfila, 9, Convert.ToDecimal(i.C_Nivel));
                            oSLDocument.SetCellValue(Nfila, 10, Convert.ToDecimal(i.C_Retail));
                            oSLDocument.SetCellValue(Nfila, 11, Convert.ToDecimal(i.C_Elite));
                            oSLDocument.SetCellValue(Nfila, 12, Convert.ToDecimal(i.C_Salto));
                            oSLDocument.SetCellValue(Nfila, 13, Convert.ToDecimal(i.C_TotalGeneral));
                            oSLDocument.SetCellValue(Nfila, 14, Convert.ToDecimal(i.P_ManejoCDA));
                            oSLDocument.SetCellValue(Nfila, 15, Convert.ToDecimal(i.P_BonoRuta));
                            oSLDocument.SetCellValue(Nfila, 16, Convert.ToDecimal(i.P_ReembolsoMeta));
                            oSLDocument.SetCellValue(Nfila, 17, Convert.ToDecimal(i.P_TotalPercepciones));
                            oSLDocument.SetCellValue(Nfila, 18, Convert.ToDecimal(i.D_ArrendamientoAuto));
                            oSLDocument.SetCellValue(Nfila, 19, Convert.ToDecimal(i.D_ConvencionRiviera));
                            oSLDocument.SetCellValue(Nfila, 20, Convert.ToDecimal(i.D_CreditoCrece));
                            oSLDocument.SetCellValue(Nfila, 21, Convert.ToDecimal(i.D_DeducibleAuto));
                            oSLDocument.SetCellValue(Nfila, 22, Convert.ToDecimal(i.D_EquipoComputo));
                            oSLDocument.SetCellValue(Nfila, 23, Convert.ToDecimal(i.D_Impresora));
                            oSLDocument.SetCellValue(Nfila, 24, Convert.ToDecimal(i.D_Mobiliario));
                            oSLDocument.SetCellValue(Nfila, 25, Convert.ToDecimal(i.D_Placas_Seguros));
                            oSLDocument.SetCellValue(Nfila, 26, Convert.ToDecimal(i.D_Prestamo));
                            oSLDocument.SetCellValue(Nfila, 27, Convert.ToDecimal(i.D_Manutencion));
                            oSLDocument.SetCellValue(Nfila, 28, Convert.ToDecimal(i.D_TotalDeducciones));
                            oSLDocument.SetCellValue(Nfila, 29, Convert.ToDecimal(i.ComisionTotal));
                            oSLDocument.SetCellValue(Nfila, 30, Convert.ToDecimal(i.I_Iva));
                            oSLDocument.SetCellValue(Nfila, 31, Convert.ToDecimal(i.I_ISR));
                            oSLDocument.SetCellValue(Nfila, 32, Convert.ToDecimal(i.Total));

                            Nfila++;
                        });


                        string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF" };                        
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
                            for (int _indexDet = 2; _indexDet < LComisiones.Count + 2; _indexDet++)
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

                        LComisiones = null;

                        oSLDocument.SaveAs(filePath);
                    }
                }

            }
            catch (Exception ex)
            {
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
                oSLDocument.SetCellValue(1, 11, "Error al procesar");
                oSLDocument.SetCellValue(1, 12, "Error al procesar");
                oSLDocument.SetCellValue(1, 13, "Error al procesar");
                oSLDocument.SetCellValue(1, 14, "Error al procesar");
                oSLDocument.SetCellValue(1, 15, "Error al procesar");
                oSLDocument.SetCellValue(1, 16, "Error al procesar");
                oSLDocument.SetCellValue(1, 17, "Error al procesar");
                oSLDocument.SetCellValue(1, 18, "Error al procesar");
                oSLDocument.SetCellValue(1, 19, "Error al procesar");
                oSLDocument.SetCellValue(1, 20, "Error al procesar");
                oSLDocument.SetCellValue(1, 21, "Error al procesar");
                oSLDocument.SetCellValue(1, 22, "Error al procesar");
                oSLDocument.SetCellValue(1, 23, "Error al procesar");
                oSLDocument.SetCellValue(1, 24, "Error al procesar");
                oSLDocument.SetCellValue(1, 25, "Error al procesar");
                oSLDocument.SetCellValue(1, 26, "Error al procesar");
                oSLDocument.SetCellValue(1, 27, "Error al procesar");
                oSLDocument.SetCellValue(1, 28, "Error al procesar");
                oSLDocument.SetCellValue(1, 29, "Error al procesar");
                oSLDocument.SetCellValue(1, 30, "Error al procesar");
                oSLDocument.SetCellValue(1, 31, "Error al procesar");
                oSLDocument.SetCellValue(1, 32, "Error al procesar");
                oSLDocument.SaveAs(filePath);

            }

            ViewBag.IsOK = 0;

            //return View();
            return File(filePath, "application/force-download", Path.GetFileName(filePath));

        }

    }
}
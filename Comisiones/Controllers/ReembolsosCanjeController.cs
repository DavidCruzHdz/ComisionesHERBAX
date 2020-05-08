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

namespace Comisiones.Controllers
{
    public class ReembolsosCanjeController : Controller
    {
        private string nameController = "ReembolsosCanjeController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();

        // GET: ReembolsosCanje
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 19 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }


            ViewBag.Message = "";


            //var query = from o in db.CO_tb_Productos_Reembolso.ToList()
            //            from c in Evo.IN_tb_Products.ToList()
            //            where o.sku == c.sku && o.Id_Empresa == c.Id_Empresa 
            //            select new SelectListItem
            //            {
            //                Text = c.CPNombreUsuario,
            //                Value = c.CPIdUsuario.ToString()
            //            };

            // ViewBag.dropdownUsuario = query.OrderBy(u => u.Text).ToList();

            // join a in db.CO_Tb_Parametro on 23 equals a.id_Parametro 
            //&& @FechaMaxima <= DATEADD(month, MesesDeVigencia.valor, Periodo.Fin
            var FechaMaxima = DateTime.Now;

            //var result = from r in db.CO_tb_Reembolsos_Hist
            //             join s in Evo.CL_tb_Customer on r.ID_PARTNER equals s.partner_id
            //             join n in Evo.CL_tb_Pais on s.country_id equals n.NombreCorto
            //             join p in Evo.CO_tb_Periodo on r.ID_PERIODO equals p.Id_Periodo
            //             join a in db.CO_tb_Parametro on 23 equals a.id_Parametro
            //             where r.ESTATUS == 14 //&& FechaMaxima <= DATEADD(month, MesesDeVigencia.valor, Periodo.Fin)
            //             select new Cliente_Reemolsos()
            //             {
            //                 Socio = s.partner_id.ToString(),
            //                 Nombre = s.last_name + " " + s.first_name,
            //                 Telefono = s.Phone_number,                            
            //                 Pais = n.Pais,
            //                 Ventas = r.VENTA_PERIODO.ToString(),
            //                 Reembolso = r.CANTIDAD_REEMBOLSO.ToString(),
            //                 Periodo = r.ID_PERIODO.ToString()
            //             };

            //ViewBag.tblReembolsos = result.ToList();

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaReembolsos2_Result> LReembolsos = new List<CO_st_BuscaReembolsos2_Result>();
            List<CO_st_BuscaProductosReembolsos1_Result> LProductos = new List<CO_st_BuscaProductosReembolsos1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //LProductos = hbxEntities.CO_st_BuscaProductosReembolsos1().ToList();
                    LReembolsos = hbxEntities.CO_st_BuscaReembolsos2().ToList();

                    ViewBag.Reembolsos = LReembolsos;

                    LProductos = hbxEntities.CO_st_BuscaProductosReembolsos1().ToList();
                    return View(LProductos.ToList());

                }
                catch (Exception ex)
                {
                    ViewBag.IsOK = 0;
                    var Col = 0;
                    var resultado = "";

                    resultado = ex.InnerException.Message.ToString();
                    Col = resultado.IndexOf("\r");

                    if (Col.ToString() == "")
                    {
                        var NewString = resultado.Substring(0, Col);
                        ViewBag.Message = NewString;
                    }
                    else
                    {
                        ViewBag.Message = resultado;
                    }


                    return View();

                }

            }
            //return View(db.CO_tb_Productos_Reembolso.ToList());
        }


        [HttpPost]
        public ActionResult Index(IList<CO_st_BuscaProductosReembolsos1_Result> entity, IList<CO_st_BuscaProductosReembolsos1_Result> entity2)
        {
            //if (entity != null)
            //{ 
            //herbaxEntities db = new herbaxEntities();
            var varFolio = "";
            var NumPartida = 0;

            try
            {
                var Folios = db.CO_tb_FoliosMovInventarios.Where(s => (s.Estatus == true)).FirstOrDefault();

                if (Folios.Folio_Actual != "0")
                {
                    varFolio = (Decimal.Parse(Folios.Folio_Actual.ToString()) + 1).ToString();
                }
                else
                {
                    varFolio = (Folios.Folio_Inicio);
                }

                var varSocio = "";
                var varEmpresa = "";
                var varPeriodo = 0;
                Double varDiferencia = 0;
                Double varSubTotal = 0;
                Double varMontoPda = 0;
                Double varCantidad = 0;
                Double varPrecio = 0;


                foreach (var item2 in entity2)
                {


                    if (item2.CantidadProducto > 0)
                    {
                        NumPartida = NumPartida + 1;
                        varCantidad = Double.Parse(item2.CantidadProducto.ToString());
                        varPrecio = Double.Parse(item2.PrecioPublico.ToString());
                        varMontoPda = varCantidad * varPrecio;
                        varSubTotal = varSubTotal + varMontoPda;

                        if (NumPartida == 1)
                        {
                            varSocio = item2.Id_Socio;
                            varEmpresa = item2.Id_Empresa;
                            varPeriodo = int.Parse(item2.Id_Periodo.ToString());
                        }
                    }

                }


                var varImpuesto = 0;
                var varTotal = 0;
                var NomUsuario = Session["IdNombre"].ToString();

                var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                var varUsuario = Usuarios.IdUsuario;
                
                var varReembolsos = db.CO_tb_Reembolsos_Hist.Where(x => x.ID_PARTNER == varSocio && x.ID_PERIODO == varPeriodo).FirstOrDefault();

                var varVentas = Double.Parse(varReembolsos.VENTA_PERIODO.ToString());
                var varReembolso = Double.Parse(varReembolsos.CANTIDAD_REEMBOLSO.ToString());

                varDiferencia = varReembolso - varSubTotal;

                var PeriodosEvo = Evo.CO_tb_Periodo.Where(x => x.Id_Periodo == varPeriodo).FirstOrDefault();
                var AnioEvo = PeriodosEvo.Anio;
                var MesEvo = PeriodosEvo.Mes;
                var QuincenaEvo = PeriodosEvo.Quincena;
                var FolioInv = varFolio.ToString();

                var PeriodosEvoPT = EvoPT.CU_tb_Periodos.Where(x => x.Anio == AnioEvo && x.Mes == MesEvo && x.Quincena == QuincenaEvo).FirstOrDefault();
                var PeriodoPT = PeriodosEvoPT.Id_Periodo;

                var GeneraPedido = db.CO_st_GenerarPedidosReembolsos5(FolioInv, varSocio, 70, varEmpresa, varDiferencia, varSubTotal, varImpuesto, varTotal, NumPartida, varVentas, varUsuario, AnioEvo, MesEvo, QuincenaEvo, varPeriodo);
                var valorPedido = int.Parse(GeneraPedido.ToString());

                var TraerPedido = db.CO_tb_Pedidos.Where(x => x.Id_MovInv == varFolio).FirstOrDefault();
                var varPedido = 0;

                if (TraerPedido != null)
                {
                    varPedido = TraerPedido.Id_Pedido;
                }
                else
                {
                    varPedido = 1;
                }


                NumPartida = 0;
                var listPartidas = new List<CO_tb_DtlPedidos>();
                foreach (var item in entity) /// aqui es
                {
                    if (item.CantidadProducto > 0)
                    {
                        CO_tb_DtlPedidos Partidas = new CO_tb_DtlPedidos();

                        NumPartida = NumPartida + 1;


                        Partidas.Id_Pedido = varPedido;
                        Partidas.Id_MovInv = varFolio;
                        Partidas.Id_DtlMovInv = NumPartida; // este es el problema
                        Partidas.Id_Producto = item.sku;
                        Partidas.Cantidad = item.CantidadProducto;
                        Partidas.Precio = Decimal.Parse(item.PrecioPublico.ToString());
                        Partidas.Impuesto = 0;
                        Partidas.Porc_Impuesto = 0;
                        Partidas.Id_Talla = 0;
                        Partidas.Desc_Importe = 0;
                        Partidas.Desc_Impuesto = 0;
                        Partidas.Id_Empresa = item.Id_Empresa;
                        listPartidas.Add(Partidas);
                    }
                }
                db.CO_tb_DtlPedidos.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                db.SaveChanges();
                //var TranspasaPedidoEvoPT = db.CO_st_TranspasaPedidosReembolsos2(varSocio, PeriodoPT, AnioEvo, MesEvo, QuincenaEvo, varFolio, NomUsuario, varEmpresa);

                ViewBag.IsOK = 1;
                ViewBag.Message = "Se genero el pedido y se transfieron los datos de inventario";
                return Redirect("~/ReembolsosCanje/Index");
            }
            catch (Exception ex)
            {

                ViewBag.IsOK = 0;
                var Col = 0;
                var resultado = "";

                resultado = ex.InnerException.Message.ToString();
                Col = resultado.IndexOf("\r");

                if (Col.ToString() == "")
                {
                    var NewString = resultado.Substring(0, Col);
                    ViewBag.Message = NewString;
                }
                else
                {
                    ViewBag.Message = resultado;
                }


                return View();


            }



        }



        public JsonResult BuscaReembolsos()
        {
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaReembolsos2_Result> LReembolsos = new List<CO_st_BuscaReembolsos2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Obtiene las comisiones atrasadas
                    LReembolsos = hbxEntities.CO_st_BuscaReembolsos2().ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaReembolsos  DETALLE: " + ex);
                }
            }
            return Json(LReembolsos, JsonRequestBehavior.AllowGet);
        }



        public ActionResult descargaEXCELL(string pais)
        {
            Log oLog = new Log(rutaArchivoLog);
            //string filePath = @"C:\herbax\archivos\";
            //string nombreArchivo = "ListadoDeReembolsos.xlsx";
            DateTime currentDate = DateTime.Now;

            String fechaActual = currentDate.ToString("yy/MM/dd");
            String fechaArchivo = currentDate.ToString("yyyyMMdd_hhmmss");

            //Campos que vienen en SP
            //periodoCreacion,partner_id,nombre,importe,saldo,Motivo,idFormaPago,descFormaPago

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "ListadoDeReembolsos.xlsx";
            //Creando documento excel
            SLDocument oSLDocument = new SLDocument();
            System.Data.DataTable dt = new System.Data.DataTable();

            //Definiendo encabezado (fila,columna, nombrecolumna)            

            try
            {
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    ////nombreArchivo = nombreArchivo.Replace("YYYYMMDD_HHMMSS", fechaArchivo);

                    ////filePath = filePath + nombreArchivo;

                    List<CO_st_ListaReembolsos1_Result> LReembolsos = new List<CO_st_ListaReembolsos1_Result>();
                    LReembolsos = hbxEntities.CO_st_ListaReembolsos1().ToList();

                    if (LReembolsos != null)
                    {

                        Int32 Nfila = 2;
                        // column
                        dt.Columns.Add("Socio", typeof(string));
                        dt.Columns.Add("Nombre", typeof(string));
                        //dt.Columns.Add("Calle", typeof(string));
                        //dt.Columns.Add("Colonia", typeof(string));
                        //dt.Columns.Add("CP", typeof(Int32));
                        dt.Columns.Add("Telefono", typeof(string));
                        dt.Columns.Add("Pais", typeof(string));
                        dt.Columns.Add("Ventas", typeof(decimal));
                        dt.Columns.Add("Reembolso", typeof(decimal));
                        dt.Columns.Add("Periodo", typeof(string));

                        // reglones
                        LReembolsos.ForEach(i =>
                        {
                            //Campos que vienen en SP
                            //periodoCreacion,partner_id,nombre,importe,saldo,Motivo,idFormaPago,descFormaPago
                            //dt.Rows.Add(i.Socio, i.Nombre, i.Calle, i.Colonia, i.CP, i.Telefono, i.Pais, Convert.ToDecimal(i.Ventas), Convert.ToDecimal(i.Reembolso), i.Situacion);
                            dt.Rows.Add(i.Socio, i.Nombre, i.Telefono, i.Pais, Convert.ToDecimal(i.Ventas), Convert.ToDecimal(i.Reembolso), i.Periodo);
                            Nfila++;
                        });

                        oSLDocument.ImportDataTable(1, 1, dt, true);
                        //oSLDocument.SaveAs(filePath);

                    }
                }

                var memorystream = new MemoryStream();
                oSLDocument.SaveAs(memorystream);
                memorystream.Position = 0;


                return new FileStreamResult(memorystream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = "PERS936AB.xlsx" };

                //return File(filePath, "application/force-download", Path.GetFileName(filePath));

            }
            catch (Exception ex)
            {
                ViewBag.IsOK = 0;
                ViewBag.Message = ex.InnerException.Message.ToString();
                return View();
            }
        }





        public ActionResult descargaReembolsosCanje(string pais)
        {
            Log oLog = new Log(rutaArchivoLog);
            string filePath = @"C:\herbax\archivos\";
            //string nombreArchivo = "ListadoDeReembolsos.xlsx";



            //string pathFile = AppDomain.CurrentDomain.BaseDirectory + "ListadoDeReembolsos.xlsx";

            DateTime currentDate = DateTime.Now;

            String fechaActual = currentDate.ToString("yy/MM/dd");
            String fechaArchivo = currentDate.ToString("yyyyMMdd_hhmmss");

            //Campos que vienen en SP
            //periodoCreacion,partner_id,nombre,importe,saldo,Motivo,idFormaPago,descFormaPago

            //Creando documento excel
            SLDocument oSLDocument = new SLDocument();
            //Definiendo encabezado (fila,columna, nombrecolumna)
            oSLDocument.SetCellValue(1, 1, "Socio");
            oSLDocument.SetCellValue(1, 2, "Nombre");
            //oSLDocument.SetCellValue(1, 3, "Calle");
            //oSLDocument.SetCellValue(1, 4, "Colonia");
            //oSLDocument.SetCellValue(1, 5, "CP");
            oSLDocument.SetCellValue(1, 3, "Telefono");
            oSLDocument.SetCellValue(1, 4, "Pais");
            oSLDocument.SetCellValue(1, 5, "Ventas");
            oSLDocument.SetCellValue(1, 6, "Reembolso");
            oSLDocument.SetCellValue(1, 7, "Periodo");


            try
            {
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    List<CO_st_NombreArchivoLReembolsos_Result> objNombre = hbxEntities.CO_st_NombreArchivoLReembolsos().ToList();
                    String nombreArchivo = objNombre[0].nombreArchivo;
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

                    List<CO_st_ListaReembolsos1_Result> LReembolsos = new List<CO_st_ListaReembolsos1_Result>();
                    LReembolsos = hbxEntities.CO_st_ListaReembolsos1().ToList();

                    if (LReembolsos != null)
                    {
                        Int32 Nfila = 2;
                        System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("es-MX");
                        System.Globalization.DateTimeFormatInfo format = c.DateTimeFormat;

                        LReembolsos.ForEach(i =>
                        {
                            //Campos que vienen en SP
                            //periodoCreacion,partner_id,nombre,importe,saldo,Motivo,idFormaPago,descFormaPago
                            oSLDocument.SetCellValue(Nfila, 1, i.Socio);
                            oSLDocument.SetCellValue(Nfila, 2, i.Nombre);
                            //oSLDocument.SetCellValue(Nfila, 3, i.Calle);
                            //oSLDocument.SetCellValue(Nfila, 4, i.Colonia);
                            //oSLDocument.SetCellValue(Nfila, 5, i.CP);
                            oSLDocument.SetCellValue(Nfila, 3, i.Telefono);
                            oSLDocument.SetCellValue(Nfila, 4, i.Pais);
                            oSLDocument.SetCellValue(Nfila, 5, Convert.ToDecimal(i.Ventas));
                            oSLDocument.SetCellValue(Nfila, 6, Convert.ToDecimal(i.Reembolso));
                            oSLDocument.SetCellValue(Nfila, 7, i.Periodo);
                            Nfila++;
                        });


                        //string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                        string[] _lstColumns = new String[] { "A", "B", "C", "D", "E", "F", "G" };
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
                            for (int _indexDet = 2; _indexDet < LReembolsos.Count + 2; _indexDet++)
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

                        LReembolsos = null;

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
                //oSLDocument.SetCellValue(1, 8, "Error al procesar");
                //oSLDocument.SetCellValue(1, 9, "Error al procesar");
                //oSLDocument.SetCellValue(1, 10, "Error al procesar");


                oSLDocument.SaveAs(filePath);

            }

            ViewBag.IsOK = 0;

            //return View();
            return File(filePath, "application/force-download", Path.GetFileName(filePath));

        }
    }
}

namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using Comisiones.Models.ModelsEvolution;
    using Comisiones.Models.ViewModels;
    using Data;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    //using SelectPdf;
    using System.Net.Mail;
    using System.Security.Permissions;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using Image = iTextSharp.text.Image;

    using System.Configuration;
    using System.Web.Configuration;
    using System.Net.Configuration;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;

    public class EstadosDeCuentaController : Controller
    {
        private string nameController = "EstadosDeCuentaController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();


        // GET: EstadosDeCuenta
        public ActionResult Index()
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 22 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            ViewBag.IsOk = 0;
            ViewBag.Message = "";
            ViewBag.IsPedido = 0;

            List<SelectListItem> Meses = new List<SelectListItem>()
                    {
                         new SelectListItem { Text = "Seleccione un Mes", Value = "0" },
                        new SelectListItem { Text = "Enero", Value = "1" },
                        new SelectListItem { Text = "Febrero", Value = "2" },
                        new SelectListItem { Text = "Marzo", Value = "3" },
                        new SelectListItem { Text = "Abril", Value = "4" },
                        new SelectListItem { Text = "Mayo", Value = "5" },
                        new SelectListItem { Text = "Junio", Value = "6" },
                        new SelectListItem { Text = "Julio", Value = "7" },
                        new SelectListItem { Text = "Agosto", Value = "8" },
                        new SelectListItem { Text = "Septiembre", Value = "9" },
                        new SelectListItem { Text = "Octubre", Value = "10" },
                        new SelectListItem { Text = "Noviembre", Value = "11" },
                        new SelectListItem { Text = "Diciembre", Value = "12" },
                    };
            //Meses.Add(new SelectListItem() { Text = "asdfsd Cape", Value = "NC" });

            ViewBag.dropdownMes = Meses;


            List<SelectListItem> Anios = new List<SelectListItem>();
            var contar = 1;
            var AnioActual = DateTime.Now.Year;
            var AnioAntes = AnioActual - 10;

            Anios.Add(new SelectListItem { Text = "Seleccione un Año", Value = "0" });
            //foreach (int AnioActual in 10000 step -1)
            for (int f = AnioAntes; f <= AnioActual; f++)
            {
                Anios.Add(new SelectListItem() { Text = f.ToString(), Value = f.ToString() });
            }

            ViewBag.dropdownAnio = Anios;
            return View();
        }

        public JsonResult GeneraPDF(string partner_id, int? Anio, int? Mes)
        {
            var MesLetra = "";

            switch (Mes)
            {
                case 1:
                    MesLetra = "Enero";
                    break;
                case 2:
                    MesLetra = "Febrero";
                    break;
                case 3:
                    MesLetra = "Marzo";
                    break;
                case 4:
                    MesLetra = "Abril";
                    break;
                case 5:
                    MesLetra = "Mayo";
                    break;
                case 6:
                    MesLetra = "Junio";
                    break;
                case 7:
                    MesLetra = "Julio";
                    break;
                case 8:
                    MesLetra = "Agosto";
                    break;
                case 9:
                    MesLetra = "Septiembre";
                    break;
                case 10:
                    MesLetra = "Octubre";
                    break;
                case 11:
                    MesLetra = "Noviembre";
                    break;
                case 12:
                    MesLetra = "Diciembre";
                    break;
                default:
                    MesLetra = "";
                    break;
            }


            try
            {

                var Socios = Evo.CL_tb_Customer.Where(x => (x.partner_id == partner_id)).FirstOrDefault();
               var NomSocio = Socios.last_name + " " + Socios.first_name;
                var DirSocio = Socios.Address_1 + " " + Socios.Address_3;
                var EmailSocio = Socios.email;
                var CiudadSocio = "";

                if (Socios.Xen_ShippingCity != null)
                {
                    CiudadSocio = Socios.Xen_ShippingCity.ToString();
                }
                else
                {
                    CiudadSocio = "";
                }

                var EstadoSocio = "";

                if (Socios.Xen_ShippingState != null)
                {
                    EstadoSocio = Socios.Xen_ShippingState.ToString();
                }
                else
                {
                    EstadoSocio = "";
                }

                var EdoyMcpioSocio = CiudadSocio + " " + EstadoSocio;
                var IdPais = Socios.country_id;
                var CP = Socios.PostalCode;
                var RFC = Socios.tax_id;
                var CDA = Socios.Address_2;
                var IdRank = Socios.RankLastPeriod_ID;
                var Paises = Evo.CL_tb_Pais.Where(x => (x.NombreCorto == IdPais)).FirstOrDefault();
                var NomPais = Paises.Pais;
                var PagActual = 1;
                var PagTotales = 1;
                var Rank = Evo.CO_tb_Rank.Where(x => (x.Rank_ID == IdRank)).FirstOrDefault();
                var Grado = "";
                var Fecha = DateTime.Now;
                //var NumSocios = decimal.Parse(partner_id.ToString());

                var NumSocio = partner_id.PadLeft(10, '0');
                var Puntos = "";
                var CompraPer = "";

                var Periodo = 0;
                var mes = ""; ;
                var yyyy = "";
                var dd = "01";
                var FechaPeriodo = "";

                var Volumen4ta = "";
                var Volumen5ta = "";
                var Directos = "";
                var ValorPunto = "";

                

                //var Periodo = int.Parse(Evo.CO_tb_Periodo.Reverse().First().Id_Periodo.ToString());
                //var mes = int.Parse(Evo.CO_tb_Periodo.Reverse().First().Mes.ToString());
                //var yyyy = int.Parse(Evo.CO_tb_Periodo.Reverse().First().Anio.ToString());

                var BuscaPeriodo = Evo.CO_tb_Periodo.Where(x => (x.Anio == Anio && x.Mes == Mes));
                if (BuscaPeriodo != null)
                {
                    var countOfRows = BuscaPeriodo.Count();
                    var lastRow = BuscaPeriodo.OrderBy(c => 1 == 1).Skip(countOfRows - 1).FirstOrDefault();

                    Periodo = lastRow.Id_Periodo;
                    mes = lastRow.Mes.ToString();
                    yyyy = lastRow.Anio.ToString();

                    FechaPeriodo = dd + "/" + mes + "/" + yyyy;


                    ////Primero obtenemos el día actual
                    //DateTime date = DateTime.Now;

                    //    //Asi obtenemos el primer dia del mes actual
                    //    DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);

                    //    //Y de la siguiente forma obtenemos el ultimo dia del mes
                    //    //agregamos 1 mes al objeto anterior y restamos 1 día.
                    //    DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.Value.AddMonths(1).AddDays(-1);

                }


                var GradoRank = Evo.CL_tb_CustomerRank.Where(x => (x.Partner_ID == NumSocio) && x.Period_ID == Periodo).FirstOrDefault();
                if (GradoRank != null)
                {
                    var NombreRank = Evo.CO_tb_Rank.Where(x => x.Rank_ID == GradoRank.PaidRankID).FirstOrDefault();
                    Grado = NombreRank.Rank;
                }

                var Arbol = db.CO_tb_ARBOL_HIST.Where(x => (x.PARTNER_ID == NumSocio)).FirstOrDefault();
                if (Arbol != null)
                {
                    Puntos = Arbol.PUNTOSPERSONAL.ToString();
                    CompraPer = Arbol.VENTAPERSONAL.ToString();
                }

                var Volumen = db.CO_tb_ARBOL_HIST.Where(x => (x.PARTNER_ID == NumSocio)).FirstOrDefault();
                if (Arbol != null)
                {
                    Puntos = Arbol.PUNTOSPERSONAL.ToString();
                    CompraPer = Arbol.VENTAPERSONAL.ToString();
                }


                var Socio = int.Parse(NumSocio.ToString());
                var FeIniPeriodo = Convert.ToDateTime(FechaPeriodo);
                var FeFinPeriodo = FeIniPeriodo.AddMonths(1).AddDays(-1);


                var ventageneracional = db.co_tb_ventageneracional.Where(x => (x.partner_id == Socio && x.Inicio == FeIniPeriodo && x.Fin == FeFinPeriodo)).FirstOrDefault();
                if (ventageneracional != null)
                {
                    Volumen4ta = ventageneracional.Volumen4taGen.ToString();
                    Volumen5ta = ventageneracional.Volumen5aGen.ToString();
                    //Directos = ventageneracional.directos.ToString();
                    
                }

                var ObtenPunto = db.CO_tb_Parametro.Where(x => (x.Key_parametro == "KEY_VALOR_DEL_PUNTO")).FirstOrDefault();
                if (ObtenPunto != null)
                {
                    ValorPunto = ObtenPunto.valor_dec.ToString();
                }
                
                var strFiles = "EdoDeCta_Socio_" + partner_id + ".pdf";
                string FullPathOut = (@"C:\Inetpub\wwwroot\EvoComisiones\EdoDeCtaPDF\" + strFiles);

                byte[] pdfBytes;
                var mem = new MemoryStream();
                {
                    Document pdfDoc = new Document(PageSize.LETTER, 15.0F, 15.0F, 0.0F, 0.0F);

                    // *******************************************************
                    // Configuración del documento PDF
                    // *******************************************************
                    var FilePath = @"C:\Inetpub\wwwroot\EvoComisiones\EdoDeCtaPDF\";
                    FileIOPermission filePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, FilePath);
                    filePermission.AllFiles = FileIOPermissionAccess.AllAccess;
                    filePermission.AllLocalFiles = FileIOPermissionAccess.AllAccess;

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, mem);
                    {

                        //PdfWriter.GetInstance(pdfDoc, new FileStream(FullPathOut, FileMode.Create));

                        // *******************************************************
                        // Fonts y colores
                        // ******************************************************* 
                        var FontColour = new BaseColor(35, 31, 32);
                        Font font_celdas = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);
                        Font font_direccion = FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.NORMAL);
                        Font font_totales = FontFactory.GetFont(FontFactory.HELVETICA, 13, Font.BOLD);
                        Font font_detalle = FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.BOLD);
                        Font font_Cliente = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);
                        Font font_Puntos = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.BOLD, FontColour);
                        Font font_Movimientos = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);
                        Font font_Deducciones = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);
                        Font font_detalle_Grado = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.BOLD);
                        Font font_titulo_Grado = FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD);
                        Font font_titulo_tabla = FontFactory.GetFont(FontFactory.HELVETICA, 18, Font.BOLD);
                        Font font_titulo_detalle = FontFactory.GetFont(FontFactory.HELVETICA, 11, Font.BOLD);

                        pdfDoc.Open();
                        pdfDoc.NewPage();
                        PdfPTable table = new PdfPTable(8);
                        PdfPCell cell = new PdfPCell();

                        //Image image = Image.GetInstance(Server.MapPath("~/myPic.jpg"));
                        //cell.AddElement(image);
                        //table.AddCell(cell);
                        //Paragraph para1 = new Paragraph("texto uno");
                        //cell = new PdfPCell();
                        //cell.AddElement(para1);
                        //table.AddCell(cell);

                        Chunk PARRAFO1 = new Chunk(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL));
                        pdfDoc.Add(new Paragraph(PARRAFO1));

                        Chunk PARRAFO2 = new Chunk(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL));
                        pdfDoc.Add(new Paragraph(PARRAFO2));
                        //var Directos = Arbol.directos; no lo tenemos todavia

                        var FileImgPath = @"C:\Inetpub\wwwroot\EvoComisiones\images\";

                        Image jpg = iTextSharp.text.Image.GetInstance(FileImgPath + "Herbax.png");
                        jpg.ScalePercent(100, 100);
                        jpg.SetDpi(1000, 1000);
                        jpg.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                        pdfDoc.Add(jpg);

                        //PdfPTable table2 = new PdfPTable(new Single[] { 17, 3, 5, 15, 10, 5, 5, 5, 20, 15, 15, 15 });
                        PdfPTable table2 = new PdfPTable(new Single[] { 11, 11, 11, 11, 11, 11, 5, 14, 14, 14, 14, 14 });
                        table2.WidthPercentage = 100;
                        table2.DefaultCell.BorderWidth = 1;
                        //table2.DefaultCell.BorderColor = new Color(99, 99, 99);
                        table2.DefaultCell.Border = 1;

                        //pdfDoc.Add(new Paragraph("GIF"));
                        //System.Drawing.Image gif = System.Drawing.Image.GetInstance(FilePath + "/Herbax.png");
                        //pdfDoc.Add(gif);

                        PdfPCell cell115 = new PdfPCell(new Phrase("Estado de cuenta", font_titulo_tabla));
                        cell115.BorderWidth = 0;
                        cell115.Colspan = 6;
                        //cell118.Rowspan = 2;
                        table2.AddCell(cell115);
                            
                        PdfPCell cell127 = new PdfPCell(new Phrase("Pag. " + PagActual + " de " + PagTotales, font_celdas));
                        cell127.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell127.BorderWidth = 0;
                        cell127.Colspan = 6;
                        table2.AddCell(cell127);

                        PdfPCell cell215 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell215.BorderWidth = 0;
                        cell215.Colspan = 6;
                        table2.AddCell(cell215);

                        PdfPCell cell227 = new PdfPCell(new Phrase("Fecha de Impresion: " + Fecha, font_direccion));
                        cell227.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell227.BorderWidth = 0;
                        cell227.Colspan = 6;
                        table2.AddCell(cell227);


                        PdfPCell cell315 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell315.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell315.BorderWidth = 0;
                        cell315.Colspan = 6;
                        table2.AddCell(cell315);

                        PdfPCell cell327 = new PdfPCell(new Phrase("Periodo: " + MesLetra + " " + Anio, font_direccion));
                        cell327.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell327.BorderWidth = 0;
                        cell327.Colspan = 6;
                        table2.AddCell(cell327);


                        PdfPCell cell415 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell415.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell415.BorderWidth = 0;
                        cell415.Colspan = 6;
                        table2.AddCell(cell415);

                        PdfPCell cell427 = new PdfPCell(new Phrase(NomSocio, font_detalle));
                        cell427.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell427.BorderWidth = 0;
                        cell427.Colspan = 6;
                        table2.AddCell(cell427);

                        PdfPCell cell515 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell515.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell515.BorderWidth = 0;
                        cell515.Colspan = 6;
                        table2.AddCell(cell515);

                        PdfPCell cell527 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell527.BorderWidth = 0;
                        cell527.Colspan = 6;
                        table2.AddCell(cell527);

                        PdfPCell cell615 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell615.BorderWidth = 0;
                        cell615.Colspan = 6;
                        table2.AddCell(cell615);

                        PdfPCell cell627 = new PdfPCell(new Phrase("GRADO", font_titulo_Grado));
                        cell627.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell627.BorderWidth = 0;
                        cell627.Colspan = 6;
                        table2.AddCell(cell627);

                        PdfPCell cell718 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell718.BorderWidth = 0;
                        cell718.Colspan = 6;
                        table2.AddCell(cell718);

                        PdfPCell cell724 = new PdfPCell(new Phrase(Grado, font_titulo_Grado));
                        cell724.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell724.BorderWidth = 0;
                        cell724.Colspan = 6;
                        table2.AddCell(cell724);

                        PdfPCell cell818 = new PdfPCell(new Phrase(" ", font_direccion));
                        cell818.BorderWidth = 0;
                        cell818.Colspan = 6;
                        table2.AddCell(cell818);

                        PdfPCell cell824 = new PdfPCell(new Phrase("", font_direccion));
                        cell824.BorderWidth = 0;
                        cell824.Colspan = 6;
                        table2.AddCell(cell824);

                    PdfPCell cell918 = new PdfPCell(new Phrase("Información del Distribuidor", font_titulo_detalle));
                    cell918.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell918.BorderWidth = 0;
                    cell918.Colspan = 5;
                    table2.AddCell(cell918);


                    PdfPCell cell920 = new PdfPCell(new Phrase("", font_direccion));
                    cell920.BorderWidth = 0;
                    cell920.Colspan = 1;
                    table2.AddCell(cell920);

                    PdfPCell cell922 = new PdfPCell(new Phrase("Estado de Cuenta", font_titulo_detalle));
                    cell922.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell922.BorderWidth = 0;
                    cell922.Colspan = 6;
                    table2.AddCell(cell922);

                    List<CO_st_EdoDeCuenta_get_Movimientos1_Result> EdoCtaMovimientos = new List<CO_st_EdoDeCuenta_get_Movimientos1_Result>();
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        EdoCtaMovimientos = hbxEntities.CO_st_EdoDeCuenta_get_Movimientos1(partner_id, Mes, Anio).ToList();
                    }

                    PdfPCell cellCte111 = new PdfPCell(new Phrase(partner_id, font_Cliente));
                    cellCte111.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellCte111.BorderWidth = 0;
                    cellCte111.Colspan = 1;
                    table2.AddCell(cellCte111);

                    PdfPCell cellCte122 = new PdfPCell(new Phrase(NomSocio, font_Cliente));
                    cellCte122.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellCte122.BorderWidth = 0;
                    cellCte122.Colspan = 4;
                    table2.AddCell(cellCte122);

                    PdfPCell cellCte112 = new PdfPCell(new Phrase("", font_direccion));
                    cellCte112.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellCte112.BorderWidth = 0;
                    cellCte112.Colspan = 1;
                    table2.AddCell(cellCte112);

                    PdfPCell cellPerc114 = new PdfPCell(new Phrase("Concepto", font_detalle));
                    cellPerc114.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellPerc114.BorderWidth = 0;
                    cellPerc114.Colspan = 3;
                    table2.AddCell(cellPerc114);

                    PdfPCell cellPerc121 = new PdfPCell(new Phrase("1er. Qna.", font_detalle));
                    cellPerc121.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellPerc121.BorderWidth = 0;
                    cellPerc121.Colspan = 1;
                    table2.AddCell(cellPerc121);

                    PdfPCell cellPerc131 = new PdfPCell(new Phrase("2da. Qna.", font_detalle));
                    cellPerc131.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellPerc131.BorderWidth = 0;
                    cellPerc131.Colspan = 1;
                    table2.AddCell(cellPerc131);

                    PdfPCell cellPerc141 = new PdfPCell(new Phrase("Total", font_detalle));
                    cellPerc141.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellPerc141.BorderWidth = 0;
                    cellPerc141.Colspan = 1;
                    table2.AddCell(cellPerc141);

                    var Registros = EdoCtaMovimientos.Count();
                    var Reg = 0;
                    if (Registros <= 7)
                    {
                        // renumeramos el Reg porque las columnas del lado derecho son 15 y las percepciones y deducciones son menos reglones
                        Reg = 15;
                    }
                    else
                    {
                        Reg = Registros+2;
                    }


                    for (int i = 0; i <= Reg; i++)
                    {
                        var Totales = "";
                        var ConceptoId = "";
                        var PrimerQuincena = "";
                        var SegundaQuincena = "";
                        var Total = "";
                        var Concepto = "";
                        var Descripcion = "";
                        var idconcepto = "";


                        if (i < Registros)
                        {
                            decimal P = decimal.Parse(EdoCtaMovimientos[i].Quincena1.ToString());
                            PrimerQuincena = P.ToString("C", CultureInfo.CurrentCulture);
                            decimal S = decimal.Parse(EdoCtaMovimientos[i].Quincena2.ToString());
                            SegundaQuincena = S.ToString("C", CultureInfo.CurrentCulture);
                            decimal T = decimal.Parse(EdoCtaMovimientos[i].Total.ToString());
                            Total = T.ToString("C", CultureInfo.CurrentCulture);
                            Concepto = EdoCtaMovimientos[i].Concepto;
                            Descripcion = EdoCtaMovimientos[i].descripcion;
                            idconcepto = EdoCtaMovimientos[i].id_concepto.ToString();
                        }

                        if (idconcepto == "0")
                        {
                            ConceptoId = "";
                        }
                        else
                        {
                            ConceptoId = idconcepto.ToString();
                        }

                        if (Concepto.ToString() == "Percepciones" || Concepto.ToString() == "Deducciones" || Concepto.ToString() == " " || Concepto == "Fecha de Pago:")
                        {
                            PrimerQuincena = "";
                        }
                        else
                        {
                            PrimerQuincena = PrimerQuincena.ToString();
                        }

                        if (Concepto.ToString() == "Percepciones" || Concepto.ToString() == "Deducciones" || Concepto.ToString() == " " || Concepto == "Fecha de Pago:")
                        {
                            SegundaQuincena = "";
                        }
                        else
                        {
                            SegundaQuincena = SegundaQuincena.ToString();
                        }


                        if (Concepto.ToString() == "Percepciones" || Concepto.ToString() == "Deducciones" || Concepto.ToString() == " " || Concepto == "Fecha de Pago:")
                        {
                            Totales = "";
                        }
                        else
                        {
                            Totales = Total.ToString();
                        }

                        // DATOS DE DIRECCION Y TITULOS DE PERCEPCIONES
                        if (i == 0)
                        {
                            PdfPCell cellCte222 = new PdfPCell(new Phrase(DirSocio, font_Cliente));
                            cellCte222.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte222.BorderWidth = 0;
                            cellCte222.Colspan = 5;
                            table2.AddCell(cellCte222);

                            PdfPCell cellCte223 = new PdfPCell(new Phrase("", font_Cliente));
                            cellCte223.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte223.BorderWidth = 0;
                            cellCte223.Colspan = 1;
                            table2.AddCell(cellCte223);


                            PdfPCell cellPerc211 = new PdfPCell(new Phrase(Concepto, font_detalle));
                            cellPerc211.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellPerc211.BorderWidth = 0;
                            cellPerc211.Colspan = 6;
                            table2.AddCell(cellPerc211);
                        }


                        // MUNICIPIO , ESTADO Y PRIMER REGLON DE PERCEPCIONES
                        if (i == 1)
                        {
                            PdfPCell cellCte311 = new PdfPCell(new Phrase(EdoyMcpioSocio, font_Cliente));
                            cellCte311.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte311.BorderWidth = 0;
                            cellCte311.Colspan = 5;
                            table2.AddCell(cellCte311);

                            PdfPCell cellCte312 = new PdfPCell(new Phrase("", font_Cliente));
                            cellCte312.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte312.BorderWidth = 0;
                            cellCte312.Colspan = 1;
                            table2.AddCell(cellCte312);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones")
                            {
                                PdfPCell cellPerc311 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc311.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc311.BorderWidth = 0;
                                cellPerc311.Colspan = 3;
                                table2.AddCell(cellPerc311);
                            }
                            else
                            {
                                PdfPCell cellPerc321 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc321.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc321.BorderWidth = 0;
                                cellPerc321.Colspan = 1;
                                table2.AddCell(cellPerc321);

                                PdfPCell cellPerc331 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc331.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc331.BorderWidth = 0;
                                cellPerc331.Colspan = 2;
                                table2.AddCell(cellPerc331);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc341 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc341.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc341.BorderWidth = 0;
                                cellPerc341.Colspan = 1;
                                table2.AddCell(cellPerc341);

                                PdfPCell cellPerc351 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc351.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc351.BorderWidth = 0;
                                cellPerc351.Colspan = 1;
                                table2.AddCell(cellPerc351);

                                PdfPCell cellPerc361 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc361.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc361.BorderWidth = 0;
                                cellPerc361.Colspan = 1;
                                table2.AddCell(cellPerc361);
                            }
                            else
                            {
                                PdfPCell cellPerc341 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc341.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc341.BorderWidth = 0;
                                cellPerc341.Colspan = 1;
                                table2.AddCell(cellPerc341);

                                PdfPCell cellPerc351 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc351.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc351.BorderWidth = 0;
                                cellPerc351.Colspan = 1;
                                table2.AddCell(cellPerc351);

                                PdfPCell cellPerc361 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc361.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc361.BorderWidth = 0;
                                cellPerc361.Colspan = 1;
                                table2.AddCell(cellPerc361);
                            }
                        }



                        // CODIGO POSTAL Y SEGUNDO REGLON DE PERCEPCIONES
                        if (i == 2)
                        {
                            PdfPCell cellCte411 = new PdfPCell(new Phrase("C.P. ", font_Cliente));
                            cellCte411.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte411.BorderWidth = 0;
                            cellCte411.Colspan = 1;
                            table2.AddCell(cellCte411);

                            PdfPCell cellCte422 = new PdfPCell(new Phrase(CP, font_Cliente));
                            cellCte422.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte422.BorderWidth = 0;
                            cellCte422.Colspan = 4;
                            table2.AddCell(cellCte422);

                            PdfPCell cellCte423 = new PdfPCell(new Phrase("", font_Cliente));
                            cellCte423.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte423.BorderWidth = 0;
                            cellCte423.Colspan = 1;
                            table2.AddCell(cellCte423);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc411 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc411.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc411.BorderWidth = 0;
                                cellPerc411.Colspan = 3;
                                table2.AddCell(cellPerc411);
                            }
                            else
                            {
                                PdfPCell cellPerc421 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc421.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc421.BorderWidth = 0;
                                cellPerc421.Colspan = 1;
                                table2.AddCell(cellPerc421);

                                PdfPCell cellPerc431 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc431.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc431.BorderWidth = 0;
                                cellPerc431.Colspan = 2;
                                table2.AddCell(cellPerc431);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc441 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc441.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc441.BorderWidth = 0;
                                cellPerc441.Colspan = 1;
                                table2.AddCell(cellPerc441);

                                PdfPCell cellPerc451 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc451.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc451.BorderWidth = 0;
                                cellPerc451.Colspan = 1;
                                table2.AddCell(cellPerc451);

                                PdfPCell cellPerc461 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc461.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc461.BorderWidth = 0;
                                cellPerc461.Colspan = 1;
                                table2.AddCell(cellPerc461);
                            }
                            else
                            {
                                PdfPCell cellPerc441 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc441.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc441.BorderWidth = 0;
                                cellPerc441.Colspan = 1;
                                table2.AddCell(cellPerc441);

                                PdfPCell cellPerc451 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc451.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc451.BorderWidth = 0;
                                cellPerc451.Colspan = 1;
                                table2.AddCell(cellPerc451);

                                PdfPCell cellPerc461 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc461.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc461.BorderWidth = 0;
                                cellPerc461.Colspan = 1;
                                table2.AddCell(cellPerc461);
                            }
                        }



                        // RFC Y TERCER REGLON DE PERCEPCIONES
                        if (i == 3)
                        {
                            PdfPCell cellCte511 = new PdfPCell(new Phrase("RFC: ", font_Cliente));
                            cellCte511.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte511.BorderWidth = 0;
                            cellCte511.Colspan = 1;
                            table2.AddCell(cellCte511);

                            PdfPCell cellCte522 = new PdfPCell(new Phrase(RFC, font_Cliente));
                            cellCte522.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte522.BorderWidth = 0;
                            cellCte522.Colspan = 4;
                            table2.AddCell(cellCte522);

                            PdfPCell cellCte523 = new PdfPCell(new Phrase("", font_Cliente));
                            cellCte523.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte523.BorderWidth = 0;
                            cellCte523.Colspan = 1;
                            table2.AddCell(cellCte523);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc511 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc511.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc511.BorderWidth = 0;
                                cellPerc511.Colspan = 3;
                                table2.AddCell(cellPerc511);

                            }
                            else
                            {
                                PdfPCell cellPerc521 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc521.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc521.BorderWidth = 0;
                                cellPerc521.Colspan = 1;
                                table2.AddCell(cellPerc521);

                                PdfPCell cellPerc531 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc531.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc531.BorderWidth = 0;
                                cellPerc531.Colspan = 2;
                                table2.AddCell(cellPerc531);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc541 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc541.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc541.BorderWidth = 0;
                                cellPerc541.Colspan = 1;
                                table2.AddCell(cellPerc541);

                                PdfPCell cellPerc551 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc551.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc551.BorderWidth = 0;
                                cellPerc551.Colspan = 1;
                                table2.AddCell(cellPerc551);

                                PdfPCell cellPerc561 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc561.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc561.BorderWidth = 0;
                                cellPerc561.Colspan = 1;
                                table2.AddCell(cellPerc561);
                            }
                            else
                            {
                                PdfPCell cellPerc541 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc541.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc541.BorderWidth = 0;
                                cellPerc541.Colspan = 1;
                                table2.AddCell(cellPerc541);

                                PdfPCell cellPerc551 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc551.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc551.BorderWidth = 0;
                                cellPerc551.Colspan = 1;
                                table2.AddCell(cellPerc551);

                                PdfPCell cellPerc561 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc561.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc561.BorderWidth = 0;
                                cellPerc561.Colspan = 1;
                                table2.AddCell(cellPerc561);
                            }
                        }


                        // CDA Y CUARTO REGLON DE PERCEPCIONES
                        if (i == 4)
                        {
                            PdfPCell cellCte611 = new PdfPCell(new Phrase("CDA: ", font_Cliente));
                            cellCte611.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte611.BorderWidth = 0;
                            cellCte611.Colspan = 1;
                            table2.AddCell(cellCte611);

                            PdfPCell cellCte622 = new PdfPCell(new Phrase(CDA, font_Cliente));
                            cellCte622.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte622.BorderWidth = 0;
                            cellCte622.Colspan = 4;
                            table2.AddCell(cellCte622);

                            PdfPCell cellCte623 = new PdfPCell(new Phrase("", font_Cliente));
                            cellCte623.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte623.BorderWidth = 0;
                            cellCte623.Colspan = 1;
                            table2.AddCell(cellCte623);


                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc611 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc611.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc611.BorderWidth = 0;
                                cellPerc611.Colspan = 3;
                                table2.AddCell(cellPerc611);
                            }
                            else
                            {
                                PdfPCell cellPerc621 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc621.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc621.BorderWidth = 0;
                                cellPerc621.Colspan = 1;
                                table2.AddCell(cellPerc621);

                                PdfPCell cellPerc631 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc631.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc631.BorderWidth = 0;
                                cellPerc631.Colspan = 2;
                                table2.AddCell(cellPerc631);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc641 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc641.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc641.BorderWidth = 0;
                                cellPerc641.Colspan = 1;
                                table2.AddCell(cellPerc641);

                                PdfPCell cellPerc651 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc651.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc651.BorderWidth = 0;
                                cellPerc651.Colspan = 1;
                                table2.AddCell(cellPerc651);

                                PdfPCell cellPerc661 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc661.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc661.BorderWidth = 0;
                                cellPerc661.Colspan = 1;
                                table2.AddCell(cellPerc661);
                            }
                            else
                            {
                                PdfPCell cellPerc641 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc641.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc641.BorderWidth = 0;
                                cellPerc641.Colspan = 1;
                                table2.AddCell(cellPerc641);

                                PdfPCell cellPerc651 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc651.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc651.BorderWidth = 0;
                                cellPerc651.Colspan = 1;
                                table2.AddCell(cellPerc651);

                                PdfPCell cellPerc661 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc661.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc661.BorderWidth = 0;
                                cellPerc661.Colspan = 1;
                                table2.AddCell(cellPerc661);
                            }
                        }


                        // INFORMACION PARA DETERMINAR GRADO
                        if (i == 5)
                        {
                            PdfPCell cellBco1711 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellBco1711.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellBco1711.BorderWidth = 0;
                            cellBco1711.Colspan = 5;
                            table2.AddCell(cellBco1711);

                            PdfPCell cellBco1712 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellBco1712.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellBco1712.BorderWidth = 0;
                            cellBco1712.Colspan = 1;
                            table2.AddCell(cellBco1712);


                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc2771 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc2771.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc2771.BorderWidth = 0;
                                cellPerc2771.Colspan = 3;
                                table2.AddCell(cellPerc2771);
                            }
                            else
                            {
                                PdfPCell cellPerc2721 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc2721.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc2721.BorderWidth = 0;
                                cellPerc2721.Colspan = 1;
                                table2.AddCell(cellPerc2721);

                                PdfPCell cellPerc2731 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc2731.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc2731.BorderWidth = 0;
                                cellPerc2731.Colspan = 2;
                                table2.AddCell(cellPerc2731);
                            }


                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc3741 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc3741.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc3741.BorderWidth = 0;
                                cellPerc3741.Colspan = 1;
                                table2.AddCell(cellPerc3741);

                                PdfPCell cellPerc3751 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_detalle));
                                cellPerc3751.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc3751.BorderWidth = 0;
                                cellPerc3751.Colspan = 1;
                                table2.AddCell(cellPerc3751);

                                PdfPCell cellPerc3761 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc3761.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc3761.BorderWidth = 0;
                                cellPerc3761.Colspan = 1;
                                table2.AddCell(cellPerc3761);
                            }
                            else
                            {
                                PdfPCell cellPerc4741 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc4741.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc4741.BorderWidth = 0;
                                cellPerc4741.Colspan = 1;
                                table2.AddCell(cellPerc4741);

                                PdfPCell cellPerc4751 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_Movimientos));
                                cellPerc4751.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc4751.BorderWidth = 0;
                                cellPerc4751.Colspan = 1;
                                table2.AddCell(cellPerc4751);

                                PdfPCell cellPerc4761 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc4761.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc4761.BorderWidth = 0;
                                cellPerc4761.Colspan = 1;
                                table2.AddCell(cellPerc4761);
                            }
                        }

                        // INFORMACION PARA DETERMINAR GRADO
                        if (i == 6)
                        {
                            PdfPCell cellCte711 = new PdfPCell(new Phrase("Información para determinar el Grado ", font_titulo_detalle));
                            cellCte711.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte711.BorderWidth = 0;
                            cellCte711.Colspan = 5;
                            table2.AddCell(cellCte711);

                            PdfPCell cellCte712 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellCte712.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCte712.BorderWidth = 0;
                            cellCte712.Colspan = 1;
                            table2.AddCell(cellCte712);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc711 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc711.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc711.BorderWidth = 0;
                                cellPerc711.Colspan = 3;
                                table2.AddCell(cellPerc711);
                            }
                            else
                            {
                                PdfPCell cellPerc721 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc721.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc721.BorderWidth = 0;
                                cellPerc721.Colspan = 1;
                                table2.AddCell(cellPerc721);

                                PdfPCell cellPerc731 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc731.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc731.BorderWidth = 0;
                                cellPerc731.Colspan = 2;
                                table2.AddCell(cellPerc731);
                            }


                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc741 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc741.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc741.BorderWidth = 0;
                                cellPerc741.Colspan = 1;
                                table2.AddCell(cellPerc741);

                                PdfPCell cellPerc751 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_detalle));
                                cellPerc751.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc751.BorderWidth = 0;
                                cellPerc751.Colspan = 1;
                                table2.AddCell(cellPerc751);

                                PdfPCell cellPerc761 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc761.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc761.BorderWidth = 0;
                                cellPerc761.Colspan = 1;
                                table2.AddCell(cellPerc761);
                            }
                            else
                            {
                                PdfPCell cellPerc741 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc741.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc741.BorderWidth = 0;
                                cellPerc741.Colspan = 1;
                                table2.AddCell(cellPerc741);

                                PdfPCell cellPerc751 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_Movimientos));
                                cellPerc751.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc751.BorderWidth = 0;
                                cellPerc751.Colspan = 1;
                                table2.AddCell(cellPerc751);

                                PdfPCell cellPerc761 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc761.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc761.BorderWidth = 0;
                                cellPerc761.Colspan = 1;
                                table2.AddCell(cellPerc761);
                            }
                        }

                        // Plan de Mercadeo
                        if (i == 7)
                        {
                            //PdfPCell cellGrado811 = new PdfPCell(new Phrase("Plan de Mercadeo: ", font_detalle_Grado));
                            PdfPCell cellGrado811 = new PdfPCell(new Phrase("", font_detalle_Grado));
                            cellGrado811.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGrado811.BorderWidth = 0;
                            cellGrado811.Colspan = 2;
                            table2.AddCell(cellGrado811);

                            //PdfPCell cellGrado821 = new PdfPCell(new Phrase("PM2011", font_detalle_Grado));
                            PdfPCell cellGrado821 = new PdfPCell(new Phrase("", font_detalle_Grado));
                            cellGrado821.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado821.BorderWidth = 0;
                            cellGrado821.Colspan = 1;
                            table2.AddCell(cellGrado821);

                            PdfPCell cellGrado831 = new PdfPCell(new Phrase("Valor del Punto: ", font_detalle_Grado));
                            cellGrado831.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGrado831.BorderWidth = 0;
                            cellGrado831.Colspan = 1;
                            table2.AddCell(cellGrado831);

                            PdfPCell cellGrado841 = new PdfPCell(new Phrase(ValorPunto.ToString(), font_detalle));
                            cellGrado841.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado841.BorderWidth = 0;
                            cellGrado841.Colspan = 1;
                            table2.AddCell(cellGrado841);

                            PdfPCell cellGrado842 = new PdfPCell(new Phrase("", font_detalle));
                            cellGrado842.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado842.BorderWidth = 0;
                            cellGrado842.Colspan = 1;
                            table2.AddCell(cellGrado842);


                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc811 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc811.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc811.BorderWidth = 0;
                                cellPerc811.Colspan = 3;
                                table2.AddCell(cellPerc811);
                            }
                            else
                            {
                                PdfPCell cellPerc821 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc821.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc821.BorderWidth = 0;
                                cellPerc821.Colspan = 1;
                                table2.AddCell(cellPerc821);

                                PdfPCell cellPerc831 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc831.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc831.BorderWidth = 0;
                                cellPerc831.Colspan = 2;
                                table2.AddCell(cellPerc831);
                            }


                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc841 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc841.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc841.BorderWidth = 0;
                                cellPerc841.Colspan = 1;
                                table2.AddCell(cellPerc841);

                                PdfPCell cellPerc851 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc851.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc851.BorderWidth = 0;
                                cellPerc851.Colspan = 1;
                                table2.AddCell(cellPerc851);

                                PdfPCell cellPerc861 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc861.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc861.BorderWidth = 0;
                                cellPerc861.Colspan = 1;
                                table2.AddCell(cellPerc861);
                            }
                            else
                            {
                                PdfPCell cellPerc841 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc841.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc841.BorderWidth = 0;
                                cellPerc841.Colspan = 1;
                                table2.AddCell(cellPerc841);

                                PdfPCell cellPerc851 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc851.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc851.BorderWidth = 0;
                                cellPerc851.Colspan = 1;
                                table2.AddCell(cellPerc851);

                                PdfPCell cellPerc861 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc861.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc861.BorderWidth = 0;
                                cellPerc861.Colspan = 1;
                                table2.AddCell(cellPerc861);
                            }
                        }

                        // Puntos Personales
                        if (i == 8)
                        {
                            PdfPCell cellGrado911 = new PdfPCell(new Phrase("Puntos Personales: ", font_detalle_Grado));
                            cellGrado911.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGrado911.BorderWidth = 0;
                            cellGrado911.Colspan = 2;
                            table2.AddCell(cellGrado911);

                            PdfPCell cellGrado921 = new PdfPCell(new Phrase(Puntos.ToString(), font_detalle_Grado));
                            cellGrado921.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado921.BorderWidth = 0;
                            cellGrado921.Colspan = 1;
                            table2.AddCell(cellGrado921);

                            PdfPCell cellGrado931 = new PdfPCell(new Phrase("Compra Personal: ", font_detalle_Grado));
                            cellGrado931.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGrado931.BorderWidth = 0;
                            cellGrado931.Colspan = 1;
                            table2.AddCell(cellGrado931);

                            PdfPCell cellGrado941 = new PdfPCell(new Phrase(CompraPer, font_Puntos));
                            cellGrado941.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado941.BorderWidth = 0;
                            cellGrado941.Colspan = 1;
                            table2.AddCell(cellGrado941);

                            PdfPCell cellGrado942 = new PdfPCell(new Phrase("", font_Puntos));
                            cellGrado942.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado942.BorderWidth = 0;
                            cellGrado942.Colspan = 1;
                            table2.AddCell(cellGrado942);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc911 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc911.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc911.BorderWidth = 0;
                                cellPerc911.Colspan = 3;
                                table2.AddCell(cellPerc911);
                            }
                            else
                            {
                                PdfPCell cellPerc921 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc921.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc921.BorderWidth = 0;
                                cellPerc921.Colspan = 1;
                                table2.AddCell(cellPerc921);

                                PdfPCell cellPerc931 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc931.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc931.BorderWidth = 0;
                                cellPerc931.Colspan = 2;
                                table2.AddCell(cellPerc931);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc941 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc941.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc941.BorderWidth = 0;
                                cellPerc941.Colspan = 1;
                                table2.AddCell(cellPerc941);

                                PdfPCell cellPerc951 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc951.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc951.BorderWidth = 0;
                                cellPerc951.Colspan = 1;
                                table2.AddCell(cellPerc951);

                                PdfPCell cellPerc961 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc961.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc961.BorderWidth = 0;
                                cellPerc961.Colspan = 1;
                                table2.AddCell(cellPerc961);
                            }
                            else
                            {
                                PdfPCell cellPerc941 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc941.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc941.BorderWidth = 0;
                                cellPerc941.Colspan = 1;
                                table2.AddCell(cellPerc941);

                                PdfPCell cellPerc951 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc951.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc951.BorderWidth = 0;
                                cellPerc951.Colspan = 1;
                                table2.AddCell(cellPerc951);

                                PdfPCell cellPerc961 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc961.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc961.BorderWidth = 0;
                                cellPerc961.Colspan = 1;
                                table2.AddCell(cellPerc961);
                            }
                        }


                        // Directos Calificados
                        if (i == 9)
                        {
                            //PdfPCell cellGrado1011 = new PdfPCell(new Phrase("Directos Calificados: ", font_detalle_Grado));
                            PdfPCell cellGrado1011 = new PdfPCell(new Phrase("", font_detalle_Grado));
                            cellGrado1011.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGrado1011.BorderWidth = 0;
                            cellGrado1011.Colspan = 2;
                            table2.AddCell(cellGrado1011);


                            //PdfPCell cellGrado1021 = new PdfPCell(new Phrase(Directos, font_Puntos));
                            PdfPCell cellGrado1021 = new PdfPCell(new Phrase("", font_Puntos));
                            cellGrado1021.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado1021.BorderWidth = 0;
                            cellGrado1021.Colspan = 1;
                            table2.AddCell(cellGrado1021);

                            PdfPCell cellGrado1031 = new PdfPCell(new Phrase(" ", font_Puntos));
                            cellGrado1031.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGrado1031.BorderWidth = 0;
                            cellGrado1031.Colspan = 1;
                            table2.AddCell(cellGrado1031);

                            PdfPCell cellGrado1041 = new PdfPCell(new Phrase(" ", font_Puntos));
                            cellGrado1041.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado1041.BorderWidth = 0;
                            cellGrado1041.Colspan = 1;
                            table2.AddCell(cellGrado1041);

                            PdfPCell cellGrado1042 = new PdfPCell(new Phrase(" ", font_Puntos));
                            cellGrado1042.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellGrado1042.BorderWidth = 0;
                            cellGrado1042.Colspan = 1;
                            table2.AddCell(cellGrado1042);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1011 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc1011.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1011.BorderWidth = 0;
                                cellPerc1011.Colspan = 3;
                                table2.AddCell(cellPerc1011);
                            }
                            else
                            {
                                PdfPCell cellPerc1021 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc1021.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1021.BorderWidth = 0;
                                cellPerc1021.Colspan = 1;
                                table2.AddCell(cellPerc1021);

                                PdfPCell cellPerc1031 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc1031.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1031.BorderWidth = 0;
                                cellPerc1031.Colspan = 2;
                                table2.AddCell(cellPerc1031);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1041 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc1041.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1041.BorderWidth = 0;
                                cellPerc1041.Colspan = 1;
                                table2.AddCell(cellPerc1041);

                                PdfPCell cellPerc1051 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc1051.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1051.BorderWidth = 0;
                                cellPerc1051.Colspan = 1;
                                table2.AddCell(cellPerc1051);

                                PdfPCell cellPerc1061 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc1061.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1061.BorderWidth = 0;
                                cellPerc1061.Colspan = 1;
                                table2.AddCell(cellPerc1061);
                            }
                            else
                            {
                                PdfPCell cellPerc1041 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc1041.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1041.BorderWidth = 0;
                                cellPerc1041.Colspan = 1;
                                table2.AddCell(cellPerc1041);

                                PdfPCell cellPerc1051 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc1051.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1051.BorderWidth = 0;
                                cellPerc1051.Colspan = 1;
                                table2.AddCell(cellPerc1051);

                                PdfPCell cellPerc1061 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc1061.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1061.BorderWidth = 0;
                                cellPerc1061.Colspan = 1;
                                table2.AddCell(cellPerc1061);
                            }
                        }


                        // ANTES DEL VOLUMEN VA UN ESPACIO EN BLANCO
                        if (i == 10)
                        {
                            PdfPCell cellBco1111 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellBco1111.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellBco1111.BorderWidth = 0;
                            cellBco1111.Colspan = 5;
                            table2.AddCell(cellBco1111);

                            PdfPCell cellBco1112 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellBco1112.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellBco1112.BorderWidth = 0;
                            cellBco1112.Colspan = 1;
                            table2.AddCell(cellBco1112);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1111 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc1111.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1111.BorderWidth = 0;
                                cellPerc1111.Colspan = 3;
                                table2.AddCell(cellPerc1111);
                            }
                            else
                            {
                                PdfPCell cellPerc1121 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc1121.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1121.BorderWidth = 0;
                                cellPerc1121.Colspan = 1;
                                table2.AddCell(cellPerc1121);

                                PdfPCell cellPerc1131 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc1131.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1131.BorderWidth = 0;
                                cellPerc1131.Colspan = 2;
                                table2.AddCell(cellPerc1131);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1141 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc1141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1141.BorderWidth = 0;
                                cellPerc1141.Colspan = 1;
                                table2.AddCell(cellPerc1141);

                                PdfPCell cellPerc1151 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_detalle));
                                cellPerc1151.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1151.BorderWidth = 0;
                                cellPerc1151.Colspan = 1;
                                table2.AddCell(cellPerc1151);

                                PdfPCell cellPerc1161 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc1161.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1161.BorderWidth = 0;
                                cellPerc1161.Colspan = 1;
                                table2.AddCell(cellPerc1161);
                            }
                            else
                            {
                                PdfPCell cellPerc0141 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc0141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc0141.BorderWidth = 0;
                                cellPerc0141.Colspan = 1;
                                table2.AddCell(cellPerc0141);

                                PdfPCell cellPerc0151 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_Movimientos));
                                cellPerc0151.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc0151.BorderWidth = 0;
                                cellPerc0151.Colspan = 1;
                                table2.AddCell(cellPerc0151);

                                PdfPCell cellPerc0161 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc0161.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc0161.BorderWidth = 0;
                                cellPerc0161.Colspan = 1;
                                table2.AddCell(cellPerc0161);
                            }
                        }



                        // VOLUMEN DE COMPRA POR GENERACION
                        if (i == 11)
                        {
                            PdfPCell cellVol1111 = new PdfPCell(new Phrase("Volumen de compra por Generación", font_titulo_detalle));
                            cellVol1111.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellVol1111.BorderWidth = 0;
                            cellVol1111.Colspan = 6;
                            table2.AddCell(cellVol1111);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc0111 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc0111.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc0111.BorderWidth = 0;
                                cellPerc0111.Colspan = 3;
                                table2.AddCell(cellPerc0111);
                            }
                            else
                            {
                                PdfPCell cellPerc0121 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc0121.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc0121.BorderWidth = 0;
                                cellPerc0121.Colspan = 1;
                                table2.AddCell(cellPerc0121);

                                PdfPCell cellPerc0131 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc0131.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc0131.BorderWidth = 0;
                                cellPerc0131.Colspan = 2;
                                table2.AddCell(cellPerc0131);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc10141 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc10141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc10141.BorderWidth = 0;
                                cellPerc10141.Colspan = 1;
                                table2.AddCell(cellPerc10141);

                                PdfPCell cellPerc10151 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_detalle));
                                cellPerc10151.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc10151.BorderWidth = 0;
                                cellPerc10151.Colspan = 1;
                                table2.AddCell(cellPerc10151);

                                PdfPCell cellPerc10161 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc10161.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc10161.BorderWidth = 0;
                                cellPerc10161.Colspan = 1;
                                table2.AddCell(cellPerc10161);
                            }
                            else
                            {
                                PdfPCell cellPerc11141 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc11141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc11141.BorderWidth = 0;
                                cellPerc11141.Colspan = 1;
                                table2.AddCell(cellPerc11141);

                                PdfPCell cellPerc11151 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_Movimientos));
                                cellPerc11151.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc11151.BorderWidth = 0;
                                cellPerc11151.Colspan = 1;
                                table2.AddCell(cellPerc11151);

                                PdfPCell cellPerc11161 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc11161.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc11161.BorderWidth = 0;
                                cellPerc11161.Colspan = 1;
                                table2.AddCell(cellPerc11161);
                            }
                        }

                        // Compra Calificable
                        if (i == 12)
                        {
                            PdfPCell cellVol1211 = new PdfPCell(new Phrase("(Compra Calificable) ", font_titulo_detalle));
                            cellVol1211.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellVol1211.BorderWidth = 0;
                            cellVol1211.Colspan = 5;
                            table2.AddCell(cellVol1211);

                            PdfPCell cellVol1212 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellVol1212.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellVol1212.BorderWidth = 0;
                            cellVol1212.Colspan = 1;
                            table2.AddCell(cellVol1212);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1211 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc1211.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1211.BorderWidth = 0;
                                cellPerc1211.Colspan = 3;
                                table2.AddCell(cellPerc1211);
                            }
                            else
                            {
                                PdfPCell cellPerc10221 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc10221.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc10221.BorderWidth = 0;
                                cellPerc10221.Colspan = 1;
                                table2.AddCell(cellPerc10221);

                                PdfPCell cellPerc10231 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc10231.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc10231.BorderWidth = 0;
                                cellPerc10231.Colspan = 2;
                                table2.AddCell(cellPerc10231);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc10241 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc10241.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc10241.BorderWidth = 0;
                                cellPerc10241.Colspan = 1;
                                table2.AddCell(cellPerc10241);

                                PdfPCell cellPerc10251 = new PdfPCell(new Phrase(SegundaQuincena.ToString(), font_detalle));
                                cellPerc10251.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc10251.BorderWidth = 0;
                                cellPerc10251.Colspan = 1;
                                table2.AddCell(cellPerc10251);

                                PdfPCell cellPerc10261 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc10261.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc10261.BorderWidth = 0;
                                cellPerc10261.Colspan = 1;
                                table2.AddCell(cellPerc10261);
                            }
                            else
                            {
                                PdfPCell cellPerc1241 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc1241.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1241.BorderWidth = 0;
                                cellPerc1241.Colspan = 1;
                                table2.AddCell(cellPerc1241);

                                PdfPCell cellPerc1251 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc1251.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1251.BorderWidth = 0;
                                cellPerc1251.Colspan = 1;
                                table2.AddCell(cellPerc1251);

                                PdfPCell cellPerc1261 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc1261.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1261.BorderWidth = 0;
                                cellPerc1261.Colspan = 1;
                                table2.AddCell(cellPerc1261);
                            }
                        }


                        // Compra Calificable
                        if (i == 13)
                        {
                            PdfPCell cellCom1311 = new PdfPCell(new Phrase("Generacion", font_detalle_Grado));
                            cellCom1311.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCom1311.BorderWidth = 0;
                            cellCom1311.Colspan = 1;
                            table2.AddCell(cellCom1311);

                            PdfPCell cellCom1321 = new PdfPCell(new Phrase("4", font_titulo_detalle));
                            cellCom1321.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellCom1321.BorderWidth = 0;
                            cellCom1321.Colspan = 1;
                            table2.AddCell(cellCom1321);

                            PdfPCell cellVol1331 = new PdfPCell(new Phrase("5", font_titulo_detalle));
                            cellVol1331.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellVol1331.BorderWidth = 0;
                            cellVol1331.Colspan = 1;
                            table2.AddCell(cellVol1331);

                            PdfPCell cellVol1341 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellVol1341.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellVol1341.BorderWidth = 0;
                            cellVol1341.Colspan = 1;
                            table2.AddCell(cellVol1341);

                            PdfPCell cellVol1351 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellVol1351.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellVol1351.BorderWidth = 0;
                            cellVol1351.Colspan = 1;
                            table2.AddCell(cellVol1351);

                            PdfPCell cellVol1361 = new PdfPCell(new Phrase(" ", font_titulo_detalle));
                            cellVol1361.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellVol1361.BorderWidth = 0;
                            cellVol1361.Colspan = 1;
                            table2.AddCell(cellVol1361);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1311 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc1311.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1311.BorderWidth = 0;
                                cellPerc1311.Colspan = 3;
                                table2.AddCell(cellPerc1311);
                            }
                            else
                            {
                                PdfPCell cellPerc1321 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc1321.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1321.BorderWidth = 0;
                                cellPerc1321.Colspan = 1;
                                table2.AddCell(cellPerc1321);

                                PdfPCell cellPerc1331 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc1331.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1331.BorderWidth = 0;
                                cellPerc1331.Colspan = 2;
                                table2.AddCell(cellPerc1331);
                            }


                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1341 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc1341.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1341.BorderWidth = 0;
                                cellPerc1341.Colspan = 1;
                                table2.AddCell(cellPerc1341);

                                PdfPCell cellPerc1351 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc1351.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1351.BorderWidth = 0;
                                cellPerc1351.Colspan = 1;
                                table2.AddCell(cellPerc1351);

                                PdfPCell cellPerc1361 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc1361.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1361.BorderWidth = 0;
                                cellPerc1361.Colspan = 1;
                                table2.AddCell(cellPerc1361);
                            }
                            else
                            {
                                PdfPCell cellPerc1341 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc1341.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1341.BorderWidth = 0;
                                cellPerc1341.Colspan = 1;
                                table2.AddCell(cellPerc1341);

                                PdfPCell cellPerc1351 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc1351.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1351.BorderWidth = 0;
                                cellPerc1351.Colspan = 1;
                                table2.AddCell(cellPerc1351);

                                PdfPCell cellPerc1361 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc1361.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1361.BorderWidth = 0;
                                cellPerc1361.Colspan = 1;
                                table2.AddCell(cellPerc1361);
                            }
                        }


                        // Compra
                        if (i == 14)
                        {
                            PdfPCell cellCom1411 = new PdfPCell(new Phrase("Compra:", font_detalle));
                            cellCom1411.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCom1411.BorderWidth = 0;
                            cellCom1411.Colspan = 1;
                            table2.AddCell(cellCom1411);

                            PdfPCell cellCom1421 = new PdfPCell(new Phrase(Volumen4ta, font_Movimientos));
                            cellCom1421.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCom1421.BorderWidth = 0;
                            cellCom1421.Colspan = 1;
                            table2.AddCell(cellCom1421);

                            PdfPCell cellVol1431 = new PdfPCell(new Phrase(Volumen5ta, font_Movimientos));
                            cellVol1431.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1431.BorderWidth = 0;
                            cellVol1431.Colspan = 1;
                            table2.AddCell(cellVol1431);

                            PdfPCell cellVol1441 = new PdfPCell(new Phrase(" ", font_Movimientos));
                            cellVol1441.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1441.BorderWidth = 0;
                            cellVol1441.Colspan = 1;
                            table2.AddCell(cellVol1441);

                            PdfPCell cellVol1451 = new PdfPCell(new Phrase(" ", font_Movimientos));
                            cellVol1451.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1451.BorderWidth = 0;
                            cellVol1451.Colspan = 1;
                            table2.AddCell(cellVol1451);

                            PdfPCell cellVol1461 = new PdfPCell(new Phrase(" ", font_Movimientos));
                            cellVol1461.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1461.BorderWidth = 0;
                            cellVol1461.Colspan = 1;
                            table2.AddCell(cellVol1461);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1411 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc1411.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1411.BorderWidth = 0;
                                cellPerc1411.Colspan = 3;
                                table2.AddCell(cellPerc1411);
                            }
                            else
                            {
                                PdfPCell cellPerc1421 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc1421.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1421.BorderWidth = 0;
                                cellPerc1421.Colspan = 1;
                                table2.AddCell(cellPerc1421);

                                PdfPCell cellPerc1431 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc1431.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1431.BorderWidth = 0;
                                cellPerc1431.Colspan = 2;
                                table2.AddCell(cellPerc1431);
                            }


                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1441 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc1441.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1441.BorderWidth = 0;
                                cellPerc1441.Colspan = 1;
                                table2.AddCell(cellPerc1441);

                                PdfPCell cellPerc1451 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc1451.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1451.BorderWidth = 0;
                                cellPerc1451.Colspan = 1;
                                table2.AddCell(cellPerc1451);

                                PdfPCell cellPerc1461 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc1461.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1461.BorderWidth = 0;
                                cellPerc1461.Colspan = 1;
                                table2.AddCell(cellPerc1461);
                            }
                            else
                            {
                                PdfPCell cellPerc1441 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc1441.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1441.BorderWidth = 0;
                                cellPerc1441.Colspan = 1;
                                table2.AddCell(cellPerc1441);

                                PdfPCell cellPerc1451 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc1451.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1451.BorderWidth = 0;
                                cellPerc1451.Colspan = 1;
                                table2.AddCell(cellPerc1451);

                                PdfPCell cellPerc1461 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc1461.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1461.BorderWidth = 0;
                                cellPerc1461.Colspan = 1;
                                table2.AddCell(cellPerc1461);
                            }
                        }


                        // Compra acumuladas
                        if (i == 15)
                        {
                            PdfPCell cellCom1511 = new PdfPCell(new Phrase("Compra Acumulada:", font_detalle_Grado));
                            cellCom1511.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellCom1511.BorderWidth = 0;
                            cellCom1511.Colspan = 1;
                            table2.AddCell(cellCom1511);


                            PdfPCell cellCom1521 = new PdfPCell(new Phrase("0", font_Movimientos));
                            cellCom1521.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCom1521.BorderWidth = 0;
                            cellCom1521.Colspan = 1;
                            table2.AddCell(cellCom1521);

                            PdfPCell cellVol1531 = new PdfPCell(new Phrase("0", font_Movimientos));
                            cellVol1531.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1531.BorderWidth = 0;
                            cellVol1531.Colspan = 1;
                            table2.AddCell(cellVol1531);

                            PdfPCell cellVol1541 = new PdfPCell(new Phrase("", font_Movimientos));
                            cellVol1541.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1541.BorderWidth = 0;
                            cellVol1541.Colspan = 1;
                            table2.AddCell(cellVol1541);

                            PdfPCell cellVol1551 = new PdfPCell(new Phrase("", font_Movimientos));
                            cellVol1551.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1551.BorderWidth = 0;
                            cellVol1551.Colspan = 1;
                            table2.AddCell(cellVol1551);

                            PdfPCell cellVol1561 = new PdfPCell(new Phrase("", font_Movimientos));
                            cellVol1561.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellVol1561.BorderWidth = 0;
                            cellVol1561.Colspan = 1;
                            table2.AddCell(cellVol1561);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1511 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellPerc1511.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1511.BorderWidth = 0;
                                cellPerc1511.Colspan = 3;
                                table2.AddCell(cellPerc1511);
                            }
                            else
                            {
                                PdfPCell cellPerc1521 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellPerc1521.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1521.BorderWidth = 0;
                                cellPerc1521.Colspan = 1;
                                table2.AddCell(cellPerc1521);

                                PdfPCell cellPerc1531 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellPerc1531.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellPerc1531.BorderWidth = 0;
                                cellPerc1531.Colspan = 2;
                                table2.AddCell(cellPerc1531);
                            }


                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellPerc1541 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellPerc1541.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1541.BorderWidth = 0;
                                cellPerc1541.Colspan = 1;
                                table2.AddCell(cellPerc1541);

                                PdfPCell cellPerc1551 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellPerc1551.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1551.BorderWidth = 0;
                                cellPerc1551.Colspan = 1;
                                table2.AddCell(cellPerc1551);

                                PdfPCell cellPerc1561 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellPerc1561.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1561.BorderWidth = 0;
                                cellPerc1561.Colspan = 1;
                                table2.AddCell(cellPerc1561);
                            }
                            else
                            {
                                PdfPCell cellPerc1541 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellPerc1541.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1541.BorderWidth = 0;
                                cellPerc1541.Colspan = 1;
                                table2.AddCell(cellPerc1541);

                                PdfPCell cellPerc1551 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellPerc1551.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1551.BorderWidth = 0;
                                cellPerc1551.Colspan = 1;
                                table2.AddCell(cellPerc1551);

                                PdfPCell cellPerc1561 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellPerc1561.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellPerc1561.BorderWidth = 0;
                                cellPerc1561.Colspan = 1;
                                table2.AddCell(cellPerc1561);
                            }
                        }



                        // Compra
                        if (i > 15)
                        {
                            PdfPCell cellResto111 = new PdfPCell(new Phrase("", font_detalle_Grado));
                            cellResto111.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellResto111.BorderWidth = 0;
                            cellResto111.Colspan = 6;
                            table2.AddCell(cellResto111);

                            if (Concepto == "Subtotal:" || Concepto == "Deducciones" || Concepto == "Total Neto:" || Concepto == "Fecha de Pago:" || Concepto == " " || Concepto == "Total Pagos:")
                                {
                                PdfPCell cellResto121 = new PdfPCell(new Phrase(Concepto, font_detalle));
                                cellResto121.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellResto121.BorderWidth = 0;
                                cellResto121.Colspan = 3;
                                table2.AddCell(cellResto121);
                            }
                            else
                            {
                                PdfPCell cellResto1121 = new PdfPCell(new Phrase(ConceptoId, font_Movimientos));
                                cellResto1121.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellResto1121.BorderWidth = 0;
                                cellResto1121.Colspan = 1;
                                table2.AddCell(cellResto1121);

                                PdfPCell cellResto1131 = new PdfPCell(new Phrase(Descripcion, font_Movimientos));
                                cellResto1131.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellResto1131.BorderWidth = 0;
                                cellResto1131.Colspan = 2;
                                table2.AddCell(cellResto1131);
                            }

                            if (Concepto == "Total Neto:" || Concepto == "Total Pagos:")
                            {
                                PdfPCell cellResto1141 = new PdfPCell(new Phrase(PrimerQuincena, font_detalle));
                                cellResto1141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellResto1141.BorderWidth = 0;
                                cellResto1141.Colspan = 1;
                                table2.AddCell(cellResto1141);

                                PdfPCell cellResto1151 = new PdfPCell(new Phrase(SegundaQuincena, font_detalle));
                                cellResto1151.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellResto1151.BorderWidth = 0;
                                cellResto1151.Colspan = 1;
                                table2.AddCell(cellResto1151);

                                PdfPCell cellResto1161 = new PdfPCell(new Phrase(Totales, font_detalle));
                                cellResto1161.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellResto1161.BorderWidth = 0;
                                cellResto1161.Colspan = 1;
                                table2.AddCell(cellResto1161);
                            }
                            else
                            {
                                PdfPCell cellResto141 = new PdfPCell(new Phrase(PrimerQuincena, font_Movimientos));
                                cellResto141.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellResto141.BorderWidth = 0;
                                cellResto141.Colspan = 1;
                                table2.AddCell(cellResto141);

                                PdfPCell cellResto151 = new PdfPCell(new Phrase(SegundaQuincena, font_Movimientos));
                                cellResto151.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellResto151.BorderWidth = 0;
                                cellResto151.Colspan = 1;
                                table2.AddCell(cellResto151);

                                PdfPCell cellResto161 = new PdfPCell(new Phrase(Totales, font_Movimientos));
                                cellResto161.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cellResto161.BorderWidth = 0;
                                cellResto161.Colspan = 1;
                                table2.AddCell(cellResto161);
                            }


                        }
                    }

                    pdfDoc.Add(table2);
                    writer.CloseStream = false;
                }

                //var fromAddress = new MailAddress("figsa@live.com.mx", "HERBAX de Mexico");
                //string fromPassword = "f92130121juan";
                //var toAddress = new MailAddress("david.cruz@grupovitek.com.mx");
                ////var toAddress = new MailAddress(EmailSocio);
                //pdfDoc.Close();
                //mem.Position = 0;
                //string Contexto = "Estado de Cuenta del Mes";
                //var pNombreSocio = NomSocio;
                //var pHtmlBody = "<div>Buenos días <br/> <br/>  " + System.Environment.NewLine + "Por medio del presente le envio el estado de cuenta del mes" + System.Environment.NewLine + "Cualquier duda o comentario estamos a sus ordenes" + System.Environment.NewLine + " <br/> <br/> Que tenga un exelente día. </div>";
                //MailMessage mm = new MailMessage(fromAddress, toAddress)
                //{ Subject = Contexto, IsBodyHtml = true, Body = pHtmlBody };
                //mm.Attachments.Add(new Attachment(mem, strFiles));
                //SmtpClient smtp = new SmtpClient();                
                //smtp.Host = "smtp.live.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);
                //smtp.Send(mm);

                pdfDoc.Close();
                mem.Position = 0;
                MailMessage correo = new MailMessage();
                //Configuration c = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                //Configuration c = System.Configuration.Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                //Configuration c = WebConfigurationManager.OpenWebConfiguration("/");
                Configuration c = WebConfigurationManager.OpenWebConfiguration("~");
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)c.GetSectionGroup("system.net/mailSettings");
                correo.From = new System.Net.Mail.MailAddress(settings.Smtp.From, settings.Smtp.Network.UserName, System.Text.Encoding.UTF8); //Correo de salida
                correo.To.Add(EmailSocio); //Correo destino
                //correo.To.Add("david.cruz@grupovitek.com.mx"); //Correo destino
                correo.Attachments.Add(new Attachment(mem, strFiles));
                correo.Subject = "Estado de Cuenta del Mes"; //Asunto
                correo.Body = "<div>Buenos días <br/> <br/>  " + System.Environment.NewLine + "Por medio del presente le envio el estado de cuenta del mes" + System.Environment.NewLine + "Cualquier duda o comentario estamos a sus ordenes" + System.Environment.NewLine + " <br/> <br/> Que tenga un exelente día. </div>"; //Mensaje del correo
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = settings.Smtp.Network.DefaultCredentials;
                smtp.Host = settings.Smtp.Network.Host; //Host del servidor de correo
                smtp.Port = settings.Smtp.Network.Port; //Puerto de salida
                smtp.Credentials = new System.Net.NetworkCredential(settings.Smtp.From, settings.Smtp.Network.Password); //Cuenta de correo
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.EnableSsl = settings.Smtp.Network.EnableSsl;//True si el servidor de correo permite ssl
                smtp.Send(correo);



                ViewBag.IsOk = 1;
                ViewBag.Mensaje = "La Estado de cuenta se envio via al correo de socio";
                return Json(ViewBag.IsOk, JsonRequestBehavior.AllowGet);
            }
            }
            catch (Exception ex)
            {
                var IsOk = 0;
                var Mensaje = ex.InnerException.Message.ToString();
                return this.Json(new { IsOk, Mensaje }, JsonRequestBehavior.AllowGet);

            };

        }



        public JsonResult FiltrarSocios(EdoDeCuentaSocioViewModel modelConsulta)
        {
            //CO_st_BuscarDatosPagoSocio
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaEstadosDeCuenta3_Result> listaSocios = new List<CO_st_BuscaEstadosDeCuenta3_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //campos que Retorna: partner_id, tax_id, social_security, bank_id, account, Paymethod_com
                    listaSocios = hbxEntities.CO_st_BuscaEstadosDeCuenta3(modelConsulta.partner_id, modelConsulta.Anio, modelConsulta.Mes, modelConsulta.Nombre, modelConsulta.Direccion).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_BuscaEstadosDeCuenta  DETALLE: " + ex);
                }


            }
            return Json(listaSocios, JsonRequestBehavior.AllowGet);

            //return View();
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
                var IsOk = 0;
                var Mensaje = ex.InnerException.Message.ToString();
                return this.Json(new { IsOk, Mensaje }, JsonRequestBehavior.AllowGet);

            };
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


        public Stream GetStreamFile(string filePath)
        {
            using (FileStream fileStream = System.IO.File.OpenRead(filePath))
            {
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                return memStream;
            }
        }


        //public MailMessage MensajeCorreo(string archivo)
        //{
        //    using (FileStream fileStream = System.IO.File.OpenRead("C:\\Inetpub\\wwwroot\\EvoComisiones\\EdoDeCtaPDF\\" + archivo))
        //    {
        //        MemoryStream memStream = new MemoryStream();
        //        memStream.SetLength(fileStream.Length);
        //        fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

        //        MailMessage mensaje = new MailMessage();
        //        mensaje.Subject = "DSI - Certificado";
        //        mensaje.To.Add(new MailAddress("david.cruz@grupovitek.com.mx"));
        //        mensaje.CC.Add(new MailAddress("erodriguez@herbax.com.mx"));
        //        mensaje.From = new MailAddress("herbax2019@gmail.com", "HERBAX de Mexico");
        //        mensaje.Attachments.Add(new Attachment(memStream, Path.GetFileName(archivo), "application/pdf"));
        //        mensaje.Body = "Hola este es un mensaje de prueba";

        //        return mensaje;
        //    }

        //}//fin MensajeCorreo

    }//fin de la clase
}


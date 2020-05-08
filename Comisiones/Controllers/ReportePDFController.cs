using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class ReportePDFController : Controller
    {
        // GET: ReportePDF
        public ActionResult Index(string partner_id, string Nombre, string Direccion, int? Anio, int? Mes)
        {
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();






            PdfPTable table = new PdfPTable(8);
            PdfPCell cell = new PdfPCell();

            //Image image = Image.GetInstance(Server.MapPath("~/myPic.jpg"));
            //cell.AddElement(image);
            //table.AddCell(cell);
            Paragraph para1 = new Paragraph("texto uno");
            cell = new PdfPCell();
            cell.AddElement(para1);
            table.AddCell(cell);

            Paragraph para2 = new Paragraph("Your Text");
            cell = new PdfPCell();
            cell.AddElement(para2);
            table.AddCell(cell);


            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Credit-Card-Report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return View();
        }
    }

}
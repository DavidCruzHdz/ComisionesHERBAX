using System.Collections.Generic;

namespace Comisiones.Models
{
    public class PdfExample
    {
        public string Heading { get; set; }
        public IEnumerable<BasketItem> Items { get; set; }
    }
}
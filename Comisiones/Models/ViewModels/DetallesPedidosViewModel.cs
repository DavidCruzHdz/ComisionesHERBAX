using System;

namespace Comisiones.Models.ViewModels
{
    public class DetallesPedidos
    {
        public string Id_MovInv { get; set; }
        public int Id_DtlMovInv { get; set; }
        public int Id_Pedido { get; set; }
        public string Id_Producto { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Impuesto { get; set; }
        public Nullable<decimal> Porc_Impuesto { get; set; }
        public int Id_Talla { get; set; }
        public Nullable<decimal> Desc_Importe { get; set; }
        public Nullable<decimal> Desc_Impuesto { get; set; }
        public string Id_Empresa { get; set; }
    }
}
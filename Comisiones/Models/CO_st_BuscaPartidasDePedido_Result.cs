//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Comisiones.Models
{
    using System;
    
    public partial class CO_st_BuscaPartidasDePedido_Result
    {
        public int Id_Pedido { get; set; }
        public string Id_MovInv { get; set; }
        public int Id_DtlMovInv { get; set; }
        public string Id_Producto { get; set; }
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

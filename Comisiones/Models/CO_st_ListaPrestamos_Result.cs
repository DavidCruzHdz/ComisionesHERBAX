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
    
    public partial class CO_st_ListaPrestamos_Result
    {
        public int periodoCreacion { get; set; }
        public string partner_id { get; set; }
        public string nombre { get; set; }
        public decimal importe { get; set; }
        public decimal saldo { get; set; }
        public string Motivo { get; set; }
        public Nullable<int> idFormaPago { get; set; }
        public string descFormaPago { get; set; }
    }
}
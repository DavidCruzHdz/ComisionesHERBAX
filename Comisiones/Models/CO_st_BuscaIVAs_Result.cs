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
    
    public partial class CO_st_BuscaIVAs_Result
    {
        public int id_pais { get; set; }
        public string pais { get; set; }
        public Nullable<int> id_regimen { get; set; }
        public string descripcion { get; set; }
        public Nullable<decimal> iva { get; set; }
        public Nullable<System.DateTime> fecha_ult_modificacion { get; set; }
        public string usuario_ult_modificacion { get; set; }
    }
}

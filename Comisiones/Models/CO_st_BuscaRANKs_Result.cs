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
    
    public partial class CO_st_BuscaRANKs_Result
    {
        public int id_ISR { get; set; }
        public int id_pais { get; set; }
        public string pais { get; set; }
        public Nullable<int> id_regimen { get; set; }
        public string descripcion { get; set; }
        public Nullable<decimal> montominimo { get; set; }
        public Nullable<decimal> limiteinferior { get; set; }
        public Nullable<decimal> limitesuperior { get; set; }
        public Nullable<decimal> ISR { get; set; }
    }
}

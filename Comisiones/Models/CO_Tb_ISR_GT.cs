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
    using System.Collections.Generic;
    
    public partial class CO_Tb_ISR_GT
    {
        public int id_ISR { get; set; }
        public Nullable<int> id_pais { get; set; }
        public Nullable<int> id_regimen { get; set; }
        public Nullable<decimal> montominimo { get; set; }
        public Nullable<decimal> limiteinferior { get; set; }
        public Nullable<decimal> limitesuperior { get; set; }
        public Nullable<decimal> ISR { get; set; }
        public Nullable<System.DateTime> fecha_ult_modificacion { get; set; }
        public string usuario_ult_modificacion { get; set; }
    }
}

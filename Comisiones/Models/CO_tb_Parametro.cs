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
    
    public partial class CO_tb_Parametro
    {
        public int id_Parametro { get; set; }
        public string Key_parametro { get; set; }
        public string parametro { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> valor { get; set; }
        public string valor_str { get; set; }
        public Nullable<decimal> valor_dec { get; set; }
        public int estatus { get; set; }
        public string usuario { get; set; }
        public Nullable<System.DateTime> fechaMovimiento { get; set; }
    }
}
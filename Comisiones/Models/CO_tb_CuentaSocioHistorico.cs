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
    
    public partial class CO_tb_CuentaSocioHistorico
    {
        public int id_CuentaSocioHistorico { get; set; }
        public string partner_id { get; set; }
        public Nullable<int> id_Banco { get; set; }
        public string Clabe { get; set; }
        public string usuario { get; set; }
        public Nullable<System.DateTime> fechaMovimiento { get; set; }
    }
}

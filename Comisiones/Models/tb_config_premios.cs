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
    
    public partial class tb_config_premios
    {
        public int idPremio { get; set; }
        public Nullable<int> idGrupo { get; set; }
        public Nullable<int> puntos { get; set; }
        public Nullable<int> mes { get; set; }
        public Nullable<int> quincena { get; set; }
        public Nullable<int> idConcepto { get; set; }
        public Nullable<System.DateTime> fechaInicio { get; set; }
        public Nullable<System.DateTime> fechaFin { get; set; }
        public Nullable<int> estatus { get; set; }
        public Nullable<int> idPais { get; set; }
    }
}

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
    
    public partial class CO_st_MovientosDetalle_Result
    {
        public string Partner_id { get; set; }
        public string NombreDistribuidor { get; set; }
        public int Id_MovimientosSocio { get; set; }
        public int id_Concepto { get; set; }
        public string DescripcionConcepto { get; set; }
        public string MontoStr { get; set; }
        public string tipoMovimiento { get; set; }
        public string Comentario { get; set; }
        public Nullable<int> Id_Periodo { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string Estatus { get; set; }
        public int isDelete { get; set; }
    }
}
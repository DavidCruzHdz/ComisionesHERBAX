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
    
    public partial class CO_st_BuscaConfigReembolsos_Result
    {
        public int Id_Reembolso { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Cantidad_Topada { get; set; }
        public decimal PorcentajeReembolso { get; set; }
        public string Concepto { get; set; }
        public string pais { get; set; }
    }
}
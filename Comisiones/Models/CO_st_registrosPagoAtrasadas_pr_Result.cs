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
    
    public partial class CO_st_registrosPagoAtrasadas_pr_Result
    {
        public int TipoRegistro { get; set; }
        public int NoIdentificaciónArchivo { get; set; }
        public Nullable<System.DateTime> FechaEnvio { get; set; }
        public string VersionLayout { get; set; }
        public int TipoRegistroDet { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string email { get; set; }
        public string Importe { get; set; }
        public string Nombre { get; set; }
        public string Clabe { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public Nullable<int> Periodo { get; set; }
        public string id_Distribuidor { get; set; }
        public int TipoRegistroCtrl { get; set; }
        public Nullable<int> NoMovimientos { get; set; }
        public string ImpTotalAbonos { get; set; }
        public Nullable<decimal> IvaDelimTotal { get; set; }
        public decimal ImporteVista { get; set; }
    }
}

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
    
    public partial class CO_st_GetComisionesPeriodoActivo1_Result
    {
        public Nullable<int> id_Periodo { get; set; }
        public Nullable<decimal> tipocambio { get; set; }
        public Nullable<decimal> montoUSD { get; set; }
        public Nullable<int> keyPais { get; set; }
        public Nullable<decimal> MontoMonedaLocal { get; set; }
        public string Pais { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Quincena { get; set; }
    }
}

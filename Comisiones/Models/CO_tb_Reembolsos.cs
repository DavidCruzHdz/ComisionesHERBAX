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
    
    public partial class CO_tb_Reembolsos
    {
        public int ID_PERIODO { get; set; }
        public string ID_PARTNER { get; set; }
        public Nullable<int> MES { get; set; }
        public Nullable<decimal> VENTA_PERIODO { get; set; }
        public Nullable<decimal> CANTIDAD_REEMBOLSO { get; set; }
        public string NOMBRE_CUSTOMER { get; set; }
        public Nullable<int> ESTATUS { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public Nullable<bool> PedidoGenerado { get; set; }
        public Nullable<int> Id_Pedido { get; set; }
        public string Situacion { get; set; }
    }
}
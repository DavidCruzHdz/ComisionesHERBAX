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
    
    public partial class CO_tb_RecepcionDepositos
    {
        public int id_RecepcionDeposito { get; set; }
        public string numeroReferencia { get; set; }
        public string cuentaOrigen { get; set; }
        public string cuentaDestino { get; set; }
        public string beneficiario { get; set; }
        public Nullable<decimal> importe { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public string estatus { get; set; }
        public string conceptoPagoTransf { get; set; }
        public string referenciaInterban { get; set; }
        public string formaAplicacion { get; set; }
        public Nullable<System.DateTime> fechaAplicacion { get; set; }
        public string claveRastreo { get; set; }
        public string motivoDevolucion { get; set; }
        public Nullable<decimal> iva { get; set; }
        public string rfc { get; set; }
        public string comprobanteElectronico { get; set; }
        public string divisa { get; set; }
        public string usuario { get; set; }
        public Nullable<System.DateTime> fechaMovimiento { get; set; }
    }
}

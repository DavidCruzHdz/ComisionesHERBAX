//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Comisiones.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class CO_tb_Productos_Reembolso
    {
        public int Id_Reembolso { get; set; }
        public string sku { get; set; }
        public string Id_Empresa { get; set; }
        public Nullable<bool> Estatus { get; set; }
        public Nullable<System.DateTime> FechaAlta { get; set; }
        public Nullable<int> UsuarioAlta { get; set; }
        public Nullable<System.DateTime> FechaCambio { get; set; }
        public Nullable<int> UsuarioCambio { get; set; }


        public string Id_Socio { get; set; }
        public string warehouse { get; set; }
        public int Warehouse_ID { get; set; }
        public string cve_art { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCorta { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<double> PrecioPublico { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<double> PrecioMinimo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<double> PrecioCosto { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<double> PrecioSocio { get; set; }
        [RegularExpression(@"[0-9]*", ErrorMessage = "La cantidad debe contener sólo números sin decimales")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int CantidadProducto { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<double> SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public string ActivoSAE { get; set; }
        public int Id_Periodo { get; set; }



        public virtual CO_tb_ConfigReembolsos CO_tb_ConfigReembolsos { get; set; }
    }
}
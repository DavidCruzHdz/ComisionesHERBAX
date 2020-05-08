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
    using System.ComponentModel.DataAnnotations;

    public partial class CO_st_BuscaProductosReembolsos1_Result
    {
        public string Id_Empresa { get; set; }
        public string Id_Socio { get; set; }
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<double> Venta { get; set; }
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<double> Reembolso { get; set; }
        public string sku { get; set; }
        public string warehouse { get; set; }
        public int Warehouse_ID { get; set; }
        public string cve_art { get; set; }
        public string Descripcion { get; set; }
        //public bool checkeado { get; set; }
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
        //[RegularExpression(@"^\d+\.\d{0,0}$", ErrorMessage = "La cantidad debe contener sólo números sin decimales")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int CantidadProducto { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<double> SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        //public Nullable<double> Diferencia { get; set; }
        public string ActivoSAE { get; set; }

        public int Id_Periodo { get; set; }
        public int Pais { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Topada { get; set; }
        public decimal Porcentaje { get; set; }

        public bool checkeado { get; set; }
        public int Id_Pais { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }

    }
}


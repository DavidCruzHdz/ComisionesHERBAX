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

    public partial class CO_tb_ConfigReembolsos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CO_tb_ConfigReembolsos()
        {
            this.CO_tb_Productos_Reembolso = new HashSet<CO_tb_Productos_Reembolso>();
        }
    
        public int Id_Reembolso { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Cantidad { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Cantidad_Topada { get; set; }
        public decimal PorcentajeReembolso { get; set; }
        public int id_concepto { get; set; }
        public int id_estatus { get; set; }
        public Nullable<int> id_pais { get; set; }
        public string UltMod_Usuario { get; set; }
        public Nullable<System.DateTime> UltMod_F { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CO_tb_Productos_Reembolso> CO_tb_Productos_Reembolso { get; set; }
    }
}
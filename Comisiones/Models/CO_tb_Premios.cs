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
    
    public partial class CO_tb_Premios
    {
        public int id_premio { get; set; }
        public string nombre_premio { get; set; }
        public Nullable<int> id_pais { get; set; }
        public Nullable<int> id_periodo { get; set; }
        public Nullable<bool> Estatus { get; set; }
        public Nullable<bool> Pagado { get; set; }
        public string CreadoUsuario { get; set; }
        public Nullable<System.DateTime> Creado_F { get; set; }
        public string UltModUsuario { get; set; }
        public Nullable<System.DateTime> UltMod_F { get; set; }
    }
}

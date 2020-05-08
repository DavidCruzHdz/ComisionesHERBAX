namespace Comisiones.Models.ViewModels
{
    public class Rango_Compra_PremiosViewModel
    {
        public int id_Rango { get; set; }
        public int id_pais { get; set; }
        public int id_premio { get; set; }
        public int id_rank { get; set; }
        public int id_periodo { get; set; }
        public decimal puntos { get; set; }
        public bool Estatus { get; set; }
        public string CreadoUsuario { get; set; }
        public System.DateTime Creado_F { get; set; }
        public string UltModUsuario { get; set; }
        public System.DateTime UltMod_F { get; set; }

        public string DescripcionPais { get; set; }
        public string DescripcionPremio { get; set; }
        public string DescripcionRank { get; set; }
        public string DescripcionEstatus { get; set; }


    }
}
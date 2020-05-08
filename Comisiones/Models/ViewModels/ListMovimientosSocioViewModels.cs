using System;

namespace Comisiones.Models.ViewModels
{
    public class ListMovimientosSocioViewModels
    {
        public int Id_MovimientosSocio { get; set; }

        public string Partner_id { get; set; }

        //private string _tipoMovimiento=string.Empty;
        public string tipoMovimiento { get; set; }

        public string NombreDistribuidor { get; set; }

        public int Id_Concepto { get; set; }

        public string TipoConcepto { get; set; }

        public string DescripcionConcepto { get; set; }

        public string Comentario { get; set; }

        public int Estatus { get; set; }

        public string strEstatus { get; set; }

        public decimal Monto { get; set; }

        public string montoFormatoMoneda
        {
            get
            {
                string _simbolTipoMov = string.Empty;
                if (tipoMovimiento.ToLower() == "p") { _simbolTipoMov = "+ "; }
                if (tipoMovimiento.ToLower() == "d") { _simbolTipoMov = "- "; }
                return (_simbolTipoMov +
                    //string.Format("{0:C2}", Monto)
                    Monto.ToString("C", new System.Globalization.CultureInfo("es-MX"))
                    );
            }
        }

        public DateTime FechaRegistro { get; set; }

        public string fechaFormatoCorta
        {
            get
            {
                return string.Format("{0:dd-MMM-yyyy}", FechaRegistro);
            }
        }

        public int Id_Periodo { get; set; }
    }
}
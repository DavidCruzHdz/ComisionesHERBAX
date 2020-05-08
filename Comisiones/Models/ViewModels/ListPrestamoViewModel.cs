using System;

namespace Comisiones.Models.ViewModels
{
    public class ListPrestamoViewModel
    {
        public int Id_prestamo { get; set; }
        public string Partner_id { get; set; }
        public decimal Importe { get; set; }
        public string TipoAplicacion { get; set; }
        public int NumeroPagos { get; set; }
        public int IdConcepto { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Estatus { get; set; } //  --- [1 SOLICITUD, 2 APROBADO,  3 PROCESO DE COBRO, 3 PAGADO]
        public string Usuario { get; set; }
        public DateTime FechaMovimiento { get; set; }

    }
}
namespace Comisiones.Models.ViewModels
{
    public class PeriodosViewModel
    {

        public int idPeriodo { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
        public int quincena { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int estatus { get; set; }
        public string AccionSend { get; set; }



    }
}
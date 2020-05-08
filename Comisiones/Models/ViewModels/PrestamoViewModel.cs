using System;

namespace Comisiones.Models.ViewModels
{
    public class PrestamoViewModel
    {

        public string Partner_id { get; set; }
        public decimal Importe { get; set; } // [decimal](18, 2) NOT NULL, 
        public int NumeroPagos { get; set; }// [int] NOT NULL,
        public int id_Concepto { get; set; } // [int] NOT NULL,
        public int id_Periodo { get; set; }  // [int] NOT NULL,

        public DateTime fechaInicio { get; set; } // [date]NULL,
        public DateTime fechaFin { get; set; } //[date]NULL,
        public string usuario { get; set; } //[varchar](150) NULL, Usuario que genero el movimiento
        public DateTime fechaMovimiento { get; set; }  //[smalldatetime]NULL, Fecha del movimiento

        public string motivo { get; set; }//[varchar](100) NULL,  descripcion de para pide el prestamo
        public int idFormaPago { get; set; } //identifica si es Quincenal=2 o Mensual =1
                                             /*     

                                             //[Required(ErrorMessage = "El campo {0} es obligatorio")]
                                             //[Display(Name = "Importe Prestamo")]
                                             [DataType(DataType.Currency)]
                                             public decimal Importe { get; set; }
                                             //[Required(ErrorMessage = "El campo {0} es obligatorio")]
                                             // [Display(Name = "Tipo Aplicación")]
                                             public Int32 TipoAplicacion { get; set; }
                                             //[Required(ErrorMessage = "El campo {0} es obligatorio")]
                                             //[Display(Name = "Numero de Pagos")]
                                             public Int32 NumeroPagos { get; set; }
                                             //[Required(ErrorMessage = "El campo {0} es obligatorio")]
                                             //[Display(Name = "Motivo Prestamo")]
                                             public Int32 IdConcepto { get; set; }
                                             public decimal Saldo { get; set; }
                                             public DateTime FechaInicio { get; set; }
                                             public DateTime FechaFin { get; set; }
                                             public Int32 Estatus { get; set; } //  --- [1 SOLICITUD, 2 APROBADO,  3 PROCESO DE COBRO, 3 PAGADO]
                                             public string Usuario { get; set; }
                                             //[DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
                                             public DateTime FechaMovimiento { get; set; }
                                             public Int32 idFormaPago { get; set; }
                                             public string motivo { get; set; }

                                             */

    }
}
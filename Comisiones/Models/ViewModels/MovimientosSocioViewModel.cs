using System.ComponentModel.DataAnnotations;

namespace Comisiones.Models.ViewModels
{
    public class MovimientosSocioViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Partner_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string tipoConcepto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Id_Concepto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string comentario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal monto { get; set; }



        public int Id_Periodo { get; set; }
        public string nombreSocio { get; set; }

        public int Id_MovimientosSocio { get; set; }

        public string DescripcionConcepto { get; set; }

        /*    
              [Display(Name = "Socio")]
              
              public string NombreDistribuidor { get; set; }
              
              [Required(ErrorMessage = "El campo {0} es obligatorio")]
              [Display(Name = "Concepto")]
              

              [Required(ErrorMessage = "El campo {0} es obligatorio")]
              [Display(Name = "Importe Préstamo")]
              [DataType(DataType.Currency)]
 
              public decimal Comisiones { get; set; }
   
      */

    }
}
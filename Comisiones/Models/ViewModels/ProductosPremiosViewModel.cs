using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Comisiones.Models.ViewModels
{
    public class ProductosPremiosViewModel : Controller
    {
        // GET: ProductosPremiosViewModel
        public CO_tb_Rango_Compra_Premios Model1 { get; set; }
        public CO_tb_Productos_Reembolso Model2 { get; set; }

    }
}


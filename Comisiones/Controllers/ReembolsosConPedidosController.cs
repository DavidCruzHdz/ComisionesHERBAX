namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using Comisiones.Models.ModelsEvolution;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using Comisiones.Data;
    using System.IO;
    using Comisiones.Models.ViewModels;


    public class ReembolsosConPedidosController : Controller
    {
        // GET: RembolsosConPedidos
        private string nameController = "ReembolsosConPedidosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private herbaxEntities db = new herbaxEntities();

        // GET: ReembolsosCanje
        public ActionResult Index()
        {
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 20 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }
            else
            {
                ViewBag.Message = "";
                ViewBag.IsPedido = 0;

                Log oLog = new Log(rutaArchivoLog);
                List<CO_st_BuscaPedidosReembolsos4_Result> LReembolsos = new List<CO_st_BuscaPedidosReembolsos4_Result>();
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    try
                    {
                        LReembolsos = hbxEntities.CO_st_BuscaPedidosReembolsos4().ToList();
                        ViewBag.Reembolsos = LReembolsos;

                        var detalles = db.CO_tb_DtlPedidos.ToList();
                        return View(detalles.ToList());
                    }
                    catch (Exception ex)
                    {
                        ViewBag.IsOK = 0;
                        var Col = 0;
                        var resultado = "";

                        resultado = ex.InnerException.Message.ToString();
                        Col = resultado.IndexOf("\r");

                        if (Col.ToString() == "")
                        {
                            var NewString = resultado.Substring(0, Col);
                            ViewBag.Message = NewString;
                        }
                        else
                        {
                            ViewBag.Message = resultado;
                        }

                        return View();
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Index(IList<CO_tb_Productos_Reembolso> entity)
        {
            var varFolio = "";
            var NumPartida = 0;

            try
            {
                var Folios = db.CO_tb_FoliosMovInventarios.Where(s => (s.Estatus == true)).FirstOrDefault();

                if (Folios.Folio_Actual != "0")
                {
                    varFolio = (Decimal.Parse(Folios.Folio_Actual.ToString()) + 1).ToString();
                }
                else
                {
                    varFolio = (Folios.Folio_Inicio);
                }

                var varSocio = "";
                var varEmpresa = "";
                Double varDiferencia = 0;
                Double varSubTotal = 0;
                Double varMontoPda = 0;
                Double varCantidad = 0;
                Double varPrecio = 0;


                foreach (var item in entity)
                {
                    NumPartida = NumPartida + 1;

                    if (NumPartida == 1)
                    {
                        varSocio = item.Id_Socio;


                        varEmpresa = item.Id_Empresa;
                    }

                    if (item.CantidadProducto > 0)
                    {
                        varCantidad = Double.Parse(item.CantidadProducto.ToString());
                        varPrecio = Double.Parse(item.PrecioSocio.ToString());
                        varMontoPda = varCantidad * varPrecio;
                        varSubTotal = varSubTotal + varMontoPda;

                    }
                }


                var varImpuesto = 0;
                var varTotal = 0;
                var NomUsuario = Session["IdNombre"].ToString();

                var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                var varUsuario = Usuarios.IdUsuario;
                //var varSocioId = 0;
                //varSocioId = int.Parse(varSocio.ToString());

                var varReembolsos = db.CO_tb_Reembolsos.Where(x => x.ID_PARTNER == varSocio).FirstOrDefault();
                var varPeriodo = varReembolsos.ID_PERIODO;
                var varVentas = Double.Parse(varReembolsos.VENTA_PERIODO.ToString());
                var varReembolso = Double.Parse(varReembolsos.CANTIDAD_REEMBOLSO.ToString());

                varDiferencia = varReembolso - varSubTotal;

                EvolutionEntities Evo = new EvolutionEntities();
                var PeriodosEvo = Evo.CO_tb_Periodo.Where(x => x.Id_Periodo == varPeriodo).FirstOrDefault();
                var AnioEvo = PeriodosEvo.Anio;
                var MesEvo = PeriodosEvo.Mes;
                var QuincenaEvo = PeriodosEvo.Quincena;
                var Periodo = PeriodosEvo.Id_Periodo;

                var GeneraPedido = db.CO_st_GeneraMovimientosPedido(varFolio, varSocio, 70, varEmpresa, varDiferencia, varReembolso, varImpuesto, varTotal, NumPartida, varVentas, varUsuario, AnioEvo, MesEvo, QuincenaEvo, Periodo);
                var valorPedido = int.Parse(GeneraPedido.ToString());

                var TraerPedido = db.CO_tb_Pedidos.Where(x => x.Id_MovInv == varFolio).FirstOrDefault();
                var varPedido = TraerPedido.Id_Pedido;

                CO_tb_DtlPedidos Partidas = new CO_tb_DtlPedidos();
                NumPartida = 0;
                foreach (var item in entity)
                {
                    NumPartida = NumPartida + 1;

                    if (item.CantidadProducto > 0)
                    {
                        Partidas.Id_Pedido = varPedido;
                        Partidas.Id_MovInv = varFolio;
                        Partidas.Id_DtlMovInv = NumPartida;
                        Partidas.Id_Producto = item.sku;
                        Partidas.Cantidad = item.CantidadProducto;
                        Partidas.Precio = Decimal.Parse(item.PrecioSocio.ToString());
                        Partidas.Impuesto = 0;
                        Partidas.Porc_Impuesto = 0;
                        Partidas.Id_Talla = 0;
                        Partidas.Desc_Importe = 0;
                        Partidas.Desc_Impuesto = 0;
                        Partidas.Id_Empresa = item.Id_Empresa;
                        db.CO_tb_DtlPedidos.Add(Partidas);
                        db.SaveChanges();
                    }

                }


                var TranspasaPedidoEvoPT = db.CO_st_TranspasaMovimientosPedido(varFolio, varSocio);


                ViewBag.IsOK = 1;
                ViewBag.Message = "Se genero el pedido y se transfieron los datos de inventario";
                //return View();
                return RedirectToAction("Index", "ReembolsosCanje");
            }
            catch (Exception e)
            {

                ViewBag.IsOK = 0;
                ViewBag.Message = e;
                return View();
            }
        }



        public ActionResult ResolveText(int? NumPedido)
        {
            List<CO_st_BuscaPartidasDePedido1_Result> LPedidosReembolsos = new List<CO_st_BuscaPartidasDePedido1_Result>();
            using (herbaxEntities db = new herbaxEntities())
            {
                //var results = db.CO_st_BuscaPartidasDePedido(Convert.ToInt32(letterId), text);
                var results = db.CO_st_BuscaPartidasDePedido1(NumPedido).ToList();
                return View(results);
            }


        }


        public JsonResult BuscaPartidas(String PedidoId)
        {
            List<CO_st_BuscaDetalleDelPedido2_Result> LPartidas = new List<CO_st_BuscaDetalleDelPedido2_Result>();
            using (herbaxEntities db = new herbaxEntities())
            {
                try
                {

                    var Pedido = int.Parse(PedidoId.ToString());
                    LPartidas = db.CO_st_BuscaDetalleDelPedido2(Pedido).ToList();

                }
                catch (Exception ex)
                {
                    //oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaReembolsos  DETALLE: " + ex);
                }


            }
            return Json(LPartidas, JsonRequestBehavior.AllowGet);
        }





        public JsonResult BuscaPedidos()
        {
            //Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaPedidosReembolsos4_Result> LReembolsos = new List<CO_st_BuscaPedidosReembolsos4_Result>();
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    var VarEstatus = 14;
                    LReembolsos = hbxEntities.CO_st_BuscaPedidosReembolsos4().ToList();

                }
                catch (Exception ex)
                {
                    //oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaReembolsos  DETALLE: " + ex);
                }


            }
            return Json(LReembolsos, JsonRequestBehavior.AllowGet);

        }

    }
}

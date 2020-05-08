namespace Comisiones.Controllers
{
    using Comisiones.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Comisiones.Models.ModelsEvolution;
    using System.Data;
    using Comisiones.Data;
    using System.IO;
    using Comisiones.Models.ViewModels;

    public class CalculaPremioController : Controller
    {
        private string nameController = "CalculaPremiosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private herbaxEntities db = new herbaxEntities();
        // GET: CalculaPremios
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 15 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            ViewBag.Message = "";
            ViewBag.IsPedido = 0;
            ViewBag.IsOK = 0;

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_GetPremiosPeriodoActivo_Result> tblPremios;
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    tblPremios = hbxEntities.CO_st_GetPremiosPeriodoActivo2().ToList();

                    if (tblPremios != null && tblPremios.Count != 0)
                    {
                        ViewBag.IsOK = 1;
                        return View(tblPremios.ToList());
                    }
                    else
                    {
                        ViewBag.IsOK = 0;
                        ViewBag.Message = "No se encontraron registros al calcular";
                        return View();
                    }

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
                };
            }

        }
        public ActionResult GenerarMovSocio()
        {
            List<CO_st_InsertaMovimientosSocio_Premio_Result> insertomovPremios;
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                //procedimiento que inserta en movimientos socio el movimiento de Premio por venta
                insertomovPremios = hbxEntities.CO_st_InsertaMovimientosSocio_Premio().ToList();

                if (Convert.ToInt32(insertomovPremios[0].numReg) > Convert.ToInt32(0))
                {
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }


            }
  
        }

        

        public ActionResult GenerarPedidos()
        {
            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                var NomUsuario = Session["IdNombre"].ToString();
                var Usuarios = hbxEntities.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                var varUsuario = Usuarios.IdUsuario;
                //var varEmpresa = "0"+Usuarios.CO_tb_Pais.Id_Pais.ToString();

                var insertoPedidos = hbxEntities.CO_st_GenerarPedidosPremios6(varUsuario);
                //var insertoPedidos = 1;

                if (insertoPedidos == -1)
                {
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }


            }
            //return Json(tblBonos, JsonRequestBehavior.AllowGet);

        }

    }
}
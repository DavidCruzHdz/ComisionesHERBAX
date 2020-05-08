namespace Comisiones.Controllers
{
    /*using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;*/

    using Comisiones.Models;
    using Comisiones.Models.ViewModels;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class CatPeriodosController : Controller
    {
        private string nameController = "CatPeriodosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        public ActionResult Index()
        {
            herbaxEntities db = new herbaxEntities();
            var VarRol = 0;
            if (Session["idRol"] != null)
            {
                VarRol = int.Parse(Session["idRol"].ToString());
            }
            else
            {
                return Redirect("~/Home/Login");
            }


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 25 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }


            return View();
        }


        public JsonResult ListarPeriodos()
        {
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscarPeriodos2_Result> listaPeridos = new List<CO_st_BuscarPeriodos2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //listaPeridos = hbxEntities.CO_st_InfoPeriodoActivo().ToList();
                    listaPeridos = hbxEntities.CO_st_BuscarPeriodos2().ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_BuscarPeriodos  DETALLE: " + ex);
                }


            }
            return Json(listaPeridos, JsonRequestBehavior.AllowGet);

            //return View();
        }

        [HttpPost]
        public JsonResult AddOrEdit(PeriodosViewModel modelPeriodos)
        {
            Log oLog = new Log(rutaArchivoLog);
            var mensaje_ = "Fail";
            //List<CO_st_BuscarPeriodos_Result> listaPeridos = new List<CO_st_BuscarPeriodos_Result>();

            oLog.Add(nameController, "  MENSAJE:  Entra al Metodo  AddOrEdit Accion: " + modelPeriodos.AccionSend + "__STATUS: " + modelPeriodos.estatus);

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {


                //2019-12-12
                Int32 anno = Convert.ToInt32(modelPeriodos.fechaInicio.Substring(0, 4));
                Int32 mes = Convert.ToInt32(modelPeriodos.fechaInicio.Substring(5, 2));
                Int32 dia = Convert.ToInt32(modelPeriodos.fechaInicio.Substring(8, 2));
                var fechaInicio = new DateTime(anno, mes, dia);

                Int32 annoF = Convert.ToInt32(modelPeriodos.fechaFin.Substring(0, 4));
                Int32 mesF = Convert.ToInt32(modelPeriodos.fechaFin.Substring(5, 2));
                Int32 diaF = Convert.ToInt32(modelPeriodos.fechaFin.Substring(8, 2));
                var fechaFin = new DateTime(annoF, mesF, diaF);

                if (modelPeriodos.AccionSend == "Edit")
                {
                    try
                    {
                        //llama al SP que edita el registrio de Acuerdo a los datos pasados 
                        var R = hbxEntities.CO_st_ActualizaPeriodos(
                            modelPeriodos.idPeriodo,
                            modelPeriodos.anio,
                            modelPeriodos.mes,
                            modelPeriodos.quincena,
                            fechaInicio,
                            fechaFin,
                            modelPeriodos.estatus,
                            "EvoComisiones"
                        );

                    }
                    catch (Exception ex)
                    {
                        oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_ActualizaPeriodos  DETALLE: " + ex);
                    }
                }
                else if (modelPeriodos.AccionSend == "Add")
                {
                    try
                    {
                        //llama al SP que edita el registrio de Acuerdo a los datos pasados
                        var R = hbxEntities.CO_st_InsertaPeriodos(
                            //modelPeriodos.idPeriodo,
                            modelPeriodos.anio,
                            modelPeriodos.mes,
                            modelPeriodos.quincena,
                            fechaInicio,
                            fechaFin,
                            modelPeriodos.estatus,
                            "EvoComisiones"
                        );

                    }
                    catch (Exception ex)
                    {
                        oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_ActualizaPeriodos  DETALLE: " + ex);
                    }
                }
                else
                {
                    // No hace nada e imprime en el Archvo  log
                    oLog.Add(nameController, "  MENSAJE:  No llego la  AccionSend");
                }


            }

            // Bloque Lista los periodo
            List<CO_st_BuscarPeriodos2_Result> listaPeridos = new List<CO_st_BuscarPeriodos2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //listaPeridos = hbxEntities.CO_st_InfoPeriodoActivo().ToList();
                    listaPeridos = hbxEntities.CO_st_BuscarPeriodos2().ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP CO_st_BuscarPeriodos  DETALLE: " + ex);
                }


            }
            //

            return Json(listaPeridos, JsonRequestBehavior.AllowGet);
            //return Json(mensaje_, JsonRequestBehavior.AllowGet);

            //return View();
        }

    }
}


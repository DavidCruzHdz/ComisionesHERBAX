using Comisiones.Data;
using Comisiones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.Entity;
using System.Net;

using Comisiones.Models.ModelsEvolution;


namespace Comisiones.Controllers
{
    public class CO_tb_ConfigReembolsosController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();
        private string nameController = "ReembolsosCanjeController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";

        // GET: AsignarReembolsos
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 33 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaConfigReembolsos2_Result> LReembolsos = new List<CO_st_BuscaConfigReembolsos2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    LReembolsos = hbxEntities.CO_st_BuscaConfigReembolsos2().ToList();
                    return View(LReembolsos.ToList());
                    
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


        // POST: AsignarReembolsos/Create
        [HttpPost]
        public ActionResult Valida()
        {
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaProductos4_Result> LProductos = new List<CO_st_BuscaProductos4_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   where o.Estatus == 1
                                   select new SelectListItem
                                   {
                                       Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                       Value = o.Id_Periodo.ToString(),
                                   };
                    ViewBag.dropdownPeriodos = Periodos.ToList();


                    List<SelectListItem> estatus = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Activo", Value = "1" },
                            new SelectListItem { Text = "Inactivo", Value = "0" },
                        };

                    ViewBag.IsOK = 0;

                    ViewBag.dropdownEstatus = estatus;

                    var idPais = 1;
                    LProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();

                    return View(LProductos.ToList());
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
                }



                return View(db.CO_tb_ConfigReembolsos.ToList());

            }
        }

        // GET: AsignarReembolsos/Details/5
        public ActionResult Details(int id)
        {
            //Log oLog = new Log(rutaArchivoLog);
            //List<CO_st_BuscaProductos3_Result> LProductos = new List<CO_st_BuscaProductos3_Result>();
            Log oLog = new Log(rutaArchivoLog);
            //List<CO_st_BuscaReembolsos2_Result> LReembolsos = new List<CO_st_BuscaReembolsos2_Result>();
            List<CO_st_BuscaProductosConUnId1_Result> LProductos = new List<CO_st_BuscaProductosConUnId1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    var Busqueda = db.CO_tb_ConfigReembolsos.Where(x => (x.Id_Reembolso == id)).FirstOrDefault();

                    ViewBag.Reembolsos = Busqueda;


                    if (Busqueda != null)
                    {
                        int IdPais = int.Parse(Busqueda.id_pais.ToString());

                        int AnioActual = DateTime.Now.Year;
                        int AnioInicial = AnioActual - 20;
                        List<SelectListItem> Anio = new List<SelectListItem>();

                        for (int i = AnioActual; i >= AnioInicial; i--)
                        {
                            Anio.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                        }


                        //ViewBag.dropdownAnio = Anio;


                        List<SelectListItem> Mes = new List<SelectListItem>()
                            {
                                new SelectListItem { Text = "Enero", Value = "1" },
                                new SelectListItem { Text = "Febrero", Value = "2" },
                                new SelectListItem { Text = "Marzo", Value = "3" },
                                new SelectListItem { Text = "Abril", Value = "4" },
                                new SelectListItem { Text = "Mayo", Value = "5" },
                                new SelectListItem { Text = "Junio", Value = "6" },
                                new SelectListItem { Text = "Julio", Value = "7" },
                                new SelectListItem { Text = "Agosto", Value = "8" },
                                new SelectListItem { Text = "Septiembre", Value = "9" },
                                new SelectListItem { Text = "Octubre", Value = "10" },
                                new SelectListItem { Text = "Noviembre", Value = "11" },
                                new SelectListItem { Text = "Diciembre", Value = "12" },
                            };

                        ViewBag.dropdownMes = Mes;

                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                        var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                       where o.Estatus == 1
                                       select new SelectListItem
                                       {
                                           Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                           Value = o.Id_Periodo.ToString(),
                                       };
                        //ViewBag.dropdownPeriodos = Periodos.ToList();

                        List<SelectListItem> estatus = new List<SelectListItem>()
                            {
                                new SelectListItem { Text = "Activo", Value = "1" },
                                new SelectListItem { Text = "Inactivo", Value = "0" },
                            };

                        ViewBag.IsOK = 0;
                        ViewBag.dropdownEstatus = estatus;
                        int Opcion = 1;
                        int Id = id;
                        int Rank = 0;

                        //ViewBag.dropdownAnio = new SelectList(db.CO_tb_Productos_Reembolso, "CPIdEmpresa", "CPDescripcionEmpresa", Busqueda.Anio);
                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais", Busqueda.id_pais);
                        ViewBag.dropdownAnio = new SelectList(Anio, "Value", "Text", Busqueda.Anio);
                        ViewBag.dropdownMes = new SelectList(Mes, "Value", "Text", Busqueda.Mes);
                        ViewBag.dropdownEstatus = new SelectList(estatus, "Value", "Text", Busqueda.id_estatus);

                        //ViewData["Cantidad"] = Busqueda.Cantidad;
                        //ViewData["CantTopada"] = Busqueda.Cantidad_Topada;                        
                        //ViewData["Porcentaje"] = Busqueda.PorcentajeReembolso;
                        //ViewBag.Estatus = Busqueda.id_estatus;

                        ViewBag.Cantidad = Busqueda.Cantidad;
                        ViewBag.CantTopada = Busqueda.Cantidad_Topada;
                        ViewBag.Porcentaje = Busqueda.PorcentajeReembolso;

                        LProductos = hbxEntities.CO_st_BuscaProductosConUnId5(IdPais, Id, Rank, Opcion).ToList();

                    }
                    return View(LProductos.ToList());
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


        // GET: AsignarReembolsos/Create
        public ActionResult Create(int? id_pais)
        {
            if (id_pais != null)
            {
                Log oLog = new Log(rutaArchivoLog);
                List<CO_st_BuscaProductos4_Result> LProductos = new List<CO_st_BuscaProductos4_Result>();

                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    try
                    {
                        int AnioActual = DateTime.Now.Year;
                        int AnioInicial = AnioActual - 20;
                        List<SelectListItem> Anio = new List<SelectListItem>();

                        for (int i = AnioActual; i >= AnioInicial; i--)
                        {
                            Anio.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                        }

                        ViewBag.dropdownAnio = Anio;

                        List<SelectListItem> Mes = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Enero", Value = "1" },
                            new SelectListItem { Text = "Febrero", Value = "2" },
                            new SelectListItem { Text = "Marzo", Value = "3" },
                            new SelectListItem { Text = "Abril", Value = "4" },
                            new SelectListItem { Text = "Mayo", Value = "5" },
                            new SelectListItem { Text = "Junio", Value = "6" },
                            new SelectListItem { Text = "Julio", Value = "7" },
                            new SelectListItem { Text = "Agosto", Value = "8" },
                            new SelectListItem { Text = "Septiembre", Value = "9" },
                            new SelectListItem { Text = "Octubre", Value = "10" },
                            new SelectListItem { Text = "Noviembre", Value = "11" },
                            new SelectListItem { Text = "Diciembre", Value = "12" },
                        };

                        ViewBag.dropdownMes = Mes;

                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                        var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                       where o.Estatus == 1
                                       select new SelectListItem
                                       {
                                           Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                           Value = o.Id_Periodo.ToString(),
                                       };
                        ViewBag.dropdownPeriodos = Periodos.ToList();


                        List<SelectListItem> estatus = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Activo", Value = "1" },
                            new SelectListItem { Text = "Inactivo", Value = "0" },
                        };

                        ViewBag.IsOK = 0;

                        ViewBag.dropdownEstatus = estatus;


                        var idPais = id_pais;
                        LProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();

                        return View(LProductos.ToList());
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
            else
            {
                Log oLog = new Log(rutaArchivoLog);
                List<CO_st_BuscaProductos4_Result> LProductos = new List<CO_st_BuscaProductos4_Result>();

                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    try
                    {
                        int AnioActual = DateTime.Now.Year;
                        int AnioInicial = AnioActual - 20;
                        List<SelectListItem> Anio = new List<SelectListItem>();

                        for (int i = AnioActual; i >= AnioInicial; i--)
                        {
                            Anio.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                        }

                        ViewBag.dropdownAnio = Anio;

                        List<SelectListItem> Mes = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Enero", Value = "1" },
                            new SelectListItem { Text = "Febrero", Value = "2" },
                            new SelectListItem { Text = "Marzo", Value = "3" },
                            new SelectListItem { Text = "Abril", Value = "4" },
                            new SelectListItem { Text = "Mayo", Value = "5" },
                            new SelectListItem { Text = "Junio", Value = "6" },
                            new SelectListItem { Text = "Julio", Value = "7" },
                            new SelectListItem { Text = "Agosto", Value = "8" },
                            new SelectListItem { Text = "Septiembre", Value = "9" },
                            new SelectListItem { Text = "Octubre", Value = "10" },
                            new SelectListItem { Text = "Noviembre", Value = "11" },
                            new SelectListItem { Text = "Diciembre", Value = "12" },
                        };

                        ViewBag.dropdownMes = Mes;

                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                        var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                       where o.Estatus == 1
                                       select new SelectListItem
                                       {
                                           Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                           Value = o.Id_Periodo.ToString(),
                                       };
                        ViewBag.dropdownPeriodos = Periodos.ToList();


                        List<SelectListItem> estatus = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Activo", Value = "1" },
                            new SelectListItem { Text = "Inactivo", Value = "0" },
                        };

                        ViewBag.IsOK = 0;

                        ViewBag.dropdownEstatus = estatus;

                        var idPais = 1;
                        LProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();

                        return View(LProductos.ToList());
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
        // POST: AsignarReembolsos/Create
        [HttpPost]
        public ActionResult Create(IList<CO_st_BuscaProductos4_Result> entity, IList<CO_st_BuscaProductos4_Result> entity2)
        {
            try
            {
                var NomUsuario = Session["IdNombre"].ToString();
                var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                var varUsuario = Usuarios.IdUsuario;

                var Pais = 0;
                var Periodo = 0;
                decimal Porcentaje = 0;
                decimal Topada = 0;
                decimal Cantidad = 0;


                CO_tb_ConfigReembolsos Reembolsos = new CO_tb_ConfigReembolsos();

                //var PeriodosEvo = Evo.CO_tb_Periodo.Where(x => x.Id_Periodo == Periodo).FirstOrDefault();
                var Anio = 0;
                var Mes = 0;

                foreach (var item in entity) /// aqui es
                {
                    Periodo = item.Id_Periodo;
                    Porcentaje = item.Porcentaje;
                    Cantidad = item.Cantidad;
                    Pais = int.Parse(item.Id_Empresa.ToString());
                    Topada = item.Topada;
                    Anio = item.Anio;
                    Mes = item.Mes;
                }

                Reembolsos.Anio = Anio;
                Reembolsos.Mes = Mes;
                Reembolsos.Cantidad = Cantidad;
                Reembolsos.Cantidad_Topada = Topada;
                Reembolsos.PorcentajeReembolso = Porcentaje;
                Reembolsos.id_concepto = 105;
                Reembolsos.id_estatus = 1;
                Reembolsos.id_pais = Pais;
                Reembolsos.UltMod_Usuario = NomUsuario;
                Reembolsos.UltMod_F = System.DateTime.Now;
                db.CO_tb_ConfigReembolsos.Add(Reembolsos);
                db.SaveChanges();

                var varReembolsos = db.CO_tb_ConfigReembolsos.Where(x => x.id_pais == Pais && x.Anio == Anio && x.Mes == Mes && x.Cantidad == Cantidad && x.Cantidad_Topada == Topada).FirstOrDefault();
                var IdReembolsos = varReembolsos.Id_Reembolso;

                var NumPartida = 0;
                var listPartidas = new List<CO_tb_Productos_Reembolso>();
                foreach (var item in entity) /// aqui es
                {                    
                    if (item.checkeado == true)
                    {
                        CO_tb_Productos_Reembolso Partidas = new CO_tb_Productos_Reembolso();

                        Periodo = item.Id_Periodo;
                        Porcentaje = item.Porcentaje;
                        Cantidad = item.Cantidad;
                        Pais = int.Parse(item.Id_Empresa.ToString());
                        Topada = item.Topada;

                        NumPartida = NumPartida + 1;
                        Partidas.Id_Reembolso = IdReembolsos;
                        Partidas.sku = item.sku;
                        Partidas.warehouse = item.Warehouse;
                        Partidas.Id_Empresa = item.Id_Empresa; // este es el problema
                        Partidas.Warehouse_ID = item.Warehouse_ID;
                        Partidas.cve_art = item.cve_art;
                        Partidas.Descripcion = "";
                        Partidas.DescripcionCorta = "";
                        Partidas.PrecioCosto = 0;
                        Partidas.PrecioPublico = item.PrecioPublico;
                        Partidas.PrecioCosto = 0;
                        Partidas.PrecioSocio = 0;
                        Partidas.PrecioMinimo = 0;
                        Partidas.ActivoSAE = "";
                        Partidas.Id_Socio = "";
                        Partidas.SubTotal = 0;
                        Partidas.Id_Periodo = item.Id_Periodo;
                        Partidas.UsuarioAlta = varUsuario;
                        Partidas.FechaAlta = System.DateTime.Now;
                        Partidas.UsuarioCambio = varUsuario;
                        Partidas.FechaCambio = System.DateTime.Now;
                        Partidas.Estatus = true;

                        listPartidas.Add(Partidas);
                    }

                }
                db.CO_tb_Productos_Reembolso.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                db.SaveChanges();


                return RedirectToAction("Index");
                
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }
        }

        // GET: AsignarReembolsos/Edit/5
        public ActionResult Edit(int id)
        {
            Log oLog = new Log(rutaArchivoLog);
            try
            {

                var Busqueda = db.CO_tb_ConfigReembolsos.Where(x => (x.Id_Reembolso == id)).FirstOrDefault();

                ViewBag.Reembolsos = Busqueda;


                if (Busqueda != null)
                {
                    int IdPais = int.Parse(Busqueda.id_pais.ToString());

                    int AnioActual = DateTime.Now.Year;
                    int AnioInicial = AnioActual - 20;
                    List<SelectListItem> Anio = new List<SelectListItem>();

                    for (int i = AnioActual; i >= AnioInicial; i--)
                    {
                        Anio.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }


                    List<SelectListItem> Mes = new List<SelectListItem>()
                            {
                                new SelectListItem { Text = "Enero", Value = "1" },
                                new SelectListItem { Text = "Febrero", Value = "2" },
                                new SelectListItem { Text = "Marzo", Value = "3" },
                                new SelectListItem { Text = "Abril", Value = "4" },
                                new SelectListItem { Text = "Mayo", Value = "5" },
                                new SelectListItem { Text = "Junio", Value = "6" },
                                new SelectListItem { Text = "Julio", Value = "7" },
                                new SelectListItem { Text = "Agosto", Value = "8" },
                                new SelectListItem { Text = "Septiembre", Value = "9" },
                                new SelectListItem { Text = "Octubre", Value = "10" },
                                new SelectListItem { Text = "Noviembre", Value = "11" },
                                new SelectListItem { Text = "Diciembre", Value = "12" },
                            };

                    ViewBag.dropdownMes = Mes;

                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   where o.Estatus == 1
                                   select new SelectListItem
                                   {
                                       Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                       Value = o.Id_Periodo.ToString(),
                                   };

                    List<SelectListItem> estatus = new List<SelectListItem>()
                            {
                                new SelectListItem { Text = "Activo", Value = "1" },
                                new SelectListItem { Text = "Inactivo", Value = "0" },
                            };

                    ViewBag.IsOK = 0;
                    ViewBag.dropdownEstatus = estatus;
                    int Opcion = 2;
                    int Id = id;

                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais", Busqueda.id_pais);
                    ViewBag.dropdownAnio = new SelectList(Anio, "Value", "Text", Busqueda.Anio);
                    ViewBag.dropdownMes = new SelectList(Mes, "Value", "Text", Busqueda.Mes);
                    ViewBag.dropdownEstatus = new SelectList(estatus, "Value", "Text", Busqueda.id_estatus);
                    ViewBag.Cantidad = Busqueda.Cantidad;
                    ViewBag.Cantidad_Topada = Busqueda.Cantidad_Topada;
                    ViewBag.PorcentajeReembolso = Busqueda.PorcentajeReembolso;
                    return View(db.CO_tb_ConfigReembolsos.Where(x => (x.Id_Reembolso == id)).FirstOrDefault());
                }

                return View();
            }
            catch (Exception ex)
            {
                var Col = 0;
                return View();
            }
        }
    

        // POST: AsignarReembolsos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, int id_pais, int Anio, int Mes, decimal Cantidad, decimal CantTopada, decimal Porcentaje)
        {
            try
            {
                var ConfigReembolsos = db.CO_tb_ConfigReembolsos.Where(x => (x.Id_Reembolso == id)).FirstOrDefault();

                if (ConfigReembolsos != null)
                {
                    ConfigReembolsos.Id_Reembolso = id;
                    ConfigReembolsos.Anio = Anio;                    
                    ConfigReembolsos.Mes = Mes;
                    ConfigReembolsos.Cantidad = Cantidad;
                    ConfigReembolsos.Cantidad_Topada = CantTopada;
                    //ConfigReembolsos.PorcentajeReembolso = PorcentajeReembolso;
                    //ConfigReembolsos.id_concepto = cO_tb_ConfigReembolsos.id_concepto;
                    //ConfigReembolsos.id_estatus = cO_tb_ConfigReembolsos.id_estatus;
                    ConfigReembolsos.id_pais = id_pais;

                    var NomUsuario = Session["IdNombre"].ToString();
                    ConfigReembolsos.UltMod_Usuario = NomUsuario;
                    ConfigReembolsos.UltMod_F = System.DateTime.Now;

                    db.Entry(ConfigReembolsos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


                // TODO: Add delete logic here

                //return RedirectToAction("Index");
                return View();
            }
            catch
            {
                return View();
            }

        }

        // GET: AsignarReembolsos/Delete/5
        public ActionResult Delete(int id)
        {
            //Log oLog = new Log(rutaArchivoLog);
            //List<CO_st_BuscaProductos3_Result> LProductos = new List<CO_st_BuscaProductos3_Result>();
            Log oLog = new Log(rutaArchivoLog);
            //List<CO_st_BuscaReembolsos2_Result> LReembolsos = new List<CO_st_BuscaReembolsos2_Result>();
            List<CO_st_BuscaProductosConUnId1_Result> LProductos = new List<CO_st_BuscaProductosConUnId1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    var Busqueda = db.CO_tb_ConfigReembolsos.Where(x => (x.Id_Reembolso == id)).FirstOrDefault();
                    //ViewBag.Reembolsos = Busqueda;


                    if (Busqueda != null)
                    {
                        int IdPais = int.Parse(Busqueda.id_pais.ToString());

                        int AnioActual = DateTime.Now.Year;
                        int AnioInicial = AnioActual - 20;
                        List<SelectListItem> Anio = new List<SelectListItem>();

                        for (int i = AnioActual; i >= AnioInicial; i--)
                        {
                            Anio.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                        }


                        //ViewBag.dropdownAnio = Anio;


                        List<SelectListItem> Mes = new List<SelectListItem>()
                            {
                                new SelectListItem { Text = "Enero", Value = "1" },
                                new SelectListItem { Text = "Febrero", Value = "2" },
                                new SelectListItem { Text = "Marzo", Value = "3" },
                                new SelectListItem { Text = "Abril", Value = "4" },
                                new SelectListItem { Text = "Mayo", Value = "5" },
                                new SelectListItem { Text = "Junio", Value = "6" },
                                new SelectListItem { Text = "Julio", Value = "7" },
                                new SelectListItem { Text = "Agosto", Value = "8" },
                                new SelectListItem { Text = "Septiembre", Value = "9" },
                                new SelectListItem { Text = "Octubre", Value = "10" },
                                new SelectListItem { Text = "Noviembre", Value = "11" },
                                new SelectListItem { Text = "Diciembre", Value = "12" },
                            };

                        ViewBag.dropdownMes = Mes;

                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                        var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                       where o.Estatus == 1
                                       select new SelectListItem
                                       {
                                           Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                           Value = o.Id_Periodo.ToString(),
                                       };
                        //ViewBag.dropdownPeriodos = Periodos.ToList();

                        List<SelectListItem> estatus = new List<SelectListItem>()
                            {
                                new SelectListItem { Text = "Activo", Value = "1" },
                                new SelectListItem { Text = "Inactivo", Value = "0" },
                            };

                        ViewBag.IsOK = 0;
                        ViewBag.dropdownEstatus = estatus;
                        int Opcion = 1;
                        int Id = id;


                        //ViewBag.dropdownAnio = new SelectList(db.CO_tb_Productos_Reembolso, "CPIdEmpresa", "CPDescripcionEmpresa", Busqueda.Anio);
                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais", Busqueda.id_pais);
                        ViewBag.dropdownAnio = new SelectList(Anio, "Value", "Text", Busqueda.Anio);
                        ViewBag.dropdownMes = new SelectList(Mes, "Value", "Text", Busqueda.Mes);
                        ViewBag.dropdownEstatus = new SelectList(estatus, "Value", "Text", Busqueda.id_estatus);

                        //ViewData["Cantidad"] = Busqueda.Cantidad;
                        //ViewData["CantTopada"] = Busqueda.Cantidad_Topada;                        
                        //ViewData["Porcentaje"] = Busqueda.PorcentajeReembolso;
                        //ViewBag.Estatus = Busqueda.id_estatus;
                        ViewBag.Cantidad = Busqueda.Cantidad;
                        ViewBag.CantTopada = Busqueda.Cantidad_Topada;
                        ViewBag.Porcentaje = Busqueda.PorcentajeReembolso;

                        int Rank = 0; // esto es porque el store trae datos para premio y reembolsos
                        LProductos = hbxEntities.CO_st_BuscaProductosConUnId5(IdPais, Id, Rank, Opcion).ToList();
                    }
                    return View(LProductos.ToList());
                }
                catch (Exception ex)
                {
                    ViewBag.IsOK = 1;
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

        // POST: AsignarReembolsos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var cO_tb_ConfigReembolsos = db.CO_tb_ConfigReembolsos.Where(x => (x.Id_Reembolso == id)).FirstOrDefault();

                if (cO_tb_ConfigReembolsos != null)
                {
                    Log oLog = new Log(rutaArchivoLog);
                    //List<CO_st_BuscaProductos3_Result> LReembolsos = new List<CO_st_BuscaProductos3_Result>();

                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        try
                        {
                            var IdReembolso = id;
                            //Obtiene las comisiones atrasadas
                            var Borrar = hbxEntities.CO_st_BorrarProductosReembolso(IdReembolso);
                        }
                        catch (Exception ex)
                        {
                            oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaProductos4  DETALLE: " + ex);
                        }
                    }
                }


                // TODO: Add delete logic here

                 return RedirectToAction("Index");
                //return View();
            }
            catch
            {
                return View();
            }
        }



        public JsonResult BuscaProductos(int IdPais)
        {
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaProductos4_Result> LReembolsos = new List<CO_st_BuscaProductos4_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Obtiene las comisiones atrasadas
                    LReembolsos = hbxEntities.CO_st_BuscaProductos4(IdPais).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaProductos4  DETALLE: " + ex);
                }
            }
            return Json(LReembolsos, JsonRequestBehavior.AllowGet);
        }


        // GET: CO_tb_Rango_Compra_Premios/Productos/5
        public ActionResult ProdReembolso(int? Id_Reembolso, int? id_pais, int? Anio, int? Mes, decimal? Cantidad, decimal? Cantidad_Topada, decimal? PorcentajeReembolso)
        {
            if (Id_Reembolso == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_ConfigReembolsos cO_tb_ConfigReembolsos = db.CO_tb_ConfigReembolsos.Find(Id_Reembolso);
            if (cO_tb_ConfigReembolsos == null)
            {
                return HttpNotFound();
            }


            ViewBag.Premios = db.CO_tb_ConfigReembolsos.Where(x => x.Id_Reembolso == Id_Reembolso).ToList();

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaProductosConUnId1_Result> LProductos = new List<CO_st_BuscaProductosConUnId1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    var Opcion = 5;
                    var IdPais = id_pais;
                    int Id = int.Parse(Id_Reembolso.ToString());

                    ViewBag.id_Reembolso = Id_Reembolso;
                    ViewBag.id_pais = id_pais;
                    ViewBag.Anio = Anio;
                    ViewBag.Mes = Mes;
                    ViewBag.Cantidad = Cantidad;
                    ViewBag.Topada = Cantidad_Topada;
                    ViewBag.Porcentaje = PorcentajeReembolso;
                    //ViewBag.Estatus = Estatus;

                    int Rank = 0;
                    LProductos = hbxEntities.CO_st_BuscaProductosConUnId5(IdPais, Id, Rank, Opcion).ToList();
                    return View(LProductos.ToList());
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


        //[ValidateAntiForgeryToken]
        [HttpPost, ActionName("ProdReembolso")]
        public ActionResult ProdReembolso(IList<CO_st_BuscaProductos4_Result> entity)
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 23 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            
            var NomUsuario = Session["IdNombre"].ToString();            

            if (entity != null)
            {
                foreach (var item in entity) /// aqui es
                {
                    //if (item.checkeado == true)
                    //{

                        var Empresa = item.Id_Pais.ToString();
                        CO_tb_Productos_Reembolso cO_tb_Productos_Reembolso = db.CO_tb_Productos_Reembolso.Where(x => x.Id_Reembolso == item.id_premio && x.sku == item.sku).FirstOrDefault();
                        if (cO_tb_Productos_Reembolso != null)
                        {
                            db.CO_tb_Productos_Reembolso.Remove(cO_tb_Productos_Reembolso);
                            db.SaveChanges();
                        }

                    var cO_tb_ConfigReembolsos = db.CO_tb_ConfigReembolsos.Where(x => x.Id_Reembolso == item.id_premio).FirstOrDefault();
                    if (cO_tb_ConfigReembolsos != null)
                        {
                            cO_tb_ConfigReembolsos.UltMod_Usuario = NomUsuario;
                            cO_tb_ConfigReembolsos.UltMod_F = System.DateTime.Now;
                            cO_tb_ConfigReembolsos.id_pais = item.Id_Pais;
                            cO_tb_ConfigReembolsos.Cantidad = item.Cantidad;
                            cO_tb_ConfigReembolsos.Cantidad_Topada = item.Topada;
                            cO_tb_ConfigReembolsos.PorcentajeReembolso = item.Porcentaje;
                            cO_tb_ConfigReembolsos.id_estatus = item.Estatus;
                            db.Entry(cO_tb_ConfigReembolsos).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    //}
                }

                var listPartidas = new List<CO_tb_Productos_Reembolso>();
                foreach (var item in entity) /// aqui es
                {
                    
                    if (item.checkeado == true)
                    {
                        var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
      
                        var Empresa = "0" + item.Id_Pais.ToString();
                        CO_tb_Productos_Reembolso Partidas = new CO_tb_Productos_Reembolso();
                        Partidas.Id_Empresa = Empresa;
                        Partidas.Id_Reembolso = item.id_premio;
                        Partidas.sku = item.sku;
                        Partidas.UsuarioCambio = Usuarios.IdUsuario;
                        Partidas.FechaCambio = DateTime.Now;
                        Partidas.UsuarioAlta = Usuarios.IdUsuario;
                        Partidas.FechaAlta = DateTime.Now;
                        Partidas.Estatus = true;
                        listPartidas.Add(Partidas);
                    }

                }
                db.CO_tb_Productos_Reembolso.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }
}


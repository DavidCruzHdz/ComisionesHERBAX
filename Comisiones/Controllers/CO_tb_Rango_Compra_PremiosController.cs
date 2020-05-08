using Comisiones.Models;
using Comisiones.Models.ModelsEvolution;
using Comisiones.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using Comisiones.Data;
using System.Data;
using SpreadsheetLight;
using System.IO;

namespace Comisiones.Controllers
{
    public class CO_tb_Rango_Compra_PremiosController : Controller
    {
        private string nameController = "CO_tb_Rango_Compra_PremiosController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();

        // GET: CO_tb_Rango_Compra_Premios
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


            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 34 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            //var result = from a in db.CO_tb_Rango_Compra_Premios
            //             join p in Evo.CO_tb_Periodo on a.id_periodo equals p.Id_Periodo
            //             join c in Evo.CL_tb_Pais on a.id_pais equals c.Id_Pais
            //             join r in Evo.CO_tb_Rank on a.id_rank equals r.Rank_ID
            //             join e in db.CO_tb_Premios on a.id_premio equals e.id_premio
            //                where a.Estatus == true
            //             select new Rango_Compra_PremiosViewModel()
            //                {
            //                 DescripcionPais = c.NombreCorto,
            //                 DescripcionPremio = e.nombre_premio,
            //                 DescripcionRank = r.Rank,
            //                 id_periodo = p.Id_Periodo,
            //                 puntos = decimal.Parse(a.puntos.ToString()),
            //                 Estatus = a.Estatus,
            //                 UltModUsuario = a.UltModUsuario
            //                    UltMod_F = a.UltMod_F,
            //             };

            //var Menu = listaMenu.OrderBy(x => x.Estatus == true).ToList();
            //return View(result.ToList());


            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaRango_Compra_Premios1_Result> LProductos = new List<CO_st_BuscaRango_Compra_Premios1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    LProductos = hbxEntities.CO_st_BuscaRango_Compra_Premios1().ToList();
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

        // GET: CO_tb_Rango_Compra_Premios/Details/5
        public ActionResult Details(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Rango_Compra_Premios cO_tb_Rango_Compra_Premios = db.CO_tb_Rango_Compra_Premios.Find(id);
            if (cO_tb_Rango_Compra_Premios == null)
            {
                return HttpNotFound();
            }


            if (cO_tb_Rango_Compra_Premios.Estatus == true)
            {
                List<SelectListItem> estatus = new List<SelectListItem>()
                {
                new SelectListItem { Text = "Activo", Value = "1" },
                };
                ViewBag.dropdownEstatus = estatus;

            }
            else
            {
                List<SelectListItem> estatus = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Inactivo", Value = "0" },
                };
                ViewBag.dropdownEstatus = estatus;

            };



            //var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
            //                //where o.tipo == "P" && o.estatus == 1
            //            select new SelectListItem
            //            {
            //                Text = o.type_id.ToString(),
            //                Value = o.Description.ToString(),
            //            };

            //ViewBag.dropdownRanks = Ranks.ToList();




            var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                            //where o.tipo == "P" && o.estatus == 1
                        select new SelectListItem
                        {
                            Text = o.Description.ToString(),
                            Value = o.type_id.ToString(),                            
                        };

            ViewBag.dropdownRanks = Ranks.ToList();

            var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                           where o.Estatus == 1
                           select new SelectListItem
                           {
                               Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                               Value = o.Id_Periodo.ToString(),
                           };

            //ViewBag.dropdownPeriodos = Periodos.ToList();
            
            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais", cO_tb_Rango_Compra_Premios.id_pais);
            ViewBag.dropdownPeriodos = new SelectList(Evo.CO_tb_Periodo.ToList(), "id_periodo", "id_periodo", cO_tb_Rango_Compra_Premios.id_periodo);
            ViewBag.dropdownPremios = new SelectList(db.CO_tb_Premios.ToList().Where(x => x.Pagado == false), "id_premio", "nombre_premio", cO_tb_Rango_Compra_Premios.id_premio);
            return View(cO_tb_Rango_Compra_Premios);
        }


        // GET: CO_tb_Rango_Compra_Premios/Create
        public ActionResult Create(int? id_premio)
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

            if (id_premio != null)
            {
                var Busqueda = db.CO_tb_Premios.Where(x => (x.id_premio == id_premio)).FirstOrDefault();

                if (Busqueda != null)
                {
                    //var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                    //                //where o.tipo == "P" && o.estatus == 1
                    //            select new SelectListItem
                    //            {
                    //                Text = o.type_id.ToString(),
                    //                Value = o.Description.ToString(),
                    //            };

                    //ViewBag.dropdownRanks = Ranks.ToList();



                    var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                                    //where o.tipo == "P" && o.estatus == 1
                                select new SelectListItem
                                {
                                    Text = o.Description.ToString(),
                                    Value = o.type_id.ToString(),
                                };

                    ViewBag.dropdownRanks = Ranks.ToList();


                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   //where o.Estatus == 0
                                   select new SelectListItem
                                   {
                                       Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                       Value = o.Id_Periodo.ToString(),
                                   };

                    //var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                    //               where o.Id_Periodo == Busqueda.id_periodo
                    //               select new SelectListItem
                    //               {
                    //                   Text = o.Id_Periodo.ToString(),
                    //                   Value = o.Id_Periodo.ToString(),
                    //               };

                    //ViewBag.dropdownPeriodos = Periodos.ToList();
                    ViewBag.dropdownPeriodos = Periodos.ToList();
                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1 && x.Id_Pais == Busqueda.id_pais), "Id_Pais", "Pais");
                    ViewBag.dropdownPremios = new SelectList(db.CO_tb_Premios.ToList().Where(x => x.Estatus == true && x.Pagado == false), "id_premio", "nombre_premio");


                    List<CO_st_BuscaRango_Compra_Premios1_Result> LProductos = new List<CO_st_BuscaRango_Compra_Premios1_Result>();
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        try
                        {
                            LProductos = hbxEntities.CO_st_BuscaRango_Compra_Premios1().ToList();
                            ViewBag.RanksDePremio = LProductos.Where(x => x.DescripcionPremio == "0").ToList();

                            var idPais = Busqueda.id_pais;
                            var SelecionProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();

                            //return View(SelecionProductos.Where(x => x.checkeado = true).ToList());
                            return View(SelecionProductos.ToList());
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
            else
            {

                //var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                //                //where o.tipo == "P" && o.estatus == 1
                //            select new SelectListItem
                //            {
                //                Text = o.type_id.ToString(),
                //                Value = o.Description.ToString(),
                //            };

                //ViewBag.dropdownRanks = Ranks.ToList();


                var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                                //where o.tipo == "P" && o.estatus == 1
                            select new SelectListItem
                            {
                                Text = o.Description.ToString(),
                                Value = o.type_id.ToString(),
                            };

                ViewBag.dropdownRanks = Ranks.ToList();


                var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                               //where o.Estatus == 1
                               select new SelectListItem
                               {
                                   Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                   Value = o.Id_Periodo.ToString(),
                               };

                ViewBag.dropdownPeriodos = Periodos.ToList();

                //var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                //               where o.Estatus == 1
                //               select new SelectListItem
                //               {
                //                   Text = o.Id_Periodo.ToString(),
                //                   Value = o.Id_Periodo.ToString(),
                //               };

                ViewBag.dropdownPeriodos = Periodos.ToList();
                ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");
                ViewBag.dropdownPremios = new SelectList(db.CO_tb_Premios.ToList().Where(x => x.Estatus == true && x.Pagado == false), "id_premio", "nombre_premio");
                
                List<CO_st_BuscaRango_Compra_Premios1_Result> LProductos = new List<CO_st_BuscaRango_Compra_Premios1_Result>();
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    try
                    {
                        LProductos = hbxEntities.CO_st_BuscaRango_Compra_Premios1().ToList();
                        ViewBag.RanksDePremio = LProductos.Where(x => x.DescripcionPremio == "0").ToList();

                        var idPais = 0;
                        var SelecionProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();

                        return View(SelecionProductos.ToList());

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


            return View();
        }

        // POST: CO_tb_Rango_Compra_Premios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(int? id_premio, string nombre_premio, int? id_pais, int? id_periodo, IList<CO_st_BuscaProductos4_Result> entity)
        {


            if (id_premio != null)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                var varUsuario = Usuarios.IdUsuario;

                CO_tb_Rango_Compra_Premios Rango_Compra = new CO_tb_Rango_Compra_Premios();

                foreach (var item in entity) /// aqui es
                {
                    Rango_Compra.CreadoUsuario = NomUsuario;
                    Rango_Compra.Creado_F = DateTime.Now;
                    Rango_Compra.UltModUsuario = NomUsuario;
                    Rango_Compra.UltMod_F = DateTime.Now;
                    Rango_Compra.Estatus = true;
                    Rango_Compra.id_pais = item.Id_Pais;
                    Rango_Compra.id_periodo = item.Id_Periodo;
                    Rango_Compra.id_rank = item.Rank_ID;
                    Rango_Compra.id_premio = item.id_premio;
                    Rango_Compra.puntos = item.Puntos;
                }
                db.CO_tb_Rango_Compra_Premios.Add(Rango_Compra);
                db.SaveChanges();



                var listPartidas = new List<CO_tb_Productos_Premios>();
                foreach (var item in entity) /// aqui es
                {
                    if (item.checkeado == true)
                    {
                        CO_tb_Productos_Premios Partidas = new CO_tb_Productos_Premios();

                        Partidas.id_pais = item.Id_Pais;
                        Partidas.id_premio = item.id_premio;
                        Partidas.id_rank = item.Rank_ID;
                        Partidas.id_producto = item.sku;

                        Partidas.CreadoUsuario = NomUsuario;
                        Partidas.Creado_F = DateTime.Now;
                        Partidas.UltModUsuario = NomUsuario;
                        Partidas.UltMod_F = DateTime.Now;
                        Partidas.Estatus = true;
                        listPartidas.Add(Partidas);
                    }

                }
                db.CO_tb_Productos_Premios.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                db.SaveChanges();


                //var Ranks = from o in Evo.CO_tb_Rank.ToList()
                //                //where o.tipo == "P" && o.estatus == 1
                //            select new SelectListItem
                //            {
                //                Text = o.Rank,
                //                Value = o.Rank_ID.ToString(),
                //            };

                //ViewBag.dropdownRanks = Ranks.ToList();



                var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                                //where o.tipo == "P" && o.estatus == 1
                            select new SelectListItem
                            {
                                Text = o.Description.ToString(),
                                Value = o.type_id.ToString(),
                            };

                ViewBag.dropdownRanks = Ranks.ToList();

                var Busca = db.CO_tb_Premios.OrderByDescending(x => x.id_premio).FirstOrDefault();
                if (Busca != null)
                {
                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   where o.Id_Periodo == Busca.id_periodo
                                   select new SelectListItem
                                   {
                                       Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                       Value = o.Id_Periodo.ToString(),
                                   };



                    //var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                    //               where o.Id_Periodo == Busca.id_periodo
                    //               select new SelectListItem
                    //               {
                    //                   Text = o.Id_Periodo.ToString(),
                    //                   Value = o.Id_Periodo.ToString(),
                    //               };

                    ViewBag.id_premio = Busca.id_premio;
                    ViewBag.nombre_premio = Busca.nombre_premio;
                    ViewBag.id_periodo = Busca.id_periodo;
                    ViewBag.id_pais = Busca.id_pais;

                    ViewBag.dropdownPeriodos = Periodos.ToList();
                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Pais == Busca.id_pais), "Id_Pais", "Pais");
                }

               
                List<CO_st_BuscaRango_Compra_Premios1_Result> LProductos = new List<CO_st_BuscaRango_Compra_Premios1_Result>();
                using (herbaxEntities hbxEntities = new herbaxEntities())
                {
                    try
                    {
                        LProductos = hbxEntities.CO_st_BuscaRango_Compra_Premios1().ToList();
                        ViewBag.RanksDePremio = LProductos.Where(x=> x.DescripcionPremio == Busca.nombre_premio).ToList();

                        var idPais = Busca.id_pais;
                        var SelecionProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();

                        return View(SelecionProductos.ToList());
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
                if (entity != null)
                {
                    var NomUsuario = Session["IdNombre"].ToString();
                    var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                    var varUsuario = Usuarios.IdUsuario;

                    CO_tb_Rango_Compra_Premios Rango_Compra = new CO_tb_Rango_Compra_Premios();

                    foreach (var item in entity) /// aqui es
                    {
                        Rango_Compra.CreadoUsuario = NomUsuario;
                        Rango_Compra.Creado_F = DateTime.Now;
                        Rango_Compra.UltModUsuario = NomUsuario;
                        Rango_Compra.UltMod_F = DateTime.Now;
                        Rango_Compra.Estatus = true;
                        Rango_Compra.id_pais = item.Id_Pais;
                        Rango_Compra.id_periodo = item.Id_Periodo;
                        Rango_Compra.id_rank = item.Rank_ID;
                        Rango_Compra.id_premio = item.id_premio;
                        Rango_Compra.puntos = item.Puntos;
                    }
                    db.CO_tb_Rango_Compra_Premios.Add(Rango_Compra);
                    db.SaveChanges();



                    var listPartidas = new List<CO_tb_Productos_Premios>();
                    foreach (var item in entity) /// aqui es
                    {
                        if (item.checkeado == true)
                        {
                            CO_tb_Productos_Premios Partidas = new CO_tb_Productos_Premios();

                            Partidas.id_pais = item.Id_Pais;
                            Partidas.id_premio = item.id_premio;
                            Partidas.id_rank = item.Rank_ID;
                            Partidas.id_producto = item.sku;

                            Partidas.CreadoUsuario = NomUsuario;
                            Partidas.Creado_F = DateTime.Now;
                            Partidas.UltModUsuario = NomUsuario;
                            Partidas.UltMod_F = DateTime.Now;
                            Partidas.Estatus = true;
                            listPartidas.Add(Partidas);
                        }

                    }
                    db.CO_tb_Productos_Premios.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                    db.SaveChanges();





                    var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                                    //where o.tipo == "P" && o.estatus == 1
                                select new SelectListItem
                                {
                                    Text = o.Description.ToString(),
                                    Value = o.type_id.ToString(),
                                };

                    ViewBag.dropdownRanks = Ranks.ToList();

                    var Busca = db.CO_tb_Premios.OrderByDescending(x => x.id_premio).FirstOrDefault();
                    if (Busca != null)
                    {
                        var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                       where o.Id_Periodo == Busca.id_periodo
                                       select new SelectListItem
                                       {
                                           Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                           Value = o.Id_Periodo.ToString(),
                                       };


                        //var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                        //               where o.Id_Periodo == Busca.id_periodo
                        //               select new SelectListItem
                        //               {
                        //                   Text = o.Id_Periodo.ToString(),
                        //                   Value = o.Id_Periodo.ToString(),
                        //               };

                        ViewBag.id_premio = Busca.id_premio;
                        ViewBag.nombre_premio = Busca.nombre_premio;
                        ViewBag.id_periodo = Busca.id_periodo;
                        ViewBag.id_pais = Busca.id_pais;

                        ViewBag.dropdownPeriodos = Periodos.ToList();
                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Pais == Busca.id_pais), "Id_Pais", "Pais");
                    }


                    List<CO_st_BuscaRango_Compra_Premios1_Result> LProductos = new List<CO_st_BuscaRango_Compra_Premios1_Result>();
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        try
                        {
                            LProductos = hbxEntities.CO_st_BuscaRango_Compra_Premios1().ToList();
                            ViewBag.RanksDePremio = LProductos.Where(x => x.DescripcionPremio == Busca.nombre_premio).ToList();
                            var idPais = Busca.id_pais;
                            var SelecionProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();
                            return View(SelecionProductos.ToList());
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
                    CO_tb_Premios Premios = new CO_tb_Premios();

                    Premios.nombre_premio = nombre_premio;
                    Premios.id_pais = id_pais;
                    Premios.id_periodo = id_periodo;
                    Premios.Estatus = true;
                    Premios.Pagado = false;

                    db.CO_tb_Premios.Add(Premios);
                    db.SaveChanges();


                    var Busca = db.CO_tb_Premios.OrderByDescending(x => x.id_premio).FirstOrDefault();
                    if (Busca != null)
                    {
                        ViewBag.id_premio = Busca.id_premio;
                    }


                    //var Ranks = from o in Evo.CO_tb_Rank.ToList()
                    //                //where o.tipo == "P" && o.estatus == 1
                    //            select new SelectListItem
                    //            {
                    //                Text = o.Rank,
                    //                Value = o.Rank_ID.ToString(),
                    //            };

                    //ViewBag.dropdownRanks = Ranks.ToList();



                    var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                                    //where o.tipo == "P" && o.estatus == 1
                                select new SelectListItem
                                {
                                    Text = o.Description.ToString(),
                                    Value = o.type_id.ToString(),
                                };

                    ViewBag.dropdownRanks = Ranks.ToList();

                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   where o.Id_Periodo == Busca.id_periodo
                                   select new SelectListItem
                                   {
                                       Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                       Value = o.Id_Periodo.ToString(),
                                   };


                    //var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                    //               where o.Id_Periodo == id_periodo
                    //               select new SelectListItem
                    //               {
                    //                   Text = o.Id_Periodo.ToString(),
                    //                   Value = o.Id_Periodo.ToString(),
                    //               };

                    ViewBag.dropdownPeriodos = Periodos.ToList();
                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1 && x.Id_Pais == id_pais), "Id_Pais", "Pais");
                    //ViewBag.dropdownPremios = new SelectList(db.CO_tb_Premios.ToList().Where(x => x.Estatus == true && x.Pagado == false), "Id_Premio", "nombre_premio");


                    List<CO_st_BuscaRango_Compra_Premios1_Result> LProductos = new List<CO_st_BuscaRango_Compra_Premios1_Result>();
                    using (herbaxEntities hbxEntities = new herbaxEntities())
                    {
                        try
                        {
                            LProductos = hbxEntities.CO_st_BuscaRango_Compra_Premios1().ToList();
                            ViewBag.RanksDePremio = LProductos.Where(x => x.DescripcionPremio == Busca.nombre_premio).ToList();
                            ViewBag.nombre_premio = nombre_premio;

                            var idPais = Busca.id_pais;
                            var SelecionProductos = hbxEntities.CO_st_BuscaProductos4(idPais).ToList();

                            //return View(SelecionProductos.Where(x => x.checkeado = false).ToList());
                            return View(SelecionProductos.ToList());
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
        }

        // GET: CO_tb_Rango_Compra_Premios/Edit/5
        public ActionResult Edit(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Rango_Compra_Premios cO_tb_Rango_Compra_Premios = db.CO_tb_Rango_Compra_Premios.Find(id);
            if (cO_tb_Rango_Compra_Premios == null)
            {
                return HttpNotFound();
            }


            if (id != null)
            {
                Session["Premio"] = id;

                var Busqueda = db.CO_tb_Premios.Where(x => (x.id_premio == cO_tb_Rango_Compra_Premios.id_premio)).FirstOrDefault();

                if (Busqueda != null)
                {
                    
                    if (cO_tb_Rango_Compra_Premios.Estatus == true)
                    {
                        List<SelectListItem> estatus = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Activo", Value = "1" },
                            new SelectListItem { Text = "Inactivo", Value = "0" },
                        };
                        ViewBag.dropdownEstatus = new SelectList(estatus.ToList(), "Value", "Text", Busqueda.Estatus);

                    }
                    else
                    {
                        List<SelectListItem> estatus = new List<SelectListItem>()
                        {
                            new SelectListItem { Text = "Inactivo", Value = "0" },
                            new SelectListItem { Text = "Activo", Value = "1" },
                        };
                        //ViewBag.dropdownEstatus = estatus;
                        ViewBag.dropdownEstatus = new SelectList(estatus.ToList(), "Value", "Text", Busqueda.Estatus);
                        //ViewBag.dropdownEstatus.OrderByDescending();
                    };



                    var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                                    //where o.tipo == "P" && o.estatus == 1
                                select new SelectListItem
                                {
                                    Text = o.Description.ToString(),
                                    Value = o.type_id.ToString(),
                                };

                    ViewBag.dropdownRanks = Ranks.ToList();

                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   where o.Id_Periodo == Busqueda.id_periodo
                                   select new SelectListItem
                                   {
                                       Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                       Value = o.Id_Periodo.ToString(),
                                   };

                    //ViewBag.dropdownPeriodos = Periodos;

                    ViewBag.dropdownPeriodos = new SelectList(Periodos.ToList(), "Value", "Text", Busqueda.id_periodo);                    
                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1 && x.Id_Pais == Busqueda.id_pais), "Id_Pais", "Pais", Busqueda.id_pais);
                    ViewBag.dropdownPremios = new SelectList(db.CO_tb_Premios.ToList().Where(x => x.Estatus == true && x.Pagado == false), "id_premio", "nombre_premio", Busqueda.id_premio);
                }
            }
            else
            {
                List<SelectListItem> status = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Activo", Value = "1" },
                    new SelectListItem { Text = "Inactivo", Value = "0" },
                };
                ViewBag.dropdownEstatus = status.ToList();

                //var Ranks = from o in Evo.CO_tb_Rank.ToList()
                //                //where o.tipo == "P" && o.estatus == 1
                //            select new SelectListItem
                //            {
                //                Text = o.Rank,
                //                Value = o.Rank_ID.ToString(),
                //            };

                //ViewBag.dropdownRanks = Ranks.ToList();



                var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                                //where o.tipo == "P" && o.estatus == 1
                            select new SelectListItem
                            {
                                Text = o.Description.ToString(),
                                Value = o.type_id.ToString(),
                            };

                ViewBag.dropdownRanks = Ranks.ToList();

                var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                               where o.Estatus == 1
                               select new SelectListItem
                               {
                                   Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                                   Value = o.Id_Periodo.ToString(),
                               };


                //ViewBag.dropdownEstatus = new SelectList(estatus.ToList(), "Value", "Text", cO_tb_Rango_Compra_Premios.Estatus);
                ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais", cO_tb_Rango_Compra_Premios.id_pais);
                ViewBag.dropdownPeriodos = new SelectList(Evo.CO_tb_Periodo.ToList(), "id_periodo", "id_periodo", cO_tb_Rango_Compra_Premios.id_periodo);
                ViewBag.dropdownPremios = new SelectList(db.CO_tb_Premios.ToList().Where(x => x.Pagado == false), "id_premio", "nombre_premio", cO_tb_Rango_Compra_Premios.id_premio);
            }

            return View(cO_tb_Rango_Compra_Premios);
        }

        // POST: CO_tb_Rango_Compra_Premios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IList<CO_st_BuscaProductos4_Result> entity)
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


            if (entity != null)
            {

                CO_tb_Rango_Compra_Premios Rango_Compra = new CO_tb_Rango_Compra_Premios();

                //foreach (var item in entity) /// aqui es
                //{
                //    Rango_Compra.CreadoUsuario = NomUsuario;
                //    Rango_Compra.Creado_F = DateTime.Now;
                //    Rango_Compra.UltModUsuario = NomUsuario;
                //    Rango_Compra.UltMod_F = DateTime.Now;
                //    Rango_Compra.Estatus = true;
                //    Rango_Compra.id_pais = item.Id_Pais;
                //    Rango_Compra.id_periodo = item.Id_Periodo;
                //    Rango_Compra.id_rank = item.Rank_ID;
                //    Rango_Compra.id_premio = item.id_premio;
                //    Rango_Compra.puntos = item.Puntos;
                //}
                //db.CO_tb_Rango_Compra_Premios.Add(Rango_Compra);
                //db.SaveChanges();

                var NomUsuario = Session["IdNombre"].ToString();
                foreach (var item in entity) /// aqui es
                {
                    Rango_Compra.UltModUsuario = NomUsuario;
                    Rango_Compra.UltMod_F = System.DateTime.Now;
                    db.CO_tb_Rango_Compra_Premios.Attach(Rango_Compra);
                    db.Entry(Rango_Compra).State = EntityState.Modified;
                    db.SaveChanges();

                    CO_tb_Productos_Premios cO_tb_Productos_Premios = db.CO_tb_Productos_Premios.Find(item.id_premio);
                    db.CO_tb_Productos_Premios.Remove(cO_tb_Productos_Premios);
                    db.SaveChanges();

                    break;
                }

                var listPartidas = new List<CO_tb_Productos_Premios>();
                foreach (var item in entity) /// aqui es
                {
                    if (item.checkeado == true)
                    {
                        CO_tb_Productos_Premios Partidas = new CO_tb_Productos_Premios();

                        Partidas.id_pais = item.Id_Pais;
                        Partidas.id_premio = item.id_premio;
                        Partidas.id_rank = item.Rank_ID;
                        Partidas.id_producto = item.sku;

                        Partidas.CreadoUsuario = NomUsuario;
                        Partidas.Creado_F = DateTime.Now;
                        Partidas.UltModUsuario = NomUsuario;
                        Partidas.UltMod_F = DateTime.Now;
                        Partidas.Estatus = true;
                        listPartidas.Add(Partidas);
                    }

                }
                db.CO_tb_Productos_Premios.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                db.SaveChanges();
            }

                return RedirectToAction("Index");
        }

        // GET: CO_tb_Rango_Compra_Premios/Delete/5
        public ActionResult Delete(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Rango_Compra_Premios cO_tb_Rango_Compra_Premios = db.CO_tb_Rango_Compra_Premios.Find(id);
            if (cO_tb_Rango_Compra_Premios == null)
            {
                return HttpNotFound();
            }



            if (cO_tb_Rango_Compra_Premios.Estatus == true)
            {
                List<SelectListItem> estatus = new List<SelectListItem>()
                {
                new SelectListItem { Text = "Activo", Value = "1" },
                };
                ViewBag.dropdownEstatus = estatus;

            }
            else
            {
                List<SelectListItem> estatus = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Inactivo", Value = "0" },
                };
                ViewBag.dropdownEstatus = estatus;

            };


            //var Ranks = from o in Evo.CO_tb_Rank.ToList()
            //                //where o.tipo == "P" && o.estatus == 1
            //            select new SelectListItem
            //            {
            //                Text = o.Rank,
            //                Value = o.Rank_ID.ToString(),
            //            };

            //ViewBag.dropdownRanks = Ranks.ToList();



            var Ranks = from o in Evo.CO_tb_TypeDist.ToList()
                            //where o.tipo == "P" && o.estatus == 1
                        select new SelectListItem
                        {
                            Text = o.Description.ToString(),
                            Value = o.type_id.ToString(),
                        };

            ViewBag.dropdownRanks = Ranks.ToList();

            var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                           where o.Estatus == 1
                           select new SelectListItem
                           {
                               Text = "Quincena:" + o.Quincena + " del Mes de " + o.Mes + " del Año " + o.Anio,
                               Value = o.Id_Periodo.ToString(),
                           };

            //ViewBag.dropdownPeriodos = Periodos.ToList();


            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais", cO_tb_Rango_Compra_Premios.id_pais);
            ViewBag.dropdownPeriodos = new SelectList(Evo.CO_tb_Periodo.ToList(), "id_periodo", "id_periodo", cO_tb_Rango_Compra_Premios.id_periodo);
            ViewBag.dropdownPremios = new SelectList(db.CO_tb_Premios.ToList().Where(x => x.Pagado == false), "id_premio", "nombre_premio", cO_tb_Rango_Compra_Premios.id_premio);

            return View(cO_tb_Rango_Compra_Premios);
        }

        // POST: CO_tb_Rango_Compra_Premios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_tb_Rango_Compra_Premios cO_tb_Rango_Compra_Premios = db.CO_tb_Rango_Compra_Premios.Find(id);
            var NomUsuario = Session["IdNombre"].ToString();
            cO_tb_Rango_Compra_Premios.UltModUsuario = NomUsuario;
            cO_tb_Rango_Compra_Premios.UltMod_F = System.DateTime.Now;
            cO_tb_Rango_Compra_Premios.Estatus = false;

            db.CO_tb_Rango_Compra_Premios.Attach(cO_tb_Rango_Compra_Premios);
            db.Entry(cO_tb_Rango_Compra_Premios).State = EntityState.Modified;
            
            //db.CO_tb_Rango_Compra_Premios.Remove(cO_tb_Rango_Compra_Premios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}


      //  @Html.LabelFor(model => model.id_premio, htmlAttributes: new { @class = "control-label col-md-2" })
						//@Html.LabelFor(model => model.id_pais, htmlAttributes: new { @class = "control-label col-md-2" })
						//@Html.LabelFor(model => model.id_periodo, htmlAttributes: new { @class = "control-label col-md-2" })
						//@Html.LabelFor(model => model.id_rank, htmlAttributes: new { @class = "control-label col-md-2" })
						//@Html.LabelFor(model => model.puntos, htmlAttributes: new { @class = "control-label col-md-2" })
						//@Html.LabelFor(model => model.Estatus, htmlAttributes: new { @class = "control-label col-md-2" })
		


        // GET: CO_tb_Rango_Compra_Premios/Productos/5
        public ActionResult Productos(int? id_Rango, int? id_premio, int? id_pais, int? id_periodo, int? id_rank, int? puntos, int? Estatus)
        {
            if (id_Rango == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Rango_Compra_Premios cO_tb_Rango_Compra_Premios = db.CO_tb_Rango_Compra_Premios.Find(id_Rango);
            if (cO_tb_Rango_Compra_Premios == null)
            {
                return HttpNotFound();
            }


            //ViewBag.Premios = db.CO_tb_Premios.Where(x => x.id_premio == id).ToList();

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaProductosConUnId1_Result> LProductos = new List<CO_st_BuscaProductosConUnId1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    var Opcion = 4;
                    var IdPais = id_pais;
                    //int IdPais = int.Parse(cO_tb_Rango_Compra_Premios.id_pais.ToString());
                    //int Id = int.Parse(cO_tb_Rango_Compra_Premios.id_premio.ToString());
                    int Id = int.Parse(id_Rango.ToString());
                    ViewBag.Premios = db.CO_tb_Rango_Compra_Premios.Where(x => x.id_Rango == Id);

                    //int? id_Rango, int? id_premio, int? id_pais, int? id_periodo, int? id_rank, int? puntos, int? Estatus)
                    ViewBag.id_Rango = id_Rango;
                    ViewBag.id_premio = id_premio;
                    ViewBag.id_pais = id_pais;
                    ViewBag.id_periodo = id_periodo;
                    ViewBag.id_rank = id_rank;
                    ViewBag.Estatus = Estatus;

                    if (puntos != null)
                    {
                        ViewBag.puntos = puntos;
                    }
                    else
                    {
                        ViewBag.puntos = cO_tb_Rango_Compra_Premios.puntos;                        
                    }


                    var IdPremio = id_premio;
                    //var IdPremio = cO_tb_Rango_Compra_Premios.id_premio;
                    //LProductos = hbxEntities.CO_st_BuscaProductosConUnId3(IdPais, IdPremio, Opcion).Where(x=>x.checkeado =true).ToList();
                    LProductos = hbxEntities.CO_st_BuscaProductosConUnId5(IdPais, IdPremio, id_rank, Opcion).ToList();
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
        [HttpPost, ActionName("Productos")]
        public ActionResult Productos(IList<CO_st_BuscaProductosConUnId1_Result> entity)
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

            //CO_tb_Rango_Compra_Premios cO_tb_Rango_Compra_Premios = db.CO_tb_Rango_Compra_Premios.Find(id_Rango);
            //if (cO_tb_Rango_Compra_Premios != null)
            //{

            //    db.cO_tb_Rango_Compra_Premios.Attach(Rango_Compra);
            //    db.CO_tb_Rango_Compra_Premios(Rango_Compra).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            var NomUsuario = Session["IdNombre"].ToString();

            //if (ModelState.IsValid)
            //{
                
            //    CO_tb_Rango_Compra_Premios Rango_Compra = new CO_tb_Rango_Compra_Premios();
            //    Rango_Compra.UltModUsuario = NomUsuario;
            //    Rango_Compra.UltMod_F = System.DateTime.Now;
            //    db.Entry(cO_tb_Rango_Compra_Premios).State = EntityState.Modified;
            //    db.SaveChanges();
            //    //return RedirectToAction("Index");
            //}


            if (entity != null)
            {
                foreach (var item in entity) /// aqui es
                {
                //    if (item.checkeado == true)
                //    {

                        CO_tb_Productos_Premios cO_tb_Productos_Premios = db.CO_tb_Productos_Premios.Where(x => x.id_premio == item.id_premio && x.id_rank == item.Rank_ID && x.id_pais == item.Id_Pais && x.id_producto == item.sku).FirstOrDefault();
                        if (cO_tb_Productos_Premios != null)
                        {
                            db.CO_tb_Productos_Premios.Remove(cO_tb_Productos_Premios);
                            db.SaveChanges();
                        }

                        var cO_tb_Rango_Compra_Premios = db.CO_tb_Rango_Compra_Premios.Where(x => x.id_premio == item.id_premio && x.id_rank == item.Rank_ID && x.id_pais == item.Id_Pais && x.id_periodo == item.Id_Periodo).FirstOrDefault();
                        if (cO_tb_Rango_Compra_Premios != null)
                        {
                            //var IdRango = cO_tb_Rango_Compra_Premios.id_Rango;
                            //CO_tb_Rango_Compra_Premios Rango_Compra = new CO_tb_Rango_Compra_Premios();
                            cO_tb_Rango_Compra_Premios.UltModUsuario = NomUsuario;
                            cO_tb_Rango_Compra_Premios.UltMod_F = DateTime.Now; 
                            //Rango_Compra.CreadoUsuario = NomUsuario;
                            //Rango_Compra.Creado_F = System.DateTime.Now;
                            cO_tb_Rango_Compra_Premios.id_pais = item.Id_Pais;
                            cO_tb_Rango_Compra_Premios.id_periodo = item.Id_Periodo;
                            cO_tb_Rango_Compra_Premios.id_rank = item.Rank_ID;
                            //Rango_Compra.Estatus = cO_tb_Rango_Compra_Premios.Estatus;
                            //Rango_Compra.id_Rango = cO_tb_Rango_Compra_Premios.id_Rango;
                            //cO_tb_Rango_Compra_Premios.puntos = cO_tb_Rango_Compra_Premios.puntos;
                            //Rango_Compra.Estatus = bool.Parse(item.Estatus.ToString());
                            cO_tb_Rango_Compra_Premios.id_premio = item.id_premio;
                            cO_tb_Rango_Compra_Premios.puntos = item.Puntos;

                            int TipoEstatus = int.Parse(item.Estatus.ToString());

                            if (TipoEstatus == 1)
                            {
                                cO_tb_Rango_Compra_Premios.Estatus = true;
                            }
                            else
                            {
                                cO_tb_Rango_Compra_Premios.Estatus = false;
                            }

                        
                            db.Entry(cO_tb_Rango_Compra_Premios).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                //    }
                }

                var listPartidas = new List<CO_tb_Productos_Premios>();
                foreach (var item in entity) /// aqui es
                {
                    if (item.checkeado == true)
                    {
                        CO_tb_Productos_Premios Partidas = new CO_tb_Productos_Premios();
                        Partidas.id_pais = item.Id_Pais;
                        Partidas.id_premio = item.id_premio;
                        Partidas.id_rank = item.Rank_ID;
                        Partidas.id_producto = item.sku;
                        Partidas.CreadoUsuario = NomUsuario;
                        Partidas.Creado_F = DateTime.Now;
                        Partidas.UltModUsuario = NomUsuario;
                        Partidas.UltMod_F = DateTime.Now;
                        Partidas.Estatus = true;
                        listPartidas.Add(Partidas);
                    }

                }
                db.CO_tb_Productos_Premios.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }





        // GET: CO_tb_Rango_Compra_Premios/Productos/5
        public ActionResult BuscaProd(int? id_Pais)
        {
            var idPais = id_Pais;
            var SelecionProductos = db.CO_st_BuscaProductos4(idPais).ToList();

            return View(SelecionProductos.ToList());
        }




        public ActionResult BuscaProductos(int? IdPais)
        {
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaProductos4_Result> LProductos = new List<CO_st_BuscaProductos4_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Obtiene las comisiones atrasadas
                    LProductos = hbxEntities.CO_st_BuscaProductos4(IdPais).ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaProductos4  DETALLE: " + ex);
                }
            }
            return View(LProductos.ToList());
        }
    }
}
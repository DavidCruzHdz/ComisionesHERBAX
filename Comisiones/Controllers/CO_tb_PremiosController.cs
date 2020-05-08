using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Comisiones.Models;
using Comisiones.Models.ModelsEvolution;
using Comisiones.Data;

namespace Comisiones.Controllers
{
     public class CO_tb_PremiosController : Controller
    {
        
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();
        private string nameController = "ReembolsosCanjeController";//nombre que se usa para generar elArchivo LOG (Archivo LOG por controlador)
        private string rutaArchivoLog = @"C:\herbax\Logs\";
        

        // GET: CO_tb_Premios
        public ActionResult Index()
        {
            
            return View(db.CO_tb_Premios.Where(x=> x.Estatus == true).ToList());
        }

        // GET: CO_tb_Premios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Premios cO_tb_Premios = db.CO_tb_Premios.Find(id);
            if (cO_tb_Premios == null)
            {
                return HttpNotFound();
            }


            ViewBag.Premios = db.CO_tb_Premios.Where(x => x.id_premio == id).ToList();

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaProductosConUnId1_Result> LProductos = new List<CO_st_BuscaProductosConUnId1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   where o.Estatus == 1
                                   select new SelectListItem
                                   {
                                       Text = o.Id_Periodo.ToString(),
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

                    var Opcion = 3;
                    int Id = cO_tb_Premios.id_premio;
                    int Rank = 0;
                    int IdPais = int.Parse(cO_tb_Premios.id_pais.ToString());

                    //LProductos = hbxEntities.CO_st_BuscaProductos4(IdPais).ToList();
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

        // GET: CO_tb_Premios/Create
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
                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                        var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                       where o.Estatus == 1
                                       select new SelectListItem
                                       {
                                           Text = o.Id_Periodo.ToString(),
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
                        ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                        var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                       where o.Estatus == 1
                                       select new SelectListItem
                                       {
                                           Text = o.Id_Periodo.ToString(),
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

        // POST: CO_tb_Premios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(IList<CO_st_BuscaProductosPremios_Result> entity)
        {
            try
            {
                //var NomUsuario = Session["IdNombre"].ToString();
                //var Usuarios = db.CO_tb_Usuarios.Where(x => (x.NombreUsuario == NomUsuario)).FirstOrDefault();
                //var varUsuario = Usuarios.IdUsuario;

                CO_tb_Premios Premios = new CO_tb_Premios();

                foreach (var item in entity) /// aqui es
                {
                    Premios.nombre_premio = item.nombre_premio;
                    Premios.id_pais = item.Id_Pais;
                    Premios.id_periodo = item.Id_Periodo;
                    Premios.Estatus = true;
                    Premios.Pagado = false;
                }
               
                db.CO_tb_Premios.Add(Premios);
                db.SaveChanges();

                var IdPremio = 0;
                //var Busca = Evo.CO_tb_Premios.OrderByDescending(x => x.nombre_premio == Premios.nombre_premio).FirstOrDefault();

                var Busca = db.CO_tb_Premios.Where(x => x.nombre_premio == Premios.nombre_premio && x.id_pais == Premios.id_pais && x.id_periodo == Premios.id_periodo).FirstOrDefault();
                if (Busca != null)
                {
                    IdPremio = Busca.id_premio;
                }


                var listPartidas = new List<CO_tb_PREMIOPRODUCTO>();
                foreach (var item in entity) /// aqui es
                {
                    if (item.checkeado == true)
                    {
                        CO_tb_PREMIOPRODUCTO Partidas = new CO_tb_PREMIOPRODUCTO();

                        Partidas.IDPREMIO = IdPremio;
                        Partidas.IDPRODUCTO = item.sku;

                        listPartidas.Add(Partidas);
                    }

                }
                db.CO_tb_PREMIOPRODUCTO.AddRange(listPartidas);// a lo que dice el errro  es algo del modelo 
                db.SaveChanges();


                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }



        }


        
    
    // GET: CO_tb_Premios/Edit/5
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Premios cO_tb_Premios = db.CO_tb_Premios.Find(id);
            if (cO_tb_Premios == null)
            {
                return HttpNotFound();
            }
            return View(cO_tb_Premios);
        }

        // POST: CO_tb_Premios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_premio,nombre_premio,id_pais,id_periodo,Estatus,Pagado,CreadoUsuario,Creado_F,UltModUsuario,UltMod_F")] CO_tb_Premios cO_tb_Premios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cO_tb_Premios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_tb_Premios);
        }

        // GET: CO_tb_Premios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_tb_Premios cO_tb_Premios = db.CO_tb_Premios.Find(id);
            if (cO_tb_Premios == null)
            {
                return HttpNotFound();
            }


            ViewBag.Premios = db.CO_tb_Premios.Where(x => x.id_premio == id).ToList();

            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaProductosConUnId1_Result> LProductos = new List<CO_st_BuscaProductosConUnId1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

                    var Periodos = from o in Evo.CO_tb_Periodo.ToList()
                                   where o.Estatus == 1
                                   select new SelectListItem
                                   {
                                       Text = o.Id_Periodo.ToString(),
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

                    var Opcion = 3;
                    int Id = cO_tb_Premios.id_premio;
                    int IdPais = int.Parse(cO_tb_Premios.id_pais.ToString());
                    int Rank = 0;

                    //LProductos = hbxEntities.CO_st_BuscaProductos3(IdPais).ToList();
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

        // POST: CO_tb_Premios/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //CO_tb_Premios cO_tb_Premios = db.CO_tb_Premios.Find(id);
            //db.CO_tb_Premios.Remove(cO_tb_Premios);
            //db.SaveChanges();
            //return RedirectToAction("Index");


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
                            var IdPremio = id;
                            //Obtiene las comisiones atrasadas
                            var Borrar = hbxEntities.CO_st_BorrarProductosPremios(IdPremio);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public JsonResult BuscaReembolsos()
        {
            Log oLog = new Log(rutaArchivoLog);
            List<CO_st_BuscaReembolsos2_Result> LReembolsos = new List<CO_st_BuscaReembolsos2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {
                    //Obtiene las comisiones atrasadas
                    LReembolsos = hbxEntities.CO_st_BuscaReembolsos2().ToList();
                }
                catch (Exception ex)
                {
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaReembolsos  DETALLE: " + ex);
                }
            }
            return Json(LReembolsos, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult PeriodosSelectList(int IdPais)
        //{
        //    //var PeriodosList = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");
        //    var PeriodosList = new SelectList(Evo.CO_tb_Periodo.ToList().Where(x => x.Id_Pais == IdPais), "Id_Pais", "Pais");
        //    return Json(PeriodosList, JsonRequestBehavior.AllowGet);
        //}


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
                    oLog.Add(nameController, "  MENSAJE:  Fallo  SP  CO_st_BuscaProductos4 DETALLE: " + ex);
                }
            }
            return Json(LReembolsos, JsonRequestBehavior.AllowGet);
        }
    }
}




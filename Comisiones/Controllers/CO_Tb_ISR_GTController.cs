using Comisiones.Models;
using Comisiones.Models.ModelsEvolution;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Controllers
{
    public class CO_Tb_ISR_GTController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();

        // GET: CO_Tb_ISR_GT
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 30 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }


            List<CO_st_BuscaISRs2_Result> LISR = new List<CO_st_BuscaISRs2_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    //Obtiene las comisiones atrasadas
                    LISR = hbxEntities.CO_st_BuscaISRs2().ToList();
                    return View(LISR.ToList());

                }
                catch (Exception ex)
                {
                    ViewBag.IsOK = 0;
                    ViewBag.Message = "  MENSAJE:  Fallo  SP  CO_st_BuscaISRs2  DETALLE: " + ex;
                    return View();
                }


            }

        }

        // GET: CO_Tb_ISR_GT/Details/5
        public ActionResult Details(int? id)
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 21 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_Tb_ISR_GT cO_Tb_ISR_GT = db.CO_Tb_ISR_GT.Find(id);
            if (cO_Tb_ISR_GT == null)
            {
                return HttpNotFound();
            }

            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

            var IdPais = cO_Tb_ISR_GT.id_pais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");

            return View(cO_Tb_ISR_GT);
        }

        // GET: CO_Tb_ISR_GT/Create
        public ActionResult Create()
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 21 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }
            //    ViewBag.Id_Regimen = new SelectList(db., "Id_Pais", "Pais");

            //    EvoEntities Evo = new EvoEntities();
            //ViewBag.Id_Pais = new SelectList(Evo.CO_tb_Pais, "Id_Pais", "Pais");


            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

            var IdPais = 0;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");



            return View();
        }

        // POST: CO_Tb_ISR_GT/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_ISR,id_pais,id_regimen,montominimo,limiteinferior,limitesuperior,ISR,fecha_ult_modificacion,usuario_ult_modificacion")] CO_Tb_ISR_GT cO_Tb_ISR_GT)
        {
            var NomUsuario = Session["IdNombre"].ToString();
            cO_Tb_ISR_GT.usuario_ult_modificacion = NomUsuario;
            cO_Tb_ISR_GT.fecha_ult_modificacion = System.DateTime.Now;
            //cO_Tb_ISR_GT.pais = "";
            //cO_Tb_ISR_GT.descripcion = "";

            if (ModelState.IsValid)
            {
                db.CO_Tb_ISR_GT.Add(cO_Tb_ISR_GT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

            var IdPais = cO_Tb_ISR_GT.id_pais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");


            return View(cO_Tb_ISR_GT);
        }

        // GET: CO_Tb_ISR_GT/Edit/5
        public ActionResult Edit(int? id)
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 21 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_Tb_ISR_GT cO_Tb_ISR_GT = db.CO_Tb_ISR_GT.Find(id);
            if (cO_Tb_ISR_GT == null)
            {
                return HttpNotFound();
            }


            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

            var IdPais = cO_Tb_ISR_GT.id_pais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");


            return View(cO_Tb_ISR_GT);
        }

        // POST: CO_Tb_ISR_GT/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_ISR,id_pais,id_regimen,montominimo,limiteinferior,limitesuperior,ISR,fecha_ult_modificacion,usuario_ult_modificacion")] CO_Tb_ISR_GT cO_Tb_ISR_GT)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_Tb_ISR_GT.usuario_ult_modificacion = NomUsuario;
                cO_Tb_ISR_GT.fecha_ult_modificacion = System.DateTime.Now;

                db.Entry(cO_Tb_ISR_GT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cO_Tb_ISR_GT);
        }

        // GET: CO_Tb_ISR_GT/Delete/5
        public ActionResult Delete(int? id)
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 21 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_Tb_ISR_GT cO_Tb_ISR_GT = db.CO_Tb_ISR_GT.Find(id);
            if (cO_Tb_ISR_GT == null)
            {
                return HttpNotFound();
            }

            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Pais == cO_Tb_ISR_GT.id_pais), "Id_Pais", "Pais");

            var IdPais = cO_Tb_ISR_GT.id_pais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");

            return View(cO_Tb_ISR_GT);
        }

        // POST: CO_Tb_ISR_GT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_Tb_ISR_GT cO_Tb_ISR_GT = db.CO_Tb_ISR_GT.Find(id);
            db.CO_Tb_ISR_GT.Remove(cO_Tb_ISR_GT);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Combo(int dropdownPaises)
        {
            //var a = GetDepartamentos();
            //ViewBag.Departamentos = new SelectList(a, "Id_Departamento", "Nombre_Departamento");
            //var b = GetEmpleadosbyDrop(??);

            var IdPais = dropdownPaises;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public JsonResult BuscaPais(int IdPais = 0)
        {
            //List<TablaClientes> lsitTablaCli = new List<TablaClientes>();
            //lsitTablaCli.Clear();
            var Valor = 0;
             var Pais = IdPais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(Pais).ToList(), "id_Regimen", "descripcion");

            if (Pais != 0)
            {
                Valor = 1;
            }
            else
            {
                Valor = 0;
            }

            //return Json(lsitTablaCli, JsonRequestBehavior.AllowGet);

            return Json(Valor);
        }


        public JsonResult RegimenSelectList(int IdPais)
        {
            var RegimenList = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");
            return Json(RegimenList, JsonRequestBehavior.AllowGet);
        }


    }
}

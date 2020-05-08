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
    public class CO_Tb_IVAController : Controller
    {
        private herbaxEntities db = new herbaxEntities();
        private EvolutionEntities Evo = new EvolutionEntities();
        private EVO_PTEntities EvoPT = new EVO_PTEntities();

        // GET: CO_Tb_IVA
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 31 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/SinPermiso");
            }

            

            //var query = from o in db.CO_Tb_IVA.ToList()
            //            from p in Evo.CL_tb_Pais.ToList()
            //            where o.id_pais == p.Id_Pais
            //            select new CO_Tb_IVA()
            //             {
            //                 id_pais = p.Id_Pais,
            //                 Pais = p.Pais,
            //                 id_regimen = o.id_regimen,
            //                 descripcion = n.Pais,
            //                 Ventas = r.VENTA_PERIODO.ToString(),
            //                 Reembolso = r.CANTIDAD_REEMBOLSO.ToString(),
            //                 Periodo = r.ID_PERIODO.ToString()
            //             };


            List<CO_st_BuscaIVAs1_Result> LIva = new List<CO_st_BuscaIVAs1_Result>();

            using (herbaxEntities hbxEntities = new herbaxEntities())
            {
                try
                {

                    //Obtiene las comisiones atrasadas
                    LIva = hbxEntities.CO_st_BuscaIVAs1().ToList();
                    return View(LIva.ToList());

                }
                catch (Exception ex)
                {
                    ViewBag.IsOK = 0;
                    ViewBag.Message = "  MENSAJE:  Fallo  SP  CO_st_BuscaProductosReembolsos  DETALLE: " + ex;
                    return View();
                }


            }


        }

        // GET: CO_Tb_IVA/Details/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 20 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_Tb_IVA cO_Tb_IVA = db.CO_Tb_IVA.Find(id);
            if (cO_Tb_IVA == null)
            {
                return HttpNotFound();
            }

            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Pais == cO_Tb_IVA.id_pais), "Id_Pais", "Pais");

            var IdPais = cO_Tb_IVA.id_pais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");


            return View(cO_Tb_IVA);
        }

        // GET: CO_Tb_IVA/Create
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 20 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }


            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

            var IdPais = 1;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");

            var contar = 0;
            List<SelectListItem> Porciento = new List<SelectListItem>();

            for (int i = 0; i <= 100; i++)
            {
                Porciento.Add(new SelectListItem() { Text = contar.ToString(), Value = contar.ToString() });
                contar++;
            }


            ViewBag.dropdownIVA = Porciento;


            return View();


            //List<SelectListItem> tipo = new List<SelectListItem>()
            //{
            //    new SelectListItem { Text = "Percepcion", Value = "P" },
            //    new SelectListItem { Text = "Deduccion", Value = "D" },
            //};
            //ViewBag.dropdownTipo = tipo;

            //  ViewBag.dropdownUsuario = new SelectList(db.CO_Tb_RegimenFiscal.ViewBag(), "CPIdUsuario", "CPNombreUsuario");


            // FROM[Evo_Comisiones].[dbo].[CO_Tb_RegimenFiscal]


        }

        // POST: CO_Tb_IVA/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_IVA,id_pais,id_regimen,iva,fecha_ult_modificacion,usuario_ult_modificacion")] CO_Tb_IVA cO_Tb_IVA)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_Tb_IVA.usuario_ult_modificacion = NomUsuario;
                cO_Tb_IVA.fecha_ult_modificacion = System.DateTime.Now;

                db.CO_Tb_IVA.Add(cO_Tb_IVA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cO_Tb_IVA);
        }

        // GET: CO_Tb_IVA/Edit/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 20 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_Tb_IVA cO_Tb_IVA = db.CO_Tb_IVA.Find(id);
            if (cO_Tb_IVA == null)
            {
                return HttpNotFound();
            }


            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Estatus == 1), "Id_Pais", "Pais");

            var IdPais = cO_Tb_IVA.id_pais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");

            var contar = 0;
            List<SelectListItem> Porciento = new List<SelectListItem>();

            for (int i = 0; i <= 100; i++)
            {
                Porciento.Add(new SelectListItem() { Text = contar.ToString(), Value = contar.ToString() });
                contar++;
            }


            ViewBag.dropdownIVA = Porciento;

            return View(cO_Tb_IVA);
        }

        // POST: CO_Tb_IVA/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_IVA,id_pais,id_regimen,iva,fecha_ult_modificacion,usuario_ult_modificacion")] CO_Tb_IVA cO_Tb_IVA)
        {
            if (ModelState.IsValid)
            {
                var NomUsuario = Session["IdNombre"].ToString();
                cO_Tb_IVA.usuario_ult_modificacion = NomUsuario;
                cO_Tb_IVA.fecha_ult_modificacion = System.DateTime.Now;

                db.Entry(cO_Tb_IVA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            return View(cO_Tb_IVA);
        }

        // GET: CO_Tb_IVA/Delete/5
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

            if (!db.CO_tb_PermisosRoles.Any(x => x.IdPantalla == 20 && x.IdRol == VarRol))
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CO_Tb_IVA cO_Tb_IVA = db.CO_Tb_IVA.Find(id);
            if (cO_Tb_IVA == null)
            {
                return HttpNotFound();
            }

            ViewBag.dropdownPaises = new SelectList(Evo.CL_tb_Pais.ToList().Where(x => x.Id_Pais == cO_Tb_IVA.id_pais), "Id_Pais", "Pais");

            var IdPais = cO_Tb_IVA.id_pais;
            ViewBag.dropdownRegimen = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");


            return View(cO_Tb_IVA);
        }

        // POST: CO_Tb_IVA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CO_Tb_IVA cO_Tb_IVA = db.CO_Tb_IVA.Find(id);
            db.CO_Tb_IVA.Remove(cO_Tb_IVA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscaRegimenes(int IdPais)
        {
            var b = db.CO_st_BuscarRegimenFiscal(IdPais);
            ViewBag.dropdownRegimenes = b.ToList();
            return View();
        }



        public JsonResult RegimenSelectList(int IdPais)
        {
            var RegimenList = new SelectList(db.CO_st_BuscarRegimenFiscal(IdPais).ToList(), "id_Regimen", "descripcion");
            return Json(RegimenList, JsonRequestBehavior.AllowGet);
        }
    }
}

using Comisiones.Models.ModelsEvolution;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comisiones.Content
{
    public class PremiosReembolsoController : Controller
    {
        private EvolutionEntities db = new EvolutionEntities();

        // GET: PremiosReembolso
        public ActionResult Index()
        {
            return View(db.IN_tb_Products.ToList());
        }

        // GET: PremiosReembolso/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IN_tb_Products iN_tb_Products = db.IN_tb_Products.Find(id);
            if (iN_tb_Products == null)
            {
                return HttpNotFound();
            }
            return View(iN_tb_Products);
        }

        // GET: PremiosReembolso/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PremiosReembolso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sku,warehouse,Id_Empresa,Warehouse_ID,cve_art,Descripcion,DescripcionCorta,PrecioPublico,PrecioMinimo,PrecioCosto,PrecioSocio,XPrecioPublicoL,XPrecioPublicoDescL,XPrecioSocioL,XPrecioSocioDescL,XPrecioPrefL,XPrecioPrefDescL,XPrecioPublico,XPrecioPublicoDesc,XPrecioSocio,XPrecioSocioDesc,XPrecioPref,XPrecioPrefDesc,PuntosPersonal,PuntosPersonalDesc,PuntosComision,PuntosComisionDesc,ActivoSAE")] IN_tb_Products iN_tb_Products)
        {
            if (ModelState.IsValid)
            {
                db.IN_tb_Products.Add(iN_tb_Products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(iN_tb_Products);
        }

        // GET: PremiosReembolso/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IN_tb_Products iN_tb_Products = db.IN_tb_Products.Find(id);
            if (iN_tb_Products == null)
            {
                return HttpNotFound();
            }
            return View(iN_tb_Products);
        }

        // POST: PremiosReembolso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sku,warehouse,Id_Empresa,Warehouse_ID,cve_art,Descripcion,DescripcionCorta,PrecioPublico,PrecioMinimo,PrecioCosto,PrecioSocio,XPrecioPublicoL,XPrecioPublicoDescL,XPrecioSocioL,XPrecioSocioDescL,XPrecioPrefL,XPrecioPrefDescL,XPrecioPublico,XPrecioPublicoDesc,XPrecioSocio,XPrecioSocioDesc,XPrecioPref,XPrecioPrefDesc,PuntosPersonal,PuntosPersonalDesc,PuntosComision,PuntosComisionDesc,ActivoSAE")] IN_tb_Products iN_tb_Products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iN_tb_Products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iN_tb_Products);
        }

        // GET: PremiosReembolso/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IN_tb_Products iN_tb_Products = db.IN_tb_Products.Find(id);
            if (iN_tb_Products == null)
            {
                return HttpNotFound();
            }
            return View(iN_tb_Products);
        }

        // POST: PremiosReembolso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            IN_tb_Products iN_tb_Products = db.IN_tb_Products.Find(id);
            db.IN_tb_Products.Remove(iN_tb_Products);
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
    }
}

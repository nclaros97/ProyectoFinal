using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.Context;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    public class UnidadesController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();

        // GET: Unidades
        public async Task<ActionResult> Index()
        {
            return View(await db.Unidades.ToListAsync());
        }

        // GET: Unidades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidades unidades = await db.Unidades.FindAsync(id);
            if (unidades == null)
            {
                return HttpNotFound();
            }
            return View(unidades);
        }

        // GET: Unidades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Unidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UnidadesId,UnidadesNombre")] Unidades unidades)
        {
            if (ModelState.IsValid)
            {
                db.Unidades.Add(unidades);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(unidades);
        }

        // GET: Unidades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidades unidades = await db.Unidades.FindAsync(id);
            if (unidades == null)
            {
                return HttpNotFound();
            }
            return View(unidades);
        }

        // POST: Unidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UnidadesId,UnidadesNombre")] Unidades unidades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidades).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(unidades);
        }

        // GET: Unidades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidades unidades = await db.Unidades.FindAsync(id);
            if (unidades == null)
            {
                return HttpNotFound();
            }
            return View(unidades);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Unidades unidades = await db.Unidades.FindAsync(id);
            db.Unidades.Remove(unidades);
            await db.SaveChangesAsync();
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

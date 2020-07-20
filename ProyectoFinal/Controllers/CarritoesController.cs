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
    public class CarritoesController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();

        // GET: Carritoes
        public async Task<ActionResult> Index()
        {
            var carritoes = db.Carritoes.Include(c => c.Producto);
            return View(await carritoes.ToListAsync());
        }

        // GET: Carritoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = await db.Carritoes.FindAsync(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // GET: Carritoes/Create
        public ActionResult Create()
        {
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "ProductoTitulo");
            return View();
        }

        // POST: Carritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CarritoId,UsuarioId,CarritoCantidad,CarritoSubTotal,FechaAgregado,IdProducto")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.Carritoes.Add(carrito);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "ProductoTitulo", carrito.IdProducto);
            return View(carrito);
        }

        // GET: Carritoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = await db.Carritoes.FindAsync(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "ProductoTitulo", carrito.IdProducto);
            return View(carrito);
        }

        // POST: Carritoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CarritoId,UsuarioId,CarritoCantidad,CarritoSubTotal,FechaAgregado,IdProducto")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carrito).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "ProductoTitulo", carrito.IdProducto);
            return View(carrito);
        }

        // GET: Carritoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = await db.Carritoes.FindAsync(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // POST: Carritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Carrito carrito = await db.Carritoes.FindAsync(id);
            db.Carritoes.Remove(carrito);
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

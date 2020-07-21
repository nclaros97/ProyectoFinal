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
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;
using ProyectoFinal.Models.ViewModels;

namespace ProyectoFinal.Controllers
{
    public class CarritoesController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();

        // GET: Carritoes
        public async Task<ActionResult> Index()
        {
            string usuario = null;

            if (User.Identity.IsAuthenticated)
            {
                usuario = User.Identity.GetUserId();
            }
            else
            {
                if (Session["UniqueIdUserAnonimo"] == null)
                {
                    usuario = Guid.NewGuid().ToString();
                    Session["UniqueIdUserAnonimo"] = usuario;
                }
                else
                {
                    usuario = (string)Session["UniqueIdUserAnonimo"];
                }

            }
            var carritoes = db.Carritoes.Include(c => c.Producto).Where(x=>x.UsuarioId == usuario);
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
        
        [HttpGet]
        public ActionResult _Carrito()
        {
            string usuario = null;

            if (User.Identity.IsAuthenticated)
            {
                usuario = User.Identity.GetUserId();
            }
            else
            {
                if (Session["UniqueIdUserAnonimo"] == null)
                {
                    usuario = Guid.NewGuid().ToString();
                    Session["UniqueIdUserAnonimo"] = usuario;
                }
                else
                {
                    usuario = (string)Session["UniqueIdUserAnonimo"];
                }

            }
            ViewBag.Cant = db.Carritoes.Where(x => x.UsuarioId == usuario).Count();
            return PartialView();
        }

        // POST: Carritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = await db.Productos.FindAsync(id);
            if(producto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string usuario = null;
            if (User.Identity.IsAuthenticated)
            {
                usuario = User.Identity.GetUserId();
            }
            else
            {
                if(Session["UniqueIdUserAnonimo"] == null)
                {
                    usuario = Guid.NewGuid().ToString();
                    Session["UniqueIdUserAnonimo"] = usuario;
                }
                else
                {
                    usuario = (string)Session["UniqueIdUserAnonimo"];
                }
               
                
            }

            var verificar = db.Carritoes.Where(x => x.IdProducto == id &&  x.UsuarioId == usuario).SingleOrDefault();
            if(verificar != null)
            {
                var addCarrito = new Carrito
                {
                    CarritoId = verificar.CarritoId,
                    CarritoCantidad = verificar.CarritoCantidad+1,
                    FechaAgregado = DateTime.Now,
                    IdProducto = (int)id,
                    UsuarioId = usuario
                };
                db.Set<Carrito>().AddOrUpdate(addCarrito);
                await db.SaveChangesAsync();
                //actualizar a cantidad de la carretilla si ya existe un producto agregado
            }
            else{
                var addCarrito = new Carrito
                {
                    CarritoCantidad = 1,
                    FechaAgregado = DateTime.Now,
                    IdProducto = (int)id,
                    UsuarioId = usuario
                };
                db.Carritoes.Add(addCarrito);
                await db.SaveChangesAsync();
            }
            var prod = await db.Productos.FindAsync(id);
            var updateProducto = new Productos
            {
                IdProducto = prod.IdProducto,
                CategoryKey = prod.CategoryKey,
                ProductoImagenes = prod.ProductoImagenes,
                ProductoTitulo = prod.ProductoTitulo,
                ProductoPrecio = prod.ProductoPrecio,
                ProductStock = prod.ProductStock - 1,
                ProdutoDescripcion = prod.ProdutoDescripcion,
                UnidadesId = prod.UnidadesId
            };
            db.Set<Productos>().AddOrUpdate(updateProducto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index","Home");
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

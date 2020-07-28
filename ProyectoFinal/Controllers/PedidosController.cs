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
using ProyectoFinal.Models.ViewModels;

namespace ProyectoFinal.Controllers
{
    public class PedidosController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();

        // GET: Pedidos
        public async Task<ActionResult> Index()
        {

            return View(await db.Pedidos.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public ActionResult Details()
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
            ViewBag.FormaPagoId = new SelectList(db.FormaPagoes, "FormaPagoId", "FormaPagoNombre");
            var carrito = db.Carritoes.Include(c => c.Producto).Where(x => x.UsuarioId == usuario).ToList();
            var view = new PedidosViewModel();
            view.carrito = carrito;
            return View(view);
        }


        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pedidos model)
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

            model.UsuarioId = usuario;
            model.EstadoPedido = 1;
            model.PedidoFecha = DateTime.Now;

            var carritoUsuario = db.Carritoes.Where(c => c.UsuarioId == usuario);
            decimal total = 0;
            foreach (var item in carritoUsuario)
            {
                total += (decimal.Parse(item.CarritoCantidad.ToString()) * item.Producto.ProductoPrecio);
            }
            model.TotalPedido = total;

            db.Pedidos.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Pedidos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedidos pedidos = await db.Pedidos.FindAsync(id);
            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PedidoId,PedidoFecha,UsuarioId,TotalPedido,EstadoPedido,PedidoDireccion")] Pedidos pedidos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedidos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pedidos);
        }

        // GET: Pedidos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedidos pedidos = await db.Pedidos.FindAsync(id);
            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pedidos pedidos = await db.Pedidos.FindAsync(id);
            db.Pedidos.Remove(pedidos);
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

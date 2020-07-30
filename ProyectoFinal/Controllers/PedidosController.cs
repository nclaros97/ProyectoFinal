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
using System.Data.Entity.Migrations;
using System.Web.Http.Results;

namespace ProyectoFinal.Controllers
{
    public class PedidosController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();

        // GET: Pedidos
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
            return View(await db.Pedidos.Where(p => p.UsuarioId == usuario).ToListAsync());
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
        public async Task<ActionResult> Create(PedidosViewModel model)
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

            var carritoUsuario = db.Carritoes.Include(c => c.Producto).Where(x => x.UsuarioId == usuario);
            decimal total = 0;
            foreach (var item in carritoUsuario)
            {
                total += (decimal.Parse(item.CarritoCantidad.ToString()) * item.Producto.ProductoPrecio);
            }

            var pedido = new Pedidos
            {

                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                Email = model.Email,
                Telefono = model.Telefono,
                UsuarioId = usuario,
                EstadoPedido = 1,
                FormaPagoId = model.FormaPagoId,
                PedidoDireccion = model.PedidoDireccion,
                PedidoFecha = DateTime.Now,
                TotalPedido = total
            };
            db.Pedidos.Add(pedido);
            await db.SaveChangesAsync();

            await saveDetalleAsync(usuario, carritoUsuario, pedido.PedidoId);

            return RedirectToAction("Index");
        }

        private async Task saveDetalleAsync(string usuario, IQueryable<Carrito> model, int id)
        {
            // se usan transacciones para poder realizar varios cambios a la db al mismo tiempo4
            using (var dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model)
                    {
                        var detalle = new DetallePedido
                        {
                            DetalleProductoCantidad = item.CarritoCantidad,
                            ProductoId = item.IdProducto,
                            PedidoId = id
                        };
                        //guarda el detalle
                        db.Set<DetallePedido>().AddOrUpdate(detalle);
                        await db.SaveChangesAsync();

                        //borrar el carrito
                        db.Set<Carrito>().Remove(item);
                        await db.SaveChangesAsync();

                    }
                    dbTran.Commit();
                }
                catch (Exception)
                {
                    // roll back all database operations, if any thing goes wrong
                    dbTran.Rollback();
                }
            }
        }


        public ActionResult Items(int? id)
        {
            if(id == null)
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

            var datosPedido = db.Pedidos.Include(p => p.FormaPago).Where(p => p.PedidoId == id).FirstOrDefault();
            var pedido = new DetalleCompraViewModel { 
                PedidoId = datosPedido.PedidoId,
                Nombres = datosPedido.Nombres,
                Apellidos = datosPedido.Apellidos,
                Email = datosPedido.Email,
                EstadoPedido = datosPedido.EstadoPedido,
                FormaPagoId = datosPedido.FormaPagoId,
                PedidoDireccion = datosPedido.PedidoDireccion,
                PedidoFecha = datosPedido.PedidoFecha,
                Telefono = datosPedido.Telefono,
                UsuarioId = usuario,
            };

            var detalle = db.DetallePedidos.Include(d => d.Productos).Include(d => d.Productos.Category).Include(d => d.Productos.Unidades).Where(d => d.PedidoId == id).ToList();

            pedido.Detalle = detalle;

            return View(pedido);
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

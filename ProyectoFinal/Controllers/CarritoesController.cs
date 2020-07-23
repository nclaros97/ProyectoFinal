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
using Microsoft.AspNet.Identity.Owin;

namespace ProyectoFinal.Controllers
{
    public class CarritoesController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();
        private ApplicationUserManager _userManager;

        public CarritoesController()
        {
        }

        public CarritoesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Carritoes
        public async Task<ActionResult> Index(string msg)
        {
            if (msg != null)
            {
                ViewBag.Mensaje = msg;
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

            //para quitar del carrito los productos agregados por usuarios anonimos que hayan perdido o caducado el tiempo de la sesion 
            //se deberia de crear una condicion que cada verifique la fecha de registro del producto en la carretilla y calcular 24 horas que es el tiempo maximo de una
            //sesion de un usuario anonimo dicho tiempo esta en web.config en el apartado de <system.web> <sessionState timeout="86400"></sessionState>

            var carritoes = db.Carritoes.Include(c => c.Producto).Where(x=>x.UsuarioId == usuario);

            return View(await carritoes.ToListAsync());
        }


        public async Task<ActionResult> Add(DetalleProductoViewModel model,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = await db.Productos.FindAsync(id);
            if (producto == null)
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

            var verificar = db.Carritoes.Where(x => x.IdProducto == id && x.UsuarioId == usuario).SingleOrDefault();
            if (producto.ProductStock > 0 && producto.ProductStock >= model.Cantidad)
            {
                if (verificar != null)
                {
                    var addCarrito = new Carrito
                    {
                        CarritoId = verificar.CarritoId,
                        CarritoCantidad = verificar.CarritoCantidad + model.Cantidad,
                        FechaAgregado = DateTime.Now,
                        IdProducto = (int)id,
                        UsuarioId = usuario
                    };
                    db.Set<Carrito>().AddOrUpdate(addCarrito);
                    await db.SaveChangesAsync();
                    //actualizar a cantidad de la carretilla si ya existe un producto agregado
                }
                else
                {
                    var addCarrito = new Carrito
                    {
                        CarritoCantidad = model.Cantidad,
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
                    ProductStock = prod.ProductStock - model.Cantidad,
                    ProdutoDescripcion = prod.ProdutoDescripcion,
                    UnidadesId = prod.UnidadesId
                };
                db.Set<Productos>().AddOrUpdate(updateProducto);
                await db.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { msg = "Stock Insuficiente" });
            }

            return RedirectToAction("Index", "Home", new { msg = "Producto añadido al carrito" });
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
            if (producto.ProductStock > 0 )
            {
                if (verificar != null)
                {
                    var addCarrito = new Carrito
                    {
                        CarritoId = verificar.CarritoId,
                        CarritoCantidad = verificar.CarritoCantidad + 1,
                        FechaAgregado = DateTime.Now,
                        IdProducto = (int)id,
                        UsuarioId = usuario
                    };
                    db.Set<Carrito>().AddOrUpdate(addCarrito);
                    await db.SaveChangesAsync();
                    //actualizar a cantidad de la carretilla si ya existe un producto agregado
                }
                else
                {
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
            }
            else
            {
                return RedirectToAction("Index", "Home", new { msg = "Stock Insuficiente"});
            }

            return RedirectToAction("Index","Home",new { msg = "Producto añadido al carrito" });
        }


        [HttpPost]
        public async Task<ActionResult> AddOne(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = await db.Productos.FindAsync(id);
            if (producto == null)
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

            var verificar = db.Carritoes.Where(x => x.IdProducto == id && x.UsuarioId == usuario).SingleOrDefault();
            if (producto.ProductStock > 0)
            {
                if (verificar != null)
                {
                    var addCarrito = new Carrito
                    {
                        CarritoId = verificar.CarritoId,
                        CarritoCantidad = verificar.CarritoCantidad + 1,
                        FechaAgregado = DateTime.Now,
                        IdProducto = (int)id,
                        UsuarioId = usuario
                    };
                    db.Set<Carrito>().AddOrUpdate(addCarrito);
                    await db.SaveChangesAsync();
                    //actualizar a cantidad de la carretilla si ya existe un producto agregado
                }
                else
                {
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
            }
            else
            {
                return RedirectToAction("Index", new { msg = "Stock Insuficiente" });
            }
            return RedirectToAction("Index", new { msg = "Producto Agregado" });
        }

        // GET: Carritoes/Delete/5
        public async Task<ActionResult> RemoveOne(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = await db.Productos.FindAsync(id);
            if (producto == null)
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

            var verificar = db.Carritoes.Where(x => x.IdProducto == id && x.UsuarioId == usuario).SingleOrDefault();

                if (verificar != null)
                {
                    if(verificar.CarritoCantidad > 1)
                {
                    var addCarrito = new Carrito
                    {
                        CarritoId = verificar.CarritoId,
                        CarritoCantidad = verificar.CarritoCantidad - 1,
                        FechaAgregado = DateTime.Now,
                        IdProducto = (int)id,
                        UsuarioId = usuario
                    };
                    db.Set<Carrito>().AddOrUpdate(addCarrito);
                    await db.SaveChangesAsync();
                    //actualizar a cantidad de la carretilla si ya existe un producto agregado
                }
                else
                {
                    Carrito carrito = await db.Carritoes.FindAsync(verificar.CarritoId);
                    db.Carritoes.Remove(carrito);
                    await db.SaveChangesAsync();
                }
            }
            var prod = await db.Productos.FindAsync(id);
            var updateProducto = new Productos
            {
                IdProducto = prod.IdProducto,
                CategoryKey = prod.CategoryKey,
                ProductoImagenes = prod.ProductoImagenes,
                ProductoTitulo = prod.ProductoTitulo,
                ProductoPrecio = prod.ProductoPrecio,
                ProductStock = prod.ProductStock + 1,
                ProdutoDescripcion = prod.ProdutoDescripcion,
                UnidadesId = prod.UnidadesId
            };
            db.Set<Productos>().AddOrUpdate(updateProducto);
            await db.SaveChangesAsync();

            return RedirectToAction("Index",new { msg = "Cantidad reducida: 1"});
        }

        // POST: Carritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Carrito carrito = await db.Carritoes.FindAsync(id);


            var prod = await db.Productos.FindAsync(id);
            var updateProducto = new Productos
            {
                IdProducto = prod.IdProducto,
                CategoryKey = prod.CategoryKey,
                ProductoImagenes = prod.ProductoImagenes,
                ProductoTitulo = prod.ProductoTitulo,
                ProductoPrecio = prod.ProductoPrecio,
                ProductStock = prod.ProductStock + carrito.CarritoCantidad,
                ProdutoDescripcion = prod.ProdutoDescripcion,
                UnidadesId = prod.UnidadesId
            };
            db.Set<Productos>().AddOrUpdate(updateProducto);
            await db.SaveChangesAsync();

            
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

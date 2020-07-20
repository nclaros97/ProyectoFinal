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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Drawing;

namespace ProyectoFinal.Controllers
{
    public class ProductosController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();

        // GET: Productos
        public async Task<ActionResult> Index()
        {
            var productos = db.Productos.Include(p => p.Category).Include(p => p.Unidades);
            return View(await productos.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = await db.Productos.FindAsync(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoryKey = new SelectList(db.Categories, "CategoryKey", "CotegoryName");
            ViewBag.UnidadesId = new SelectList(db.Unidades, "UnidadesId", "UnidadesNombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProductosViewModel productos)
        {
            if (ModelState.IsValid)
            {
                Productos newProduct = null;
                if (productos.ProductoImagenes.ElementAt(0) == null)
                {
                    newProduct = new Productos
                    {
                        ProductoTitulo = productos.ProductoTitulo,
                        ProdutoDescripcion = productos.ProdutoDescripcion,
                        ProductoPrecio = productos.ProductoPrecio,
                        ProductStock = productos.ProductStock,
                        CategoryKey = productos.CategoryKey,
                        UnidadesId = productos.UnidadesId,
                        ProductoImagenes = "/img/noimage.jpg"

                    };
                    db.Productos.Add(newProduct);
                    await db.SaveChangesAsync();
                }
                else
                {
                    newProduct = new Productos
                    {
                        ProductoTitulo = productos.ProductoTitulo,
                        ProdutoDescripcion = productos.ProdutoDescripcion,
                        ProductoPrecio = productos.ProductoPrecio,
                        ProductStock = productos.ProductStock,
                        CategoryKey = productos.CategoryKey,
                        UnidadesId = productos.UnidadesId,
                        ProductoImagenes = "/img/"+Path.GetFileName(productos.ProductoImagenes.ElementAt(0).FileName)

                    };
                    db.Productos.Add(newProduct);
                    await db.SaveChangesAsync();
                }
                
                string uniqueName = null;
                if(productos.ProductoImagenes.ElementAt(0) != null)
                {
                    for (int i = 0; i < productos.ProductoImagenes.Count(); i++)
                    {
                        var fileName = Path.GetFileName(productos.ProductoImagenes.ElementAt(i).FileName);
                        string pa = Server.MapPath("/img").ToString();
                        var Filepath = Path.Combine(Server.MapPath("/img"), fileName);
                        productos.ProductoImagenes.ElementAt(i).SaveAs(Filepath);
                        uniqueName = "/img/" + fileName;

                        var imagen = new ImagenProducto
                        {
                            urlImagen = uniqueName,
                            IdProducto = newProduct.IdProducto
                        };
                        db.ImagenesProductos.Add(imagen);
                        await db.SaveChangesAsync();
                    }

                }
                else
                {
                    var imagen = new ImagenProducto
                    {
                        urlImagen = "/img/noimage.jpg",
                        IdProducto = newProduct.IdProducto
                    };
                    db.ImagenesProductos.Add(imagen);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }

            ViewBag.CategoryKey = new SelectList(db.Categories, "CategoryKey", "CotegoryName", productos.CategoryKey);
            ViewBag.UnidadesId = new SelectList(db.Unidades, "UnidadesId", "UnidadesNombre", productos.UnidadesId);
            return View(productos);
        }

        // GET: Productos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = await db.Productos.FindAsync(id);
            CreateProductosViewModel prod = new CreateProductosViewModel
            {
                IdProducto = productos.IdProducto,
                ProductoTitulo = productos.ProductoTitulo,
                ProdutoDescripcion = productos.ProdutoDescripcion,
                ProductoPrecio = productos.ProductoPrecio,
                ProductStock = productos.ProductStock,
                CategoryKey = productos.CategoryKey,
                UnidadesId = productos.UnidadesId
            };
            if (productos == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryKey = new SelectList(db.Categories, "CategoryKey", "CotegoryName", productos.CategoryKey);
            ViewBag.UnidadesId = new SelectList(db.Unidades, "UnidadesId", "UnidadesNombre", productos.UnidadesId);
            return View(prod);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateProductosViewModel productos)
        {
            if (ModelState.IsValid)
            {
                Productos newProduct = null;
                if (productos.ProductoImagenes.ElementAt(0) == null)
                {
                    newProduct = new Productos
                    {
                        IdProducto = productos.IdProducto,
                        ProductoTitulo = productos.ProductoTitulo,
                        ProdutoDescripcion = productos.ProdutoDescripcion,
                        ProductoPrecio = productos.ProductoPrecio,
                        ProductStock = productos.ProductStock,
                        CategoryKey = productos.CategoryKey,
                        UnidadesId = productos.UnidadesId,
                        ProductoImagenes = "/img/noimage.jpg"

                    };
                }
                else
                {
                    newProduct = new Productos
                    {
                        IdProducto = productos.IdProducto,
                        ProductoTitulo = productos.ProductoTitulo,
                        ProdutoDescripcion = productos.ProdutoDescripcion,
                        ProductoPrecio = productos.ProductoPrecio,
                        ProductStock = productos.ProductStock,
                        CategoryKey = productos.CategoryKey,
                        UnidadesId = productos.UnidadesId,
                        ProductoImagenes = "/img/" + Path.GetFileName(productos.ProductoImagenes.ElementAt(0).FileName)

                    };
                }

                db.Entry(newProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();

                string uniqueName = null;
                if (productos.ProductoImagenes.ElementAt(0) != null)
                {
                    //borrar imagenes que ya estaban almacenadas
                    var ima =  db.ImagenesProductos.Where(x => x.IdProducto == newProduct.IdProducto);
                    foreach (var item in ima)
                    {
                        if(item.IdProducto == newProduct.IdProducto)
                        {
                            db.ImagenesProductos.Remove(item);
                            //await db.SaveChangesAsync();
                        }
                    }

                    for (int i = 0; i < productos.ProductoImagenes.Count(); i++)
                    {
                        var fileName = Path.GetFileName(productos.ProductoImagenes.ElementAt(i).FileName);
                        string pa = Server.MapPath("/img").ToString();
                        var Filepath = Path.Combine(Server.MapPath("/img"), fileName);
                        productos.ProductoImagenes.ElementAt(i).SaveAs(Filepath);
                        uniqueName = "/img/" + fileName;

                        var imagen = new ImagenProducto
                        {
                            urlImagen = uniqueName,
                            IdProducto = newProduct.IdProducto
                        };
                        db.ImagenesProductos.Add(imagen);
                        await db.SaveChangesAsync();
                    }

                }
                else
                {
                    var imagen = new ImagenProducto
                    {
                        urlImagen = "/img/noimage.jpg",
                        IdProducto = newProduct.IdProducto
                    };
                    db.ImagenesProductos.Add(imagen);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            ViewBag.CategoryKey = new SelectList(db.Categories, "CategoryKey", "CotegoryName", productos.CategoryKey);
            ViewBag.UnidadesId = new SelectList(db.Unidades, "UnidadesId", "UnidadesNombre", productos.UnidadesId);
            return View(productos);
        }

        // GET: Productos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = await db.Productos.FindAsync(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Productos productos = await db.Productos.FindAsync(id);
            db.Productos.Remove(productos);
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

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
    public class HomeController : Controller
    {
        private ProyectoFinalDbContext db = new ProyectoFinalDbContext();
        public ActionResult Index()
        {
            var productosGroupByCat = new List<Productos>();
            var productos = db.Productos.Include(p => p.Category).Include(p => p.Unidades);

            return View(productos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
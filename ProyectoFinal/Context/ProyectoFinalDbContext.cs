using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Context
{
    public class ProyectoFinalDbContext : DbContext
    {
        public System.Data.Entity.DbSet<ProyectoFinal.Models.Productos> Productos { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.Carrito> Carritoes { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.Unidades> Unidades { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.ImagenProducto> ImagenesProductos { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.Pedidos> Pedidos { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.FormaPago> FormaPagoes { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.DetallePedido> DetallePedidos { get; set; }
    }
}
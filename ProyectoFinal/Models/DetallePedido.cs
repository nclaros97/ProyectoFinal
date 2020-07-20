using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class DetallePedido
    {
        public int DetallePedidoId { get; set; }

        public int ProductoId { get; set; }

        public string DetalleProductoDescripcion { get; set; }

        public int DetalleProductoCantidad { get; set; }

        public decimal DetalleProductoPrecio { get; set; }
    }
}
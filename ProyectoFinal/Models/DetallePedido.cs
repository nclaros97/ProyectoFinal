using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class DetallePedido
    {
        public int DetallePedidoId { get; set; }

        public int PedidoId { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Productos Productos { get; set; }

        public int DetalleProductoCantidad { get; set; }

    }
}
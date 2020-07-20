using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Pedidos
    {
        public int PedidoId { get; set; }

        public DateTime PedidoFecha { get; set; }

        public string UsuarioId { get; set; }

        public decimal TotalPedido { get; set; }

        public int EstadoPedido { get; set; }

        public string PedidoDireccion { get; set; }


    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class PedidosViewModel
    {
        public PedidosViewModel()
        {
            carrito = new List<Carrito>();
        }
        public int PedidoId { get; set; }

        public DateTime PedidoFecha { get; set; }

        public string UsuarioId { get; set; }

        public decimal TotalPedido { get; set; }

        public int EstadoPedido { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string PedidoDireccion { get; set; }

        [Display(Name = "Forma Pago")]
        public int FormaPagoId { get; set; }

        [ForeignKey("FormaPagoId")]
        public FormaPago FormaPago { get; set; }

        public List<Carrito> carrito { get; set; }

    }
}
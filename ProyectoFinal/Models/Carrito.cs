using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Carrito
    {
        [Key]
        [Display(Name ="ID")]
        public int CarritoId { get; set; }

        [Display(Name = "Usuario ID")]
        public string UsuarioId { get; set; }

        [Display(Name = "Cantidad")]
        public int CarritoCantidad { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime FechaAgregado { get; set; }

        [Display(Name = "Producto ID")]
        public int IdProducto { get; set; }
        [ForeignKey("IdProducto")]
        public Productos Producto { get; set; }
    }
}
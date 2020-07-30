using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Productos
    {
        [Key]
        public int IdProducto { get; set; }

        public string ProductoTitulo { get; set; }

        public string ProdutoDescripcion { get; set; }

        public string ProductoImagenes { get; set; }

        public decimal ProductoPrecio { get; set; }

        [Display(Name = "Existencia")]
        [Required]
        public float ProductStock { get; set; }

        [Display(Name = "Categoría")]
        public int CategoryKey { get; set; }

        [ForeignKey("CategoryKey")]
        public Category Category { get; set; }

        [Display(Name = "Unidades")]
        public int UnidadesId { get; set; }

        [ForeignKey("UnidadesId")]
        public Unidades Unidades { get; set; }

        public virtual ICollection<Carrito> Carritos { get; set; }

        public virtual ICollection<DetallePedido> DetallePedidos { get; set; }

        public virtual ICollection<ImagenProducto> ImagesnProductos { get; set; }
    }
}
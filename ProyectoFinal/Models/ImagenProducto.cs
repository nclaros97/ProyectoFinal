using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class ImagenProducto
    {
        [Key]
        public int IdImagen { get; set; }

        public string urlImagen { get; set; }

        public int IdProducto { get; set; }

        [ForeignKey("IdProducto")]
        public Productos Productos { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class DetalleProductoViewModel
    {
        public Productos producto { get; set; }

        public List<ImagenProducto> imagenesProducto { get; set; }

        [Range(1,int.MaxValue, ErrorMessage = "No se permiten numeros negativos")]
        public int Cantidad { get; set; }
    }
}
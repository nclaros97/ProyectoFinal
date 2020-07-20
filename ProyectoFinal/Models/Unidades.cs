using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Unidades
    {
        [Key]
        [Display(Name = "ID")]
        public int UnidadesId { get; set; }

        [Display(Name ="Unidad")]
        public string UnidadesNombre { get; set; }
        public virtual ICollection<Productos> Productos { get; set; }
    }
}
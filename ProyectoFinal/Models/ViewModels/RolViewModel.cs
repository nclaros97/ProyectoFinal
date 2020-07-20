using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class RolViewModel
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Nombre del rol es obligatorio")]
        public string Name { get; set; }
    }
}
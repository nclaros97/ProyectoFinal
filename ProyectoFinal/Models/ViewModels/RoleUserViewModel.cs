using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class RoleUserViewModel
    {
        public RoleUserViewModel()
        {
            Roles = new List<string>();
        }

        [Display(Name = "ID")]
        public string UserId { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Display(Name = "Telefono")]
        public string UserPhone { get; set; }

        public List<string> Roles { get; set; }
    }
}
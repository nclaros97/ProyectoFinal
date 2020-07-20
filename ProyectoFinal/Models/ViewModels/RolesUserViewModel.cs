using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class RolesUserViewModel
    {
        public string Id { get; set; }
        public string RolName { get; set; }
        public bool isSelected { get; set; }
    }
}
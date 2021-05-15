using LaTrinidad.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaTrinidad.Web.Models
{
    public class UserRoleViewModel : UserEntity
    {
        [Display(Name = "Foto")]
        public IFormFile FotoFile { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}

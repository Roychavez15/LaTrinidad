using LaTrinidad.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LaTrinidad.Web.Data.Entities
{

        public class UserEntity : IdentityUser
        {
            [Display(Name = "Cédula")]
            [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
            [Required(ErrorMessage = "The field {0} is mandatory.")]
            public string Document { get; set; }

            [Display(Name = "Nombres")]
            [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
            [Required(ErrorMessage = "The field {0} is mandatory.")]
            public string FirstName { get; set; }

            [Display(Name = "Apellidos")]
            [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
            [Required(ErrorMessage = "The field {0} is mandatory.")]
            public string LastName { get; set; }

            [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]

        [Display(Name = "Dirección")]
        public string Address { get; set; }

            [Display(Name = "Foto")]
            public string PicturePath { get; set; }

            [Display(Name = "Role")]
            public UserType UserType { get; set; }

        public SpecialtyEntity SpecialtyEntity { get; set; }

        [Display(Name = "Nombres y Apellidos")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Nombres-Cedula")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
        }
    
}
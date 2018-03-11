using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinalTask.Domain.Entities;

namespace FinalTask.WebUI.Areas.Admin.Models
{
    public class CreateModel
    {
        [Required]
        [Display(Name = "Введіть електронну пошту (логін)")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Введіть пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Введіть ім'я")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Введіть прізвище")]
        public string Surname { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Введіть номер телефону")]
        public string Telephone { get; set; }
    }

    public class RoleEditModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
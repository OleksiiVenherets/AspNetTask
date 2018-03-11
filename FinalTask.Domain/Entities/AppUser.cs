using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalTask.Domain.Entities
{
    public class AppUser: IdentityUser
    {
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Display(Name = "Номер телефону")]
        public string Telephone { get; set; }

        public virtual List<Visit> Visits { get; set; }
    }
}

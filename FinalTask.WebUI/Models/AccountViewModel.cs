using System.ComponentModel.DataAnnotations;

namespace FinalTask.WebUI.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Електронна пошта (логін)")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "{0} має мати щонайменше{2} символи", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Введіть електронну пошту (логін)")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} має мати щонайменше{2} символи", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Введіть пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Введіть пароль ще раз")]
        [Compare("Password", ErrorMessage = "Введені паролі не співпадають")]
        public string ConfirmPassword { get; set; }

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

    public class EditViewModel
    {
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
}
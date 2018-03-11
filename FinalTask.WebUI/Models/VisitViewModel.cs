using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using FinalTask.Domain.Entities;

namespace FinalTask.WebUI.Models
{
    public class AddVisitViewModel
    {
        [Required]
        [Display(Name = "Введіть назву міста")]
        public string CityName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsVisited { get; set; }

        [Required]
        [Display(Name = "Введіть дату відвідування")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Напишіть відгук про візит")]
        public string Comment { get; set; }

        [Display(Name = "Вкажіть рейтинг відвіданого міста (від 1 до 10)")]
        public decimal? Rate { get; set; }

        [Display(Name = "Оберіть фото")]
        public HttpPostedFileBase Photo { get; set; }
    }

    public class EditVisitViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Напишіть відгук про візит")]
        public string Comment { get; set; }

        [Display(Name = "Вкажіть рейтинг відвіданого міста (від 1 до 10)")]
        public decimal? Rate { get; set; }

        [Display(Name = "Оберіть фото")]
        public HttpPostedFileBase Photo { get; set; }
    }

    public class SeeVisitViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Дата відвідування")]
        public DateTime? Date { get; set; }

        [Display(Name = "Місто")]
        public string CityName { get; set; }

        [Display(Name = "Коментар")]
        public string Comment { get; set; }

        [Display(Name = "Ваша оцінка візиту")]
        public decimal? Rate { get; set; }

        [Display(Name = "Фото із візиту")]
        public IEnumerable<Photo> Photos { get; set; }
    }

    public class SeeInMapModel
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
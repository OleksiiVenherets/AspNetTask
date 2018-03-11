using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalTask.Domain.Entities
{
    public class Visit
    {
        public int Id { get; set; }

        public bool IsVisited { get; set; }

        [Display(Name = "Дата відвідування")]
        public DateTime? Date { get; set; }

        [Display(Name = "Відгук")]
        public string Comment { get; set; }

        [Display(Name = "Рейтинг")]
        public decimal? Rate { get; set; }

        public int CityId { get; set; }

        public string UserId { get; set; }

        public virtual List<Photo> Photos { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
    }
}

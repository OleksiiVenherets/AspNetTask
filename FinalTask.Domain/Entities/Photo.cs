using System.ComponentModel.DataAnnotations.Schema;


namespace FinalTask.Domain.Entities
{
    public class Photo
    {
        public int Id { get; set; }

        public int VisitId { get; set; }

        public string Link { get; set; }

        [ForeignKey("VisitId")]
        public Visit Visit { get; set; }
    }
}

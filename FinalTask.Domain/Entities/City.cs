using System.Collections.Generic;

namespace FinalTask.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public virtual List<Visit> Visits { get; set; }
    }
}

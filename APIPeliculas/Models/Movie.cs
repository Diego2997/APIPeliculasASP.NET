using System.ComponentModel.DataAnnotations.Schema;

namespace APIPeliculas.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RouteImage { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public enum TypeClassification
        {
            seven, thirteen, sixteen,eighteen
        }
        public TypeClassification Classification { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

}

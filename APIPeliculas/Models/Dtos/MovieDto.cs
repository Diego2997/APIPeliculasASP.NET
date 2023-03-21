using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPeliculas.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "the name is required")]
        public string Name { get; set; }
        public string RouteImage { get; set; }
        [Required(ErrorMessage = "the description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "the duration is required")]
        public int Duration { get; set; }
        public enum TypeClassification
        {
            seven, thirteen, sixteen, eighteen
        }
        public TypeClassification Classification { get; set; }
        public DateTime CreationDate { get; set; }
        public int CategoryId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "the name is required")]
        [MaxLength(60, ErrorMessage = "the maximum number of characters is 60")]
        public string Name { get; set; } = string.Empty;
    }
}

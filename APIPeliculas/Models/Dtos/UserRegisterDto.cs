using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "User is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}

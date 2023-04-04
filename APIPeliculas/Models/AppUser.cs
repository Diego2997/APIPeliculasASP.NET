using Microsoft.AspNetCore.Identity;

namespace APIPeliculas.Models
{
    public class AppUser : IdentityUser
    {
        //Añadir campos personalizados
        public string Nombre { get; set; }
    }
}

using APIPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPeliculas.Data
{
    public class APIContext :  DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}

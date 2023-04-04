using APIPeliculas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIPeliculas.Data
{
    public class APIContext :  IdentityDbContext<AppUser>
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}

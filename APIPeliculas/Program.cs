
using APIPeliculas.Data;
using APIPeliculas.FilmsMapperr;
using APIPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using APIPeliculas.Repository;

namespace APIPeliculas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            const string CONNECTIONNAME = "ConexionSql";
            string connectionString = builder.Configuration.GetConnectionString(CONNECTIONNAME);
            builder.Services.AddDbContext<APIContext>(options => options.UseSqlServer(connectionString));

            //Add repositories
            builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();



            //Add AutoMapper
            builder.Services.AddAutoMapper(typeof(FilmsMapper));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Villas.API.Data;
using Villas.API.Repositories;

namespace Villas.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
            builder.Services.AddDbContext<VillaDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IVillaRepository, VillaRepository>();

            builder.Services.AddOpenApi();
            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

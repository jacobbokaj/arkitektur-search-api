using SearchProgamModul3.Server.Models;

namespace arkitekturSearchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<IDatabaseRepository, DatabaseDBContext>();
            // Add services to the container.

            builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

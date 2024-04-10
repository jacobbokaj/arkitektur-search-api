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
          //  builder.Services.AddReverseProxy().
            //    LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            builder.Services.AddHealthChecks();
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

        //    app.MapReverseProxy();
          //  app.MapHealthChecks("health");


            app.MapControllers();

            app.Run();
        }
    }
}

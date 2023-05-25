using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using WebApplication1.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        //var sqlConnection = builder.Configuration["ConnectionString:MyDbConnection"];
       // builder.Services.AddSqlServer<SecretContext>(sqlConnection, options => options.EnableRetryOnFailure());
        builder.Services.AddDbContext<SecretContext>(opt => opt.UseSqlServer("name=MyDbConnection"));//ha ez kommentelem �sa Contextben kiveszem a kommentet majd a 25. sort kommentelem akkor a lok�lis adatb�zissal komunik�l
      //  builder.Services.AddDbContext<SecretContext>(opt => opt.UseMySql("name=secrets"));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "Secret Server",
                Description = "This is an API of a secret service. You can save your secret by using the API. You can restrict the access of a secret after the certen number of views or after a certen period of time.",
            });
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
      //  if (app.Environment.IsDevelopment())
       // {
            app.UseSwagger();
            app.UseSwaggerUI();
       // }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}
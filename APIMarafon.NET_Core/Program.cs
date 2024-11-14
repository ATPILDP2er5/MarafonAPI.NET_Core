using APIMarafon.NET_Core.Models;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoApi.Models.MARAFON_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//void ConfigureServices(IServiceCollection services)
//{
//    services.AddDbContext<MARAFON_DBContext>(options => options.UseSqlServer("Server=213.155.192.79,3002;Database=MARAFON_DB;User_Id=u23dmitriev;Password=be3v;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;"));
//}
static void Main(string[] args)
{
    CreateHostBuilder(args).Build().Run();
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Program>();
        });

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

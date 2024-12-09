using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Services;
using System.Text.Json.Serialization;
using APICatalogo.Extensions;
using APICatalogo.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExcepitonFilter));
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obter a string de conex�o
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddTransient<IMeuServico, MeuServico>();

builder.Services.AddScoped<ApiLogginFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

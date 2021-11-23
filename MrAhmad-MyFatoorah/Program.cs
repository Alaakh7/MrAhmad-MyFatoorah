using Microsoft.EntityFrameworkCore;
using Models;
using MrAhmad_MyFatoorah.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Initial Config
Config.SetJeson();
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(Config.GetConnectionString()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Initial DB
var context = Config.GetContext();
context.Database.EnsureCreated();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

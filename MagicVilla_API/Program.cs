using MagicVilla_API;
using MagicVilla_API.Data;
using MagicVilla_API.Repositories;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ADD SERVICES TO THE CONTAINER.

builder.Services.AddControllers().AddNewtonsoftJson();

// Inyección de nuestro servicio referente al VillaContext, para poder usarlo en el controlador
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Inyección de nuestro servicio Mapper para mapear los DTOs, para poder usarlo en el controlador
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IVillaRepository, VillaRepository>(); // Existen también el AddTranscient y el AddSingleton

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

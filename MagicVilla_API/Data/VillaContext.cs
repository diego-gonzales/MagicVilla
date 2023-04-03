using MagicVilla_API.Models;
using MagicVilla_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data;

public class VillaContext : DbContext
{
    // el : base le indicamos que le vamos a pasar toda la configuración de la base de datos a la clase base DbContext.
    public VillaContext(DbContextOptions<VillaContext> options) : base(options)
    {
    }
    public DbSet<Villa> Villas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Villa>().HasData(
            new Villa() { Id = 1, Name = "Villa 1", Detail = "Villa detail 1", Fee = 1, ImageUrl = "", Amenity = "Amenity 1", Occupants = 10, SquareMeters = 80.5F, CreationDate = DateTime.Now, UpdateDate = DateTime.Now },
            new Villa() { Id = 2, Name = "Villa 2", Detail = "Villa detail 2", Fee = 2, ImageUrl = "", Amenity = "Amenity 2", Occupants = 20, SquareMeters = 200, CreationDate = DateTime.Now, UpdateDate = DateTime.Now },
            new Villa() { Id = 3, Name = "Villa 3", Detail = "Villa detail 3", Fee = 3, ImageUrl = "", Amenity = "Amenity 3", Occupants = 30, SquareMeters = 300, CreationDate = DateTime.Now, UpdateDate = DateTime.Now },
            new Villa() { Id = 4, Name = "Villa 4", Detail = "Villa detail 4", Fee = 4, ImageUrl = "", Amenity = "Amenity 4", Occupants = 40, SquareMeters = 400, CreationDate = DateTime.Now, UpdateDate = DateTime.Now },
            new Villa() { Id = 5, Name = "Villa 5", Detail = "Villa detail 5", Fee = 5, ImageUrl = "", Amenity = "Amenity 5", Occupants = 50, SquareMeters = 500, CreationDate = DateTime.Now, UpdateDate = DateTime.Now }
        );
    }
}

using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTOs;

public class VillaUpdateDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string? Name { get; set; }

    public string Detail { get; set; }

    [Required]
    public float Fee { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    public string Amenity { get; set; }

    [Required]
    public int Occupants { get; set; }

    [Required]
    public float SquareMeters { get; set; }
}

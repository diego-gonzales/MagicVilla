using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTOs;

public class VillaNumberCreateDTO
{
    [Required]
    public int VillaNro { get; set; }

    [Required]
    public int VillaId { get; set; }

    public string? Descripcion { get; set; }
}
using MagicVilla_API.Models.DTOs;

namespace MagicVilla_API.Data;

public static class VillaStore
{
    public static List<VillaDTO> villaList = new List<VillaDTO>
    {
        new VillaDTO { Id = 1, Name = "Villa 1", Occupants = 10, SquareMeters = 80.5F },
        new VillaDTO { Id = 2, Name = "Villa 2", Occupants = 20, SquareMeters = 200 },
        new VillaDTO { Id = 3, Name = "Villa 3", Occupants = 30, SquareMeters = 300 },
        new VillaDTO { Id = 4, Name = "Villa 4", Occupants = 40, SquareMeters = 400 },
        new VillaDTO { Id = 5, Name = "Villa 5", Occupants = 50, SquareMeters = 500 },
    };
}

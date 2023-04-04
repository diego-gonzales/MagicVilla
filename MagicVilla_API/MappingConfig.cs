using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTOs;

namespace MagicVilla_API;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        // <fuente, destino>
        CreateMap<Villa, VillaDTO>();
        CreateMap<VillaDTO, Villa>();

        CreateMap<Villa, VillaCreateDTO>().ReverseMap(); // Es lo mismo que lo anterior pero en una sola línea.
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
    }
}

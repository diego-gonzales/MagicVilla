using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaController : ControllerBase
{
    private readonly ILogger<VillaController> _logger; // Sirve para mostrar logs en consola
    private readonly VillaContext _context;
    private readonly IMapper _mapper;

    public VillaController(ILogger<VillaController> logger, VillaContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        _logger.LogInformation("Getting villas");

        IEnumerable<Villa> villaList = await _context.Villas.ToListAsync();

        // el mapper básicamente me permite mapear de una fuente a un destino (en este caso una lista de Villa a una lista de VillaDTO)
        return Ok(_mapper.Map<IEnumerable<VillaDTO>>(villaList));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<VillaDTO>> GetVilla(int id)
    {
        if (id == 0)
        {
            _logger.LogError("Error to get villa with id 0");
            return BadRequest();
        }

        // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);

        return villa != null ? Ok(_mapper.Map<VillaDTO>(villa)) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<VillaCreateDTO>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
    {
        if (!ModelState.IsValid) return BadRequest();

        if (await _context.Villas.FirstOrDefaultAsync(v => v.Name!.ToLower() == villaCreateDTO.Name!.ToLower()) != null)
        {
            ModelState.AddModelError("NameExists", $"Villa with the {villaCreateDTO.Name} name already exists");
            return BadRequest(ModelState);
        }

        if (villaCreateDTO == null) return BadRequest();

        // Código reemplazado por el uso del _mapper
        /*
        Villa newVilla = new()
        {
            Name = villaCreateDTO.Name,
            Detail = villaCreateDTO.Detail,
            Fee = villaCreateDTO.Fee,
            ImageUrl = villaCreateDTO.ImageUrl,
            Amenity = villaCreateDTO.Amenity,
            Occupants = villaCreateDTO.Occupants,
            SquareMeters = villaCreateDTO.SquareMeters,
        };*/

        Villa newVilla = _mapper.Map<Villa>(villaCreateDTO);

        _context.Villas.Add(newVilla);
        await _context.SaveChangesAsync();

        // el 3 parámetro lo puedo enviar según el DTO, para no mostrar datos innecesarios (usar método villaToVillaDTO() que creé más abajo, o usar _mapper.Map<VillaDTO(newVilla)>)
        return CreatedAtAction(nameof(GetVilla), new { id = newVilla.Id }, newVilla);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteVilla(int id)
    {
        if (id == 0) return BadRequest();

        var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

        if (villa == null) return NotFound();

        _context.Villas.Remove(villa);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
    {
        if (villaUpdateDTO == null || id != villaUpdateDTO.Id) return BadRequest();

        Villa updatedVilla = _mapper.Map<Villa>(villaUpdateDTO);

        _context.Villas.Update(updatedVilla);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0) return BadRequest();

        var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id); // Se usa asNoTracking() method

        if (villa == null) return NotFound();

        VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

        patchDTO.ApplyTo(villaDTO, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        Villa updatedVilla = _mapper.Map<Villa>(villaDTO);

        _context.Villas.Update(updatedVilla);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /*
    private static VillaDTO villaToVillaDTO(Villa villa) =>
       new()
       {
           Id = villa.Id,
           Name = villa.Name,
           Detail = villa.Detail,
           Fee = villa.Fee,
           ImageUrl = villa.ImageUrl,
           Amenity = villa.Amenity,
           Occupants = villa.Occupants,
           SquareMeters = villa.SquareMeters
       }; */
}


// IEnumerable es para cuando devolvemos una lista.
// ActionResult<T> me va a permitir devolver múltiples códigos de estado dentro de mi método, además que me permite devolver lo que coloque como genérico.
// IActionResult hace lo mismo que el anterior pero como no devuelvo nada entonces es usado, se usa mayormente para el DELETE y PUT (devuelvo un NoContent()).
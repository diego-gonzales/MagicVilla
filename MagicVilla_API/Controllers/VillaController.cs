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

    public VillaController(ILogger<VillaController> logger, VillaContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        _logger.LogInformation("Getting villas");
        return Ok(_context.Villas.ToList());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public ActionResult<VillaDTO> GetVilla(int id)
    {
        if (id == 0)
        {
            _logger.LogError("Error to get villa with id 0");
            return BadRequest();
        }

        // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        var villa = _context.Villas.FirstOrDefault(v => v.Id == id);

        return villa != null ? Ok(villa) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
    {
        if (!ModelState.IsValid) return BadRequest();

        if (_context.Villas.FirstOrDefault(v => v.Name!.ToLower() == villaDTO.Name!.ToLower()) != null)
        {
            ModelState.AddModelError("NameExists", $"Villa with the {villaDTO.Name} name already exists");
            return BadRequest(ModelState);
        }

        if (villaDTO == null) return BadRequest();

        if (villaDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

        Villa villa = new()
        {
            Name = villaDTO.Name,
            Detail = villaDTO.Detail,
            Fee = villaDTO.Fee,
            ImageUrl = villaDTO.ImageUrl,
            Amenity = villaDTO.Amenity,
            Occupants = villaDTO.Occupants,
            SquareMeters = villaDTO.SquareMeters,
        };

        _context.Villas.Add(villa);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetVilla), new { id = villaDTO.Id }, villaDTO);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteVilla(int id)
    {
        if (id == 0) return BadRequest();

        var villa = _context.Villas.FirstOrDefault(x => x.Id == id);

        if (villa == null) return NotFound();

        _context.Villas.Remove(villa);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
    {
        if (villaDTO == null || id != villaDTO.Id) return BadRequest();

        var villa = _context.Villas.FirstOrDefault(v => v.Id == id);

        if(villa == null) return NotFound();

        villa.Name = villaDTO.Name;
        villa.Detail = villaDTO.Detail;
        villa.Amenity = villaDTO.Amenity;
        villa.ImageUrl = villaDTO.ImageUrl;
        villa.Fee = villaDTO.Fee;
        villa.Occupants = villaDTO.Occupants;
        villa.SquareMeters = villaDTO.SquareMeters;

        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult PatchVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
    {
        if (patchDTO == null || id == 0) return BadRequest();

        var villa = _context.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id); // Se usa asNoTracking() method

        if (villa == null) return NotFound();

        VillaDTO villaDTO = new()
        {
            Name = villa.Name,
            Detail = villa.Detail,
            Fee = villa.Fee,
            ImageUrl = villa.ImageUrl,
            Amenity = villa.Amenity,
            Occupants = villa.Occupants,
            SquareMeters = villa.SquareMeters
        };

        patchDTO.ApplyTo(villaDTO, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        Villa updatedVilla = new()
        {
            Name = villaDTO.Name,
            Detail = villaDTO.Detail,
            Fee = villaDTO.Fee,
            ImageUrl = villaDTO.ImageUrl,
            Amenity = villaDTO.Amenity,
            Occupants = villaDTO.Occupants,
            SquareMeters = villaDTO.SquareMeters
        };

        _context.Update(updatedVilla);
        _context.SaveChanges();

        return NoContent();
    }
}


// IEnumerable es para cuando devolvemos una lista.
// ActionResult<T> me va a permitir devolver múltiples códigos de estado dentro de mi método, además que me permite devolver lo que coloque como genérico.
// IActionResult hace lo mismo que el anterior pero como no devuelvo nada entonces es usado, se usa mayormente para el DELETE y PUT (devuelvo un NoContent()).
using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTOs;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaController : ControllerBase
{
    private readonly ILogger<VillaController> _logger; // Sirve para mostrar logs en consola
    //private readonly VillaContext _context;
    private readonly IVillaRepository _villaRepository;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepository, IMapper mapper)
    {
        _logger = logger;
        //_context = context;
        _villaRepository = villaRepository;
        _mapper = mapper;
        _response = new();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            _logger.LogInformation("Getting villas");

            IEnumerable<Villa> villaList = await _villaRepository.GetAll();

            _response.Result = _mapper.Map<IEnumerable<VillaDTO>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;

            // el mapper básicamente me permite mapear de una fuente a un destino (en este caso una lista de Villa a una lista de VillaDTO)
            return Ok(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccessfull = false;
            _response.ErrorMessages = new List<string>() { exception.ToString() };
        }

        return _response;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError("Error to get villa with id 0");
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _villaRepository.Get(v => v.Id == id);

            if (villa == null)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccessfull = false;
            _response.ErrorMessages = new List<string>() { exception.ToString() };
        }

        return _response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await _villaRepository.Get(v => v.Name!.ToLower() == villaCreateDTO.Name!.ToLower()) != null)
            {
                ModelState.AddModelError("NameExists", $"Villa with the {villaCreateDTO.Name} name already exists");
                return BadRequest(ModelState);
            }

            if (villaCreateDTO == null) return BadRequest(villaCreateDTO);

            Villa newVilla = _mapper.Map<Villa>(villaCreateDTO);
            newVilla.CreationDate = DateTime.Now;
            newVilla.UpdateDate = DateTime.Now;

            await _villaRepository.Create(newVilla);
            //await _villaRepository.Save(); // Esto ya no es necesario porque el método Create ya hace el Save
            _response.Result = newVilla;
            _response.StatusCode = HttpStatusCode.Created;

            // el 3 parámetro lo puedo enviar según el DTO, para no mostrar datos innecesarios (usar método villaToVillaDTO() que creé más abajo, o usar _mapper.Map<VillaDTO(newVilla)>)
            return CreatedAtAction(nameof(GetVilla), new { id = newVilla.Id }, _response);
        }
        catch (Exception exception)
        {
            _response.IsSuccessfull = false;
            _response.ErrorMessages = new List<string> { exception.ToString() };
        }

        return _response;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _villaRepository.Get(x => x.Id == id);

            if (villa == null)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _villaRepository.Remove(villa);
            //await _villaRepository.Save(); // Esto ya no es necesario porque el método Remove ya hace el Save
            _response.StatusCode = HttpStatusCode.NoContent;

            //return NoContent();
            return Ok(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccessfull = false;
            _response.ErrorMessages = new List<string>() { exception.ToString() };
        }

        return BadRequest(_response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
    {
        if (villaUpdateDTO == null || id != villaUpdateDTO.Id)
        {
            _response.IsSuccessfull = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        Villa updatedVilla = _mapper.Map<Villa>(villaUpdateDTO);

        await _villaRepository.Update(updatedVilla);
        //await _villaRepository.Save(); // Esto ya no es necesario porque el método Update ya hace el Save
        _response.StatusCode = HttpStatusCode.NoContent;

        return Ok(_response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            _response.IsSuccessfull = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        var villa = await _villaRepository.Get(v => v.Id == id, false); // Se usa asNoTracking() method para que no se haga un seguimiento de los cambios en la entidad

        if (villa == null)
        {
            _response.IsSuccessfull = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }

        VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

        patchDTO.ApplyTo(villaDTO, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        Villa updatedVilla = _mapper.Map<Villa>(villaDTO);

        await _villaRepository.Update(updatedVilla);
        //await _villaRepository.Save(); // Esto ya no es necesario porque el método Update ya hace el Save
        _response.StatusCode = HttpStatusCode.NoContent;

        return Ok(_response);
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
// ActionResult<T> me va a permitir devolver múltiples status codes dentro de mi método, además que me permite devolver lo que coloque como genérico.
// IActionResult hace lo mismo que el anterior pero como no devuelve nada ya que no acepta genéricos, se usa mayormente para el DELETE y PUT (devuelvo un NoContent()).
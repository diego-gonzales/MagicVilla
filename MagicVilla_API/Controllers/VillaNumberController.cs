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
public class VillaNumberController : ControllerBase
{
    private readonly ILogger<VillaNumberController> _logger;
    private readonly IVillaNumberRepository _villaNumberRepository;
    private readonly IVillaRepository _villaRepository;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public VillaNumberController(
        ILogger<VillaNumberController> logger,
        IVillaRepository villaRepository,
        IVillaNumberRepository villaNumberRepository,
        IMapper mapper
    )
    {
        _logger = logger;
        _villaRepository = villaRepository;
        _villaNumberRepository = villaNumberRepository;
        _mapper = mapper;
        _response = new();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillaNumbers()
    {
        try
        {
            _logger.LogInformation("Getting villas number");

            IEnumerable<VillaNumber> villaNumberList = await _villaNumberRepository.GetAll();

            _response.Result = _mapper.Map<IEnumerable<VillaNumberDTO>>(villaNumberList);
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

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError("Error to get villa number with id 0");
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await _villaNumberRepository.Get(vn => vn.VillaNro == id);

            if (villaNumber == null)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest();

            if (await _villaNumberRepository.Get(vn => vn.VillaNro == villaNumberCreateDTO.VillaNro) != null)
            {
                ModelState.AddModelError("NameExists", $"Villa with the {villaNumberCreateDTO.VillaNro} number already exists");
                return BadRequest(ModelState);
            }

            if (await _villaRepository.Get(v => v.Id == villaNumberCreateDTO.VillaId) == null)
            {
                ModelState.AddModelError("VillaId", $"Villa with the {villaNumberCreateDTO.VillaId} id does not exists");
                return BadRequest(ModelState);
            }

            if (villaNumberCreateDTO == null) return BadRequest(villaNumberCreateDTO);

            VillaNumber newVillaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);
            newVillaNumber.CreationDate = DateTime.Now;
            newVillaNumber.UpdateDate = DateTime.Now;

            await _villaNumberRepository.Create(newVillaNumber);
            _response.Result = newVillaNumber;
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtAction(nameof(GetVillaNumber), new { id = newVillaNumber.VillaNro }, _response);
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
    public async Task<IActionResult> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await _villaNumberRepository.Get(vn => vn.VillaNro == id);

            if (villaNumber == null)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _villaNumberRepository.Remove(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;

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
    public async Task<IActionResult> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
    {
        if (villaNumberUpdateDTO == null || id != villaNumberUpdateDTO.VillaNro)
        {
            _response.IsSuccessfull = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        if (await _villaRepository.Get(v => v.Id == villaNumberUpdateDTO.VillaId) == null)
        {
            ModelState.AddModelError("VillaId", $"Villa with the {villaNumberUpdateDTO.VillaId} id does not exists");
            return BadRequest(ModelState);
        }

        VillaNumber updatedVillaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);

        await _villaNumberRepository.Update(updatedVillaNumber);
        _response.StatusCode = HttpStatusCode.NoContent;

        return Ok(_response);
    }

    /*
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PatchVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            _response.IsSuccessfull = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        var villaNumber = await _villaNumberRepository.Get(vn => vn.VillaNro == id, false);

        if (villaNumber == null)
        {
            _response.IsSuccessfull = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }

        VillaNumberUpdateDTO villaNumberDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

        patchDTO.ApplyTo(villaNumberDTO, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        VillaNumber updatedVillaNumber = _mapper.Map<VillaNumber>(villaNumberDTO);

        await _villaNumberRepository.Update(updatedVillaNumber);
        _response.StatusCode = HttpStatusCode.NoContent;

        return Ok(_response);
    } */
}

using dotNetRPG.DTO.Character;
using dotNetRPG.Models;
using dotNetRPG.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace dotNetRPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> GetAsync()
        {
            return Ok(await _characterService.GetAllCharactersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingleAsync(int id)
        {
            return Ok(await _characterService.GetCharacterByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacterAsync(AddCharacterDTO character)
        {
            return Ok(await _characterService.AddCharacterAsync(character));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> UpdateCharacterAsync(UppdateCharacterDTO uppdateCharacterDTO)
        {
            var serviceResponse = await _characterService.UpdateCharacterAsync(uppdateCharacterDTO);
            if (serviceResponse.Data == null)
                return NotFound(serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> DeleteCharacterAsync(int id)
        {
            var serviceResponse = await _characterService.DeleteCharacterAsync(id);
            if (serviceResponse.Data == null)
                return NotFound(serviceResponse);

            return Ok(serviceResponse);
        }
    }
}

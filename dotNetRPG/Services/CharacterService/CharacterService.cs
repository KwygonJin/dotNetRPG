using AutoMapper;
using dotNetRPG.Data;
using dotNetRPG.DTO.Character;
using dotNetRPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetRPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacterAsync(AddCharacterDTO newCharacter)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharactersAsync()
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            serviceResponse.Success = serviceResponse.Data != null;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterByIdAsync(int id)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacterAsync(UppdateCharacterDTO uppdateCharacterDTO)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == uppdateCharacterDTO.Id);

                character.Name = uppdateCharacterDTO.Name;
                character.HitPoints = uppdateCharacterDTO.HitPoints;
                character.Strength = uppdateCharacterDTO.Strength;
                character.Defence = uppdateCharacterDTO.Defence;
                character.Intelligence = uppdateCharacterDTO.Intelligence;
                character.Class = uppdateCharacterDTO.Class;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacterAsync(int id)
        {
            ServiceResponse< List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                Character character = await _context.Characters.FirstAsync(c => c.Id == id);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}

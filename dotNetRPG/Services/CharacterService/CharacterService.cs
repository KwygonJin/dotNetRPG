using AutoMapper;
using dotNetRPG.Data;
using dotNetRPG.DTO.Character;
using dotNetRPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotNetRPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacterAsync(AddCharacterDTO newCharacter)
        {
            int userId = GetUserId();
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters
                .Where(c => c.User.Id == userId)
                .Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharactersAsync()
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            serviceResponse.Success = serviceResponse.Data != null;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterByIdAsync(int id)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var dbCharacter = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacterAsync(UppdateCharacterDTO uppdateCharacterDTO)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                Character character = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == uppdateCharacterDTO.Id);
                if (character.User.Id == GetUserId())
                {
                    character.Name = uppdateCharacterDTO.Name;
                    character.HitPoints = uppdateCharacterDTO.HitPoints;
                    character.Strength = uppdateCharacterDTO.Strength;
                    character.Defence = uppdateCharacterDTO.Defence;
                    character.Intelligence = uppdateCharacterDTO.Intelligence;
                    character.Class = uppdateCharacterDTO.Class;

                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found!";
                }
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
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = await _context.Characters
                        .Where(c => c.User.Id == GetUserId())
                        .Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found!";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkillAsync(AddCharacterSkillDTO newCharacterSkill)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());
                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found!";
                    return serviceResponse;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Skill not found!";
                    return serviceResponse;
                }

                character.Skills.Add(skill);
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
    }
}

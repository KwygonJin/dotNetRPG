using dotNetRPG.DTO.Character;
using dotNetRPG.Models;

namespace dotNetRPG.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharactersAsync();
        Task<ServiceResponse<GetCharacterDTO>> GetCharacterByIdAsync(int id);
        Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacterAsync(AddCharacterDTO newCharacter);
        Task<ServiceResponse<GetCharacterDTO>> UpdateCharacterAsync(UppdateCharacterDTO uppdateCharacterDTO);
        Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacterAsync(int id);
        Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkillAsync(AddCharacterSkillDTO newCharacterSkill);

    }
}

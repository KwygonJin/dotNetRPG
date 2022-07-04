using dotNetRPG.DTO.Fight;
using dotNetRPG.Models;

namespace dotNetRPG.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDTO>> WeaponAttackAsync(WeaponAttackDTO weaponAttack);
        Task<ServiceResponse<AttackResultDTO>> SkillAttackAsync(SkillAttackDTO skillAttack);
        Task<ServiceResponse<FightResultDTO>> FightAsync(FightRequestDTO fightRequest);
        Task<ServiceResponse<List<HighscoreDTO>>> HighscoreAsync();
    }
}

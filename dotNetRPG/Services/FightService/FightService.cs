using AutoMapper;
using dotNetRPG.Data;
using dotNetRPG.DTO.Fight;
using dotNetRPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotNetRPG.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FightService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<AttackResultDTO>> WeaponAttackAsync(WeaponAttackDTO weaponAttack)
        {
            ServiceResponse<AttackResultDTO> serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                var characterAttacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == weaponAttack.AttackerId);

                if (characterAttacker == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Attacker not found";
                    return serviceResponse;
                }

                var characterOpponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == weaponAttack.OpponentId);

                if (characterOpponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Opponent not found";
                    return serviceResponse;
                }

                int damage = DoWeaponAttack(characterAttacker, characterOpponent);

                if (characterOpponent.HitPoints <= 0)
                    serviceResponse.Message = $"{characterOpponent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = characterAttacker.Name,
                    AttackerHP = characterAttacker.HitPoints,
                    Opponent = characterOpponent.Name,
                    OpponentHP = characterOpponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        private static int DoWeaponAttack(Character? characterAttacker, Character? characterOpponent)
        {
            int damage = characterAttacker.Weapon.Damage + (new Random().Next(characterAttacker.Strength));
            damage -= (new Random().Next(characterOpponent.Defence));

            if (damage > 0)
                characterOpponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<AttackResultDTO>> SkillAttackAsync(SkillAttackDTO skillAttack)
        {
            ServiceResponse<AttackResultDTO> serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                var characterAttacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == skillAttack.AttackerId);

                if (characterAttacker == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Attacker not found";
                    return serviceResponse;
                }

                Skill skill = characterAttacker.Skills.FirstOrDefault(s => s.Id == skillAttack.SkillId);
                if (skill == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Skill not found";
                    return serviceResponse;
                }

                var characterOpponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == skillAttack.OpponentId);

                if (characterOpponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Opponent not found";
                    return serviceResponse;
                }

                int damage = DoSkillAttack(characterAttacker, skill, characterOpponent);

                if (characterOpponent.HitPoints <= 0)
                    serviceResponse.Message = $"{characterOpponent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = characterAttacker.Name,
                    AttackerHP = characterAttacker.HitPoints,
                    Opponent = characterOpponent.Name,
                    OpponentHP = characterOpponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        private static int DoSkillAttack(Character? characterAttacker, Skill skill, Character? characterOpponent)
        {
            int damage = skill.Damage + (new Random().Next(characterAttacker.Intelligence));
            damage -= (new Random().Next(characterOpponent.Defence));

            if (damage > 0)
                characterOpponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<FightResultDTO>> FightAsync(FightRequestDTO fightRequest)
        {
            ServiceResponse<FightResultDTO> serviceResponse = new ServiceResponse<FightResultDTO>
            {
                Data = new FightResultDTO()
            };

            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => fightRequest.CharacterIds.Contains(c.Id)).ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if(useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, skill, opponent);
                        }

                        serviceResponse.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)}");

                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;

                            serviceResponse.Data.Log.Add($"{opponent.Name} nas been defeated!");
                            serviceResponse.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
                            break;
                        }
                    }
                }

                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<HighscoreDTO>>> HighscoreAsync()
        {
            ServiceResponse<List<HighscoreDTO>> serviceResponse = new ServiceResponse<List<HighscoreDTO>>();

            try
            {
                var characters = await _context.Characters
                    .Where(c => c.Fights > 0)
                    .OrderByDescending(c => c.Victories)
                    .ThenBy(c => c.Defeats)
                    .ToListAsync();

                serviceResponse.Data = characters.Select(c => _mapper.Map<HighscoreDTO>(c)).ToList();
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

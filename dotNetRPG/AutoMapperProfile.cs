using AutoMapper;
using dotNetRPG.DTO.Character;
using dotNetRPG.DTO.Fight;
using dotNetRPG.DTO.Skill;
using dotNetRPG.DTO.Weapon;
using dotNetRPG.Models;

namespace dotNetRPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO, Character>();
            CreateMap<Weapon, GetWeaponDTO>();
            CreateMap<Skill, GetSkillDTO>();
            CreateMap<Character, HighscoreDTO>();
        }
    }
}

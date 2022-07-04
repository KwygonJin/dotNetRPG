using dotNetRPG.DTO.Skill;
using dotNetRPG.DTO.Weapon;
using dotNetRPG.Models;

namespace dotNetRPG.DTO.Character
{
    public class GetCharacterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Intelligence { get; set; }
        public RpgClass Class { get; set; }
        public GetWeaponDTO weapon { get; set; }
        public List<GetSkillDTO> Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}

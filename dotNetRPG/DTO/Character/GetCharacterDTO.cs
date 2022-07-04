﻿using dotNetRPG.DTO.Skill;
using dotNetRPG.DTO.Weapon;
using dotNetRPG.Models;

namespace dotNetRPG.DTO.Character
{
    public class GetCharacterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public GetWeaponDTO weapon { get; set; }
        public List<GetSkillDTO> Skills { get; set; }
    }
}

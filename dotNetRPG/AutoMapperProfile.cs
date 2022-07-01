using AutoMapper;
using dotNetRPG.DTO.Character;
using dotNetRPG.Models;

namespace dotNetRPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO, Character>();
        }
    }
}

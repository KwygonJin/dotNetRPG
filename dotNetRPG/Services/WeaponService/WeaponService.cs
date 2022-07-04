using AutoMapper;
using dotNetRPG.Data;
using dotNetRPG.DTO.Character;
using dotNetRPG.DTO.Weapon;
using dotNetRPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotNetRPG.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeaponService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<ServiceResponse<GetCharacterDTO>> AddWeaponAsync(AddWeaponDTO newWeapon)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && 
                    c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found";
                    return serviceResponse;
                }

                Weapon weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };
                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}

using dotNetRPG.DTO.Character;
using dotNetRPG.DTO.Weapon;
using dotNetRPG.Models;

namespace dotNetRPG.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDTO>> AddWeaponAsync(AddWeaponDTO newWeapon);
    }
}

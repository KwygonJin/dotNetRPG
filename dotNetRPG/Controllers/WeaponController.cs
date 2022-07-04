using dotNetRPG.DTO.Character;
using dotNetRPG.DTO.Weapon;
using dotNetRPG.Models;
using dotNetRPG.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotNetRPG.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddWeaponAsync(AddWeaponDTO newWeapon)
        {
            return Ok(await _weaponService.AddWeaponAsync(newWeapon));
        }
    }
}

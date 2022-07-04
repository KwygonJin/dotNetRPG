using dotNetRPG.DTO.Fight;
using dotNetRPG.Models;
using dotNetRPG.Services.FightService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotNetRPG.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> WeaponAttackAsync(WeaponAttackDTO weaponAttack)
        {
            return Ok(await _fightService.WeaponAttackAsync(weaponAttack));
        }

        [HttpPost("skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> SkillAttackAsync(SkillAttackDTO skillAttack)
        {
            return Ok(await _fightService.SkillAttackAsync(skillAttack));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightResultDTO>>> FightAsync(FightRequestDTO fightRequest)
        {
            return Ok(await _fightService.FightAsync(fightRequest));
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<HighscoreDTO>>>> HighscoreAsync()
        {
            return Ok(await _fightService.HighscoreAsync());
        }
    }
}

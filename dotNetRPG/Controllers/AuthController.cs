using dotNetRPG.Data;
using dotNetRPG.DTO.User;
using dotNetRPG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotNetRPG.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> RegisterAsync(UserRegisterDTO userRegister)
        {
            var response = await _authRepository.RegisterAsync(
                    new User { Username = userRegister.UserName }, userRegister.Password 
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> LoginAsync(UserLoginDTO userLogin)
        {
            var response = await _authRepository.LoginAsync(userLogin.UserName, userLogin.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

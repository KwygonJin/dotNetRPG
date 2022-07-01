using dotNetRPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetRPG.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<string>> LoginAsync(string userName, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(userName.ToLower()));
            if(user == null)
            {
                response.Success = false;
                response.Message = "User not found!";
                return response;
            }
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswrodSalt))
            {
                response.Success = false;
                response.Message = "Wrong password!";
                return response;
            }
            else
            {
                response.Data = user.Id.ToString();
                return response;
            }
        }

        public async Task<ServiceResponse<int>> RegisterAsync(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if(await UserExistsAsync(user.Username))
            {
                response.Success = false;
                response.Message = "User alreeady exists!";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswrodSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            if(await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(userName.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length ; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

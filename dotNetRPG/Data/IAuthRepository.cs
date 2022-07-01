using dotNetRPG.Models;

namespace dotNetRPG.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> RegisterAsync(User user, string password);
        Task<ServiceResponse<string>> LoginAsync(string userName, string password);
        Task<bool> UserExistsAsync(string userName);
    }
}

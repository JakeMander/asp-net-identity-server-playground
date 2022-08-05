using Microsoft.AspNetCore.Identity;

namespace Creating_A_User
{
    public interface IUserCreationService
    {
        Task<IdentityResult> CreateUserAsync(string username, string password, string email);
    }
}
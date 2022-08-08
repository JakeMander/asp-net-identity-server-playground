using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Creating_A_User
{
    public interface IUserCreationService
    {
        Task<bool> CheckUserPassword(string username, string passwordPlaintext);
        Task<IdentityResult> CreateUserAsync(string username, string password, string email);

        Task<IdentityResult> AssignUserClaim(string userId, Claim userClaim);
    }
}
using Microsoft.AspNetCore.Identity;

namespace Creating_A_User
{
    public class UserCreationService : IUserCreationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserCreationService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateUserAsync(string username, string password, string email)
        {
            var user = new IdentityUser(username);
            user.Email = email;
            user.EmailConfirmed = false;

            var result = await _userManager.CreateAsync(user, password);
            return result;
        }
    }
}
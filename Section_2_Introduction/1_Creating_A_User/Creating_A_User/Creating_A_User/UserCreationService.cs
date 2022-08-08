using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Creating_A_User
{
    public class UserCreationService : IUserCreationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserCreationService(UserManager<IdentityUser> userManager)
        {
            if (userManager == null)
            {
                throw new NullReferenceException("An Null User Manager Was Injected To The User Service");
            }
            _userManager = userManager;
        }

        public async Task<IdentityResult> AssignUserClaim(string username, Claim claimToAssign)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new NullReferenceException("Cannot Assign Claim In AssignUserClaim: Username Was Empty");
            }

            if (claimToAssign == null)
            {
                throw new NullReferenceException("Cannot Assign Claim In AssignUserClaim. Claim Was Null");
            }

            var user = await _userManager.FindByNameAsync(username);
            var claims = await _userManager.GetClaimsAsync(user);

            if (claims.Where(c => c.Type == claimToAssign.Type).SingleOrDefault() != null)
            {
                return IdentityResult.Failed(new IdentityError[]
                {
                    new IdentityError
                    {
                        Code = "0002",
                        Description = "Claim Type Already Exists"
                    }
                });
            }
            return await _userManager.AddClaimAsync(user, claimToAssign);
        }

        public async Task<bool> CheckUserPassword(string username, string passwordPlaintext)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new NullReferenceException("Could Not Check Password: Username Was Empty");
            }

            if (string.IsNullOrEmpty(passwordPlaintext))
            {
                throw new NullReferenceException("Could Not Check Password: Password Was Null");
            }
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new InvalidOperationException($"User {username} Could Not Be Found");
            }

            return await _userManager.CheckPasswordAsync(user, passwordPlaintext);
        }

        public async Task<IdentityResult> CreateUserAsync(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new NullReferenceException("No Username Was Provided.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new NullReferenceException("No Password Was Provided");
            }

            if (await _userManager.FindByNameAsync(username) != null)
            {
                return IdentityResult.Failed(
                    new IdentityError[]
                    {
                        new IdentityError
                        {
                            Code = "0001",
                            Description = "User Already Exists "
                        }
                    });
            }
            var user = new IdentityUser(username);
            user.Email = email;
            user.EmailConfirmed = false;

            var result = await _userManager.CreateAsync(user, password);
            return result;
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Creating_A_User.Identity;
using Microsoft.Extensions.Logging;

namespace Creating_A_User
{
    public class Program
    {
       private interface IUserCreationService
        {
            Task<IdentityResult> CreateUserAsync(string username, string password, string email);
        }

        private class UserCreationService : IUserCreationService
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
        public static async Task Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection();
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 7;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddLogging(configure => configure.AddConsole());

            services.AddScoped<IUserCreationService, UserCreationService>();

            //  Set up DI/IoC to set up our user class/inject all the behind the scenes
            //  identity classes.
            var provider = services.BuildServiceProvider();

            //  Retrieve the class that's been injected with all the Identity goodness.
            var userService = provider.GetService<IUserCreationService>();

            var userResult = await userService.CreateUserAsync("jakemander96", "jake123", 
                "jake@email.co.uk");

            Console.WriteLine((userResult.Succeeded) ? "Creation Successful" : "Creation Failed");

            return;
        }
    }
}
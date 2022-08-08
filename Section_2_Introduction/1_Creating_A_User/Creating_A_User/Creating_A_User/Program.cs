using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Creating_A_User.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
namespace Creating_A_User
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection();
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString),
                ServiceLifetime.Transient);

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

            services.AddTransient<IUserCreationService, UserCreationService>();

            //  Set up DI/IoC to set up our user class/inject all the behind the scenes
            //  identity classes.
            var provider = services.BuildServiceProvider();

            //  Retrieve the class that's been injected with all the Identity goodness.
            var userService = provider.GetService<IUserCreationService>();

            if (userService == null)
            {
                throw new NullReferenceException("User Service Was Null. Terminating Program");
            }

            var userResult = await userService.CreateUserAsync("jakemander96", "jake123", 
                "jake@email.co.uk");
            Console.WriteLine((userResult.Succeeded) ? "Creation Successful" : "Creation Failed: " +
                $"{userResult.Errors.First().Description}");

            var claimResult = await userService.AssignUserClaim("jakemander96", new Claim("given_name", "Jake"));
            Console.WriteLine((claimResult.Succeeded) ? "Claim Created" : "Claim Creation Failed: " +
                $"{claimResult.Errors.First().Description}");

            var passwordCheckResult = await userService.CheckUserPassword("jakemander96", "jake123");
            Console.WriteLine((passwordCheckResult) ? "Password Matched" : "Passwords Did Not Match");
            return;
        }
    }
}
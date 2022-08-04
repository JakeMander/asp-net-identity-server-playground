using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Creating_A_User
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            /*  
             *  UserManger is the entry point for ASP.NET Identity. It contains
             *  all of the CRUD methods for managing Identity data. It also
             *  contains methods for managing passwords, claims and roles for 
             *  users.
             *  
             *  All of these implementations are generic. It is possible to 
             *  define many user types that can be used with the UserManager.
             *  
             *  Here we'll be using the default user entity (SQL table)
             *  that comes the Identity  Entity Framework package. This is 
             *  "IdentityUser" which comes pre-defined.
             *  
             *  A UserManager takes in a single parameter of a "UserStore"
             *  which abstracts away the underlying storage mechanism
             *  (i.e. the database). 
             */

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            /*
             * Create Takes an IU and a password. Username is mandatory, password
             * is optional to allow or users who use external identity providers
             * or a certificate.
             * 
             * Create returns a creation result which contains a success bool and
             * a collection of errors (if something went wrong).
             */
            userManager.Create(new IdentityUser("jmmander1@hotmail.co.uk"), "jake123");
        }
    }
}
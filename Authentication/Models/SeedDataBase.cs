using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Authentication.Models
{
    public class SeedDataBase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            ApplicationContext context = serviceProvider.GetRequiredService<ApplicationContext>();
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            context.Database.EnsureCreated();

            if(!context.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "alberto@email.com",
                    UserName = "betoramiz",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                userManager.CreateAsync(user);
            }
        }
    }
}

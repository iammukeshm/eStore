using eStore.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Infrastructure.Identity.Context
{
    public static class IdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "mukesh",
                FirstName = "Mukesh",
                LastName = "Murugan",

                Email = "mukesh@estore.com",
                EmailConfirmed = true,
                PhoneNumber = "7306488721",
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "P@33word");
            }
        }
    }
}

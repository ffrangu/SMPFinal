using Microsoft.AspNetCore.Identity;
using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models
{
    public static class UserAndRoleDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("fitonfrangi@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "fitonfrangi@gmail.com";
                user.Email = "fitonfrangi@gmail.com";
                user.FirstName = "Fiton";
                user.LastName = "Frangu";

                IdentityResult result = userManager.CreateAsync(user, "123456789Aa#").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }


            if (userManager.FindByEmailAsync("gentgovori@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "gentgovori@gmail.com";
                user.Email = "gentgovori@gmail.com";
                user.FirstName = "Gent";
                user.LastName = "Govori";

                IdentityResult result = userManager.CreateAsync(user, "123456789Aa#").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("HR").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "HR";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }

}

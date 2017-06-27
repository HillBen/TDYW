using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDYW.Data;
using TDYW.Models;

/// <summary>
/// 
/// </summary>
namespace TDYW
{
    public class InitialData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            string[] roles = new string[] { "Owner", "Contributor", "Administrator", "Sponsor", "VIP", "Member" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            var user = new ApplicationUser
            {
                DisplayName = "Ben Hill",
                Email = "jerrod.benjamin@gmail.com",
                NormalizedEmail = "JERROD.BENJAMIN@GMAIL.COM",
                UserName = "jerrod.benjamin@gmail.com",
                NormalizedUserName = "JERROD.BENJAMIN@GMAIL.COM",
                PhoneNumber = "5555555555",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "Temp123!");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(user);

            }

            await AssignRoles(serviceProvider, user.Email, roles);

            await context.SaveChangesAsync();
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }

    }
}

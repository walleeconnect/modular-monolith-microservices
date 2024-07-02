using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Controllers
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new ApplicationRole("User"));
            }

            if (!await roleManager.RoleExistsAsync("EntityAdmin"))
            {
                await roleManager.CreateAsync(new ApplicationRole("EntityAdmin"));
            }
            if (!await roleManager.RoleExistsAsync("GroupAdmin"))
            {
                await roleManager.CreateAsync(new ApplicationRole("GroupAdmin"));
            }
        }
    }

}

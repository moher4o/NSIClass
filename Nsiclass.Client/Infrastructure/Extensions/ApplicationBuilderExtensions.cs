using Nsiclass.Data;
using Nsiclass.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Nsiclass.Common;

namespace Nsiclass.Client.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ClassDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var roleName = DataConstants.AdministratorRole;

                        var result = await roleManager.RoleExistsAsync(roleName);
                        if (!result)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = roleName
                            });
                        }

                        roleName = DataConstants.DeveloperRole;

                        result = await roleManager.RoleExistsAsync(roleName);
                        if (!result)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = roleName
                            });
                        }

                        roleName = DataConstants.MethodologistsExRole;

                        result = await roleManager.RoleExistsAsync(roleName);
                        if (!result)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = roleName
                            });
                        }

                        roleName = DataConstants.MethodologistsInRole;

                        result = await roleManager.RoleExistsAsync(roleName);
                        if (!result)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = roleName
                            });
                        }

                        roleName = DataConstants.TsbRole;

                        result = await roleManager.RoleExistsAsync(roleName);
                        if (!result)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = roleName
                            });
                        }

                        roleName = DataConstants.NsiUser;

                        result = await roleManager.RoleExistsAsync(roleName);
                        if (!result)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = roleName
                            });
                        }


                        var adminEmail = DataConstants.DeveloperUsername;
                        var adminUser = await userManager.FindByEmailAsync(adminEmail);
                        if (adminUser == null)
                        {
                            adminUser = new User
                            {
                                Email = DataConstants.DeveloperUsername,
                                UserName = DataConstants.DeveloperUsername,
                                isDeleted = false,
                                DateCreated = DateTime.UtcNow.Date,
                                DepartmentId = 3,
                                Name = string.Concat(DataConstants.DeveloperFirstName, " ",DataConstants.DeveloperLastName)
                             };
                            await userManager.CreateAsync(adminUser, DataConstants.DeveloperPassword);

                            await userManager.AddToRoleAsync(adminUser, DataConstants.DeveloperRole);
                            await userManager.AddToRoleAsync(adminUser, DataConstants.AdministratorRole);
                        }
                        //if (!await userManager.IsInRoleAsync(adminUser, DataConstants.DeveloperRole))
                        //{
                        //    await userManager.AddToRoleAsync(adminUser, DataConstants.DeveloperRole);
                        //}
                        //if (!await userManager.IsInRoleAsync(adminUser, DataConstants.AdministratorRole))
                        //{
                        //    await userManager.AddToRoleAsync(adminUser, DataConstants.AdministratorRole);
                        //}


                    }
                    ).Wait();
            }


            return app;
        }
    }
}

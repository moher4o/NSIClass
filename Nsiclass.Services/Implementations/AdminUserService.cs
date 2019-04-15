using Nsiclass.Data;
using AutoMapper.QueryableExtensions;
using Nsiclass.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using static Nsiclass.Common.DataConstants;
using Nsiclass.Services.Models;
using Nsiclass.Common;

namespace Nsiclass.Services.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly ClassDbContext db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public AdminUserService(ClassDbContext db, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.db = db;
            this.roleManager = roleManager;
            this._userManager = userManager;
        }

        public async Task<List<AdminUserServiceModel>> GetAllUsers()
        {
            return await Task.Run(() =>
            {
                var allUsers = this._userManager.Users;

                var result = allUsers.AsQueryable()
                        .ProjectTo<AdminUserServiceModel>()
                        .ToList();

                return result;
            });

        }

        public async Task<List<AdminUserServiceModel>> GetAllUsers(string role)
        {
           var allUsers = await _userManager.GetUsersInRoleAsync(role);

            var result = allUsers.Select(u => new AdminUserServiceModel
            {
                Id = u.Id,
                DateCreated = u.DateCreated,
                DepartmentName = this.db.Departments.Where(d => d.Id == u.DepartmentId).Select(d => d.Name).FirstOrDefault(),
                Name = u.Name,
                ParentUser = this.db.Users.Where(p => p.Id == u.CreateUserId).Select(p => p.UserName).FirstOrDefault(),
                Phone = u.Phone,
                isDeleted = u.isDeleted
            }).ToList();

            return result;
        }

        public async Task<List<AdminUserServiceModel>> GetSearchedUsers(string searchString)
        {
            return await Task.Run(() =>
            {
                var allUsers = this._userManager.Users.Where(u => u.Name.Contains(searchString) || u.UserName.Contains(searchString));
                
                var result = allUsers.AsQueryable()
                        .ProjectTo<AdminUserServiceModel>()
                        .ToList();

                return result;
            });
        }


        public async Task<IQueryable<User>> GetAllUsersAsync()
        {
            return await Task.Run(() =>
            {
                return this._userManager.Users;
            });
        }

        public async Task<bool> UpdateUserData(string userId, string name, string phone,string phoneNumber, string email, int departmentId, IEnumerable<string> roles)
        {
            try
            {
                var realUser = this.db.Users.FirstOrDefault(u => u.Id == userId);
                var department = this.db.Departments.Where(d => d.Id == departmentId && d.isDeleted == false).FirstOrDefault();
                if (realUser == null || department == null)
                {
                    return false;
                }

                realUser.Name = name;
                realUser.Phone = phone;
                realUser.PhoneNumber = phoneNumber;
                realUser.Email = email;
                realUser.Department = department;

                await this.db.SaveChangesAsync();

                //премахвам всички роли на потребителя
                var realroles = this.roleManager.Roles.Where(r => r.Name != DataConstants.DeveloperRole).ToList();
                foreach (var role in realroles)
                {
                    if (await this._userManager.IsInRoleAsync(realUser, role.Name))
                    {
                        var result = await this._userManager.RemoveFromRoleAsync(realUser, role.Name);
                        if (!result.Succeeded)
                        {
                            return false;
                        }
                    }

                }

                //добавям го към избраните роли
                foreach (var role in roles)
                {
                    if (!await this._userManager.IsInRoleAsync(realUser,role))
                    {
                        var result = await this._userManager.AddToRoleAsync(realUser, role);
                        if (!result.Succeeded)
                        {
                            return false;
                        }
                    }
                }
                

                return true;

            }
            catch (Exception)
            {
                return false;
            }
         }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (userId == null)
            {
                return false;
            }

            var userToDelete = this.db.Users.FirstOrDefault(u => u.Id == userId);
            if (userToDelete == null)
            {
                return false;
            }

            //this.db.Users.Remove(userToDelete);
            userToDelete.isDeleted = true;
            userToDelete.LockoutEnabled = true;
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreUserAsync(string userId)
        {
            if (userId == null)
            {
                return false;
            }

            var userToRestore = this.db.Users.FirstOrDefault(u => u.Id == userId);
            if (userToRestore == null)
            {
                return false;
            }

            //this.db.Users.Remove(userToDelete);
            userToRestore.isDeleted = false;
            userToRestore.LockoutEnabled = false;
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<string> CreateNewUser(string username, string name, string phone, string phoneNumber, string email, string password, int departmentId, List<string> roles, string parentUserId)
        {

            var department = this.db.Departments.Where(d => d.Id == departmentId && d.isDeleted == false).FirstOrDefault();
            if (department == null)
            {
                return "Невалидна месторабота";
            }
            
            var newUser = new User()
            {
                UserName = username,
                Name = name,
                Phone = phone,
                PhoneNumber = phoneNumber,
                Email = email,
                Department = department,
                DateCreated = DateTime.UtcNow.Date,
                CreateUserId = parentUserId
               
            };

            var result = await this._userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                await this._userManager.AddToRolesAsync(newUser, roles);
                return "success";
            }
            else
            {
                var error = result.Errors.Select(e => e.Description).FirstOrDefault();

                return error;
            }
            


        }

    }
}

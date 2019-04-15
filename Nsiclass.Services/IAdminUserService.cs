using Nsiclass.Data.Models;
using Nsiclass.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsiclass.Services
{
    public interface IAdminUserService
    {

        Task<List<AdminUserServiceModel>> GetAllUsers();

        Task<List<AdminUserServiceModel>> GetAllUsers(string role);

        Task<IQueryable<User>> GetAllUsersAsync();

        Task<bool> UpdateUserData(string userId, string name, string phone, string phoneNumber, string email, int departmentId, IEnumerable<string> roles);

        Task<bool> DeleteUserAsync(string userId);

        Task<bool> RestoreUserAsync(string userId);

        Task<string> CreateNewUser(string username, string name, string phone, string phoneNumber, string email, string password, int departmentName, List<string> roles, string parentUserId);

        Task<List<AdminUserServiceModel>> GetSearchedUsers(string searchString);
    }
}

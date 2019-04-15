using Nsiclass.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nsiclass.Services
{
    public interface IAdminLinksService
    {
        List<AdminLinksServiceModel> GetAllLinks();

        Task<bool> CreateLink(string name, string adress);

        AdminLinksServiceModel GetLinkById(int linkId);

        Task<bool> EditLink(AdminLinksServiceModel model);

        Task<bool> DeleteLink(int linkId);

        Task<bool> RestoreLink(int linkId);
    }
}

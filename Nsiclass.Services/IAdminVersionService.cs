using Nsiclass.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nsiclass.Services
{
    public interface IAdminVersionService
    {
        Task<VersionServiceModel> GetVersionDetailsAsync(string classCode, string versionCode);

        Task<string> EditVersionAsync(string classif, string version, string parent, string publications, string remarks, string byLow, DateTime valid_From, DateTime? valid_To, string useAreas);

        Task<string> DeleteClassVersionAsync(string classCode, string versionCode);

        Task<string> RestoreClassVersionAsync(string classCode, string versionCode);

        Task<bool> IsClassVersionExistAsync(string classCode, string versionCode);

        Task<bool> IsClassVersionDeletedAsync(string classCode, string versionCode);

        Task<IEnumerable<string>> GetVersionNamesByClassAsync(string classCode);

        Task<string> ActivateClassVersionAsync(string classCode, string versionCode);

        Task<string> DeactivateClassVersionAsync(string classCode, string versionCode);
    }
}

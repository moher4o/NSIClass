using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nsiclass.Services.Models;

namespace Nsiclass.Services
{
    public interface IAdminItemsService
    {
 
        Task<ClassItemServiceModel> GetItemInfoAsync(string classCode, string versionCode, string itemCode);

        Task<bool> ItemExistAsync(string classCode, string versionCode, string itemCode);

        Task<string> EditItemDetailsAsync(string classCode, string versionCode, string itemCode, string newItemCode, string description, string descriptionShort, string descriptionEng, string include, string includeMore, string includeNo, string userId, DateTime editTime, bool isLeaf);

        Task<string> DeleteItemAsync(string classCode, string versionCode, string itemCode, string userId);

        Task<string> RestoreItemAsync(string classCode, string versionCode, string itemCode, string userId);

        Task<string> ChangeRelationStatus(string relTypeId, string srcclassCode, string srcversionCode, string srcitemCode, string destclassCode, string destversionCode, string destitemCode);

        Task<string> AddNewItemAsync(string classCode, string versionCode, string newItemId, string description, string ParentItemCode, bool isLeaf, string userId);
    }
}

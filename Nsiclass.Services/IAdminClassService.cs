using Nsiclass.Data.Models;
using Nsiclass.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nsiclass.Services
{
    public interface IAdminClassService
    {
        //Task<bool> AddClassItem(List<TC_Classif_Items> items);

        Task<string> AddItemsCollection(List<AddNewClassItemsServiceModel> items);

        //bool ItemExist(string classif, string version, string itemCode);

        Task<bool> AddNewClassification(string classCode, string version, string name, string nameEng, string remarks, bool isHierachy, DateTime validFrom, DateTime? validTo);

        Task<bool> AddNewClassVersionAsync(string classCode, string version, string remarks, DateTime validFrom, DateTime? validTo);

        IEnumerable<string> GetClassNames();

        IEnumerable<string> GetAllClassNames();

        IEnumerable<string> GetClassVersionCodes(string classCode);

        Task<IEnumerable<ClassListServiceModel>> GetClassCodesNamesVersions(string classVersionCode);

        Task<ClassListServiceModel> GetClassInfoAsync(string classCode);

        bool IsClassificationHierachical(string classCode);

        bool IsClassificationExist(string classCode);

        bool IsClassificationDeleted(string classCode);

        Task<bool> AddRelationTypeAsync(string sourceClass, string sourceVersionCode, string destClass, string destVersionCode, string description, DateTime validFrom, DateTime? validTo);

        IEnumerable<string> GetRelationNames();

        TC_Classif_Rel_Types GetRelationTypeByName(string relName);

        Task<List<TC_Classif_Items>> GetTreeItemsAsync(string classCode, string versionCode, string searchString);

        Task<string> AddRelItemsCollection(List<AddNewRelItemsServiceModel> items);

        Task<AdminClassServiceMoedel> GetClassificationDetailsAsync(string classCode);

        Task<string> UpdateClassificationDetailsAsync(string id, string name, string nameEng, string remarks);

        Task<string> DeleteClasificationAsync(string classId);

        Task<string> RestoreClasificationAsync(string classId);

        Task<IList<ClassListServiceModel>> GetAllClassListAsync(string searchString);

        Task<IList<ExportRelItemsServiceModel>> ExportRelationAsync(string relationId);

        Task<IList<RelationListServiceModel>> GetAllRelationListAsync(string searchString);

        Task<RelationListServiceModel> GetRelInfoAsync(string relCode);

        Task<IList<RelListServiceModel>> GetAllRelsNames(string searchString);

        Task<AdminRelationsDetailsServiceModel> GetRelationDetailsAsync(string relCode);

        Task<string> UpdateRelationAsync(string id, string description, DateTime valid_From, DateTime? valid_To);

        //Task<RelationListServiceModel> GetRelShortInfoAsync(string relCode);

        Task<bool> AddRelationItemAsync(string relationId, string srcItemId, string destItemId, string userId);

        Task<IList<ExportClassVersionServiceModel>> ExportClassVersionAsync(string classCode, string versionCode);

        Task<string> GetClassNameByIdAsync(string classCode);
    }
}

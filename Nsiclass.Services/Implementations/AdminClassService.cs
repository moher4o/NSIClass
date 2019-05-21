using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nsiclass.Data;
using Nsiclass.Data.Models;
using System.Linq;
using System.Data.SqlClient;
//using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;
using Nsiclass.Services.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Nsiclass.Services.Implementations
{
    public class AdminClassService : IAdminClassService
    {
        private readonly ClassDbContext db;
        public AdminClassService(ClassDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public async Task<string> GetClassNameByIdAsync(string classCode)
        {
            return await this.db.Classifications.Where(c => c.Id == classCode).Select(i => i.Name).FirstOrDefaultAsync();
        }

        public async Task<IList<ExportRelItemsServiceModel>> ExportRelationAsync(string relationId)
        {
                var relation = await this.db.ClassRelations
                    .Where(r => r.RelationTypeId == relationId && r.IsDeleted == false)
                    .OrderBy(r => r.SrcItemId)
                    .ProjectTo<ExportRelItemsServiceModel>()
                    .ToListAsync();

             return relation;
        }

        public async Task<RelationListServiceModel> GetRelInfoAsync(string relCode)
        {
            return await this.db.ClassRelationsTypes
            .Where(c => c.Id == relCode)
            .Include(c => c.Relations)
            .OrderBy(c => c.Description)
            .ProjectTo<RelationListServiceModel>()
            .FirstOrDefaultAsync();

        }

        public async Task<bool> AddRelationItemAsync(string relationId, string srcItemId, string destItemId, string userId)
        {
            var currentRelationType = await this.db.ClassRelationsTypes.Where(r => r.Id == relationId).FirstOrDefaultAsync();
            if (currentRelationType == null)
            {
                return false;
            }

            var searchedRelation = await this.db.ClassRelations.Where(r => r.RelationTypeId == relationId && r.SrcClassif == currentRelationType.SrcClassifId && r.SrcVer == currentRelationType.SrcVersionId && r.SrcItemId == srcItemId && r.DestClassif == currentRelationType.DestClassifId && r.DestVer == currentRelationType.DestVersionId && r.DestItemId == destItemId).FirstOrDefaultAsync();
            if (searchedRelation != null)
            {
                return false;
            }

            await this.db.Database.BeginTransactionAsync();
            try
            {
                var newRelation = new TC_Classif_Rels()
                {
                    RelationTypeId = relationId,
                    SrcClassif = currentRelationType.SrcClassifId,
                    SrcVer = currentRelationType.SrcVersionId,
                    SrcItemId = srcItemId,
                    DestClassif = currentRelationType.DestClassifId,
                    DestVer = currentRelationType.DestVersionId,
                    DestItemId = destItemId,
                    EntryTime = DateTime.Now,
                    EnteredByUserId = userId
                };
                await this.db.ClassRelations.AddAsync(newRelation);
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return false;
            }
        
            this.db.Database.CommitTransaction();

            return true;
        }


        //public async Task<RelationListServiceModel> GetRelShortInfoAsync(string relCode)
        //{
        //    return await this.db.ClassRelationsTypes
        //    .Where(c => c.Id == relCode)
        //    .OrderBy(c => c.Description)
        //    .ProjectTo<RelationListServiceModel>()
        //    .FirstOrDefaultAsync();
        //}



        public async Task<IList<RelationListServiceModel>> GetAllRelationListAsync(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await this.db.ClassRelationsTypes
                                .OrderBy(c => c.Description)
                                .ProjectTo<RelationListServiceModel>()
                                .ToListAsync();

            }
            else
            {
                return await this.db.ClassRelationsTypes
                                .Where(c => c.Description.Contains(searchString) || c.Id == searchString)
                                .OrderBy(c => c.Description)
                                .ProjectTo<RelationListServiceModel>()
                                .ToListAsync();

            }
        }

        public async Task<IList<ExportClassVersionServiceModel>> ExportClassVersionAsync(string classCode, string versionCode)
        {
            var relation = await this.db.ClassItems
                .Where(i => i.Classif == classCode && i.Version == versionCode && i.IsDeleted == false)
                .OrderBy(i => i.ItemLevel)
                .ProjectTo<ExportClassVersionServiceModel>()
                .ToListAsync();

            return relation;
        }


        public async Task<bool> AddNewClassification(string classCode, string version, string name, string nameEng, string remarks, bool isHierachy, DateTime validFrom, DateTime? validTo)
        {
            if (this.IsClassificationExist(classCode))
            {
                return false;
            }

            var newClass = new TC_Classifications()
            {
                Id = classCode,
                Name = name,
                NameEng = nameEng,
                IsHierachy = isHierachy,
                isDeleted = false,
                Remarks = remarks

            };
            var newClassVer = new TC_Classif_Vers()
            {
                Classif = classCode,
                Version = version,
                Valid_From = validFrom.Date,
                Valid_To = validTo != null ? validTo : null,
                isDeleted = false,
                Remarks = remarks
            };

            await this.db.Database.BeginTransactionAsync();
            try
            {
                await this.db.Classifications.AddAsync(newClass);
                await this.db.SaveChangesAsync();
                await this.db.ClassVersions.AddAsync(newClassVer);
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return false;
            }
            this.db.Database.CommitTransaction();
            return true;
        }

        public IEnumerable<string> GetClassNames()
        {
            var names = this.db.Classifications.Where(c => c.isDeleted == false).Select(c => c.Id).ToList();
            return names;
        }

        public async Task<ClassListServiceModel> GetClassInfoAsync(string classCode)
        {
            return await this.db.Classifications
            .Where(c => c.Id == classCode)
            .Include(c => c.Versions)
            .OrderBy(c => c.Name)
            .ProjectTo<ClassListServiceModel>()
            .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<ClassListServiceModel>> GetClassCodesNamesVersions(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await this.db.Classifications
                .Where(c => c.isDeleted == false)
                .Include(c => c.Versions)
                .OrderBy(c => c.Name)
                .ProjectTo<ClassListServiceModel>()
                .ToListAsync();
            }
            else
            {
                return await this.db.Classifications
                .Where(c => c.isDeleted == false && (c.Id.Contains(searchString) || c.Name.Contains(searchString) || c.NameEng.Contains(searchString)))
                .Include(c => c.Versions)
                .OrderBy(c => c.Name)
                .ProjectTo<ClassListServiceModel>()
                .ToListAsync();

            }
        }

        public async Task<IList<ClassListServiceModel>> GetAllClassListAsync(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await this.db.Classifications
                                .OrderBy(c => c.Name)
                                .ProjectTo<ClassListServiceModel>()
                                .ToListAsync();

            }
            else
            {
                return await this.db.Classifications
                            .Where(c => c.Id.Contains(searchString) || c.Name.Contains(searchString) || c.NameEng.Contains(searchString))
                                .OrderBy(c => c.Name)
                                .ProjectTo<ClassListServiceModel>()
                                .ToListAsync();

            }
        }

        public IEnumerable<string> GetAllClassNames()
        {
            var names = this.db.Classifications.Select(c => c.Id).ToList();
            return names;
        }

        public async Task<IList<RelListServiceModel>> GetAllRelsNames(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await this.db.ClassRelationsTypes
                                .OrderBy(c => c.Description)
                                .ProjectTo<RelListServiceModel>()
                                .ToListAsync();

            }
            else
            {
                return await this.db.ClassRelationsTypes
                            .Where(c => c.Description.Contains(searchString))
                                .OrderBy(c => c.Description)
                                .ProjectTo<RelListServiceModel>()
                                .ToListAsync();

            }
        }



        public IEnumerable<string> GetClassVersionCodes(string classCode)
        {
            return this.db.ClassVersions.Where(c => c.Classif == classCode).Select(c => c.Version).ToList();
        }

        public async Task<bool> AddNewClassVersionAsync(string classCode, string version, string remarks, DateTime validFrom, DateTime? validTo)
        {
            var currentClass = this.db.Classifications.Where(c => c.Id == classCode && c.isDeleted == false).FirstOrDefault();
            if (currentClass == null || version == null)
            {
                return false;
            }

            var newVersion = new TC_Classif_Vers()
            {
                Classif = currentClass.Id,
                Version = version,
                Remarks = remarks,
                Valid_From = validFrom,
                Valid_To = validTo != null ? validTo : null,
                isDeleted = false
            };
            await this.db.Database.BeginTransactionAsync();
            try
            {
                await this.db.ClassVersions.AddAsync(newVersion);
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return false;
            }
            this.db.Database.CommitTransaction();
            return true;

        }

        public async Task<string> AddItemsCollection(List<AddNewClassItemsServiceModel> items)
        {
            if (!IsClassificationExist(items[0].Classif))
            {
                return $"Няма класификация с код {items[0].Classif}";
            }
            var connectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand("ALTER TABLE ClassItems NOCHECK CONSTRAINT FK_ClassItems_ClassItems_Classif_Version_ParentItemCode");
            SqlCommand comend = new SqlCommand("ALTER TABLE ClassItems CHECK CONSTRAINT FK_ClassItems_ClassItems_Classif_Version_ParentItemCode");
            com.Connection = con;
            comend.Connection = con;

            con.Open();
            com.ExecuteNonQuery();
            //SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable);
            await this.db.Database.BeginTransactionAsync();
            for (int i = 0; i <= items.Count() - 1; i++)
            {
                try
                {
                    var newItemDb = new TC_Classif_Items()
                    {
                        Classif = items[i].Classif,
                        Version = items[i].Version,
                        ItemCode = items[i].ItemCode,
                        Description = items[i].Description,
                        DescriptionShort = items[i].DescriptionShort,
                        DescriptionEng = items[i].DescriptionEng,
                        Includes = items[i].Includes,
                        IncludesMore = items[i].IncludesMore,
                        IncludesNo = items[i].IncludesNo,
                        EnteredByUserId = items[i].EnteredByUserId,
                        EntryTime = items[i].EntryTime,
                        IsLeaf = items[i].IsLeaf,
                        ItemLevel = items[i].ItemLevel,
                        OrderNo = items[i].OrderNo,
                        ParentItemCode = items[i].ParentItemCode,
                        IsDeleted = false
                    };
                    await this.db.ClassItems.AddAsync(newItemDb);
                    await this.db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    this.db.Database.RollbackTransaction();
                    comend.ExecuteNonQuery();
                    con.Close();
                    return string.Concat($"Ред N:{i}", " ", items[i].Classif, " ", items[i].Version, " ", items[i].ItemCode, " има грешка");
                }
            }
            this.db.Database.CommitTransaction();
            //transaction.Commit();
            comend.ExecuteNonQuery();
            con.Close();
            return "success";

        }

        private async Task AddSingleItem(SqlConnection con, SqlCommand com, SqlCommand comend, TC_Classif_Items item)
        {
            con.Open();


            com.Connection = con;
            comend.Connection = con;
            com.ExecuteNonQuery();



            await this.db.ClassItems.AddAsync(item);
            await this.db.SaveChangesAsync();
            comend.ExecuteNonQuery();
            comend.Dispose();
            com.Dispose();
            con.Close();
        }

        //public bool ItemExist(string classif, string version, string itemCode)
        //{
        //    var item = this.db.ClassItems.FirstOrDefault(i => i.Classif == classif && i.Version == version && i.ItemCode == itemCode);
        //    if (item != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public bool IsClassificationHierachical(string classCode)
        {
            var result = this.db.Classifications.FirstOrDefault(c => c.Id == classCode);
            if (result.IsHierachy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsClassificationExist(string classCode)
        {
            var result = this.db.Classifications.Where(c => c.Id == classCode).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsClassificationDeleted(string classCode)
        {
            var result = this.db.Classifications.Where(c => c.Id == classCode && c.isDeleted == true).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> AddRelationTypeAsync(string sourceClass, string sourceVersionCode, string destClass, string destVersionCode, string description, DateTime validFrom, DateTime? validTo)
        {
            if (!this.IsClassificationExist(sourceClass) || !this.IsClassificationExist(destClass))
            {
                return false;
            }

            var newRelationType = new TC_Classif_Rel_Types()
            {
                SrcClassifId = sourceClass,
                DestClassifId = destClass,
                Description = description,
                Valid_From = validFrom,
                Valid_To = validTo,
                IsDeleted = false,
                SrcVersionId = sourceVersionCode,
                DestVersionId = destVersionCode
            };

            try
            {
                await this.db.ClassRelationsTypes.AddAsync(newRelationType);
                await this.db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public IEnumerable<string> GetRelationNames()
        {
            return this.db.ClassRelationsTypes.Where(r => r.IsDeleted == false).Select(r => r.Description).ToList();
        }

        public TC_Classif_Rel_Types GetRelationTypeByName(string relName)
        {
            return this.db.ClassRelationsTypes
                .Where(r => r.IsDeleted == false && r.Description == relName)
                .FirstOrDefault();
        }

        public async Task<string> AddRelItemsCollection(List<AddNewRelItemsServiceModel> items)
        {
            if (!IsRelationExist(items[0].RelationTypeId))
            {
                return $"Няма релация с код {items[0].RelationTypeId}";
            }
            await this.db.Database.BeginTransactionAsync();
            for (int i = 0; i <= items.Count() - 1; i++)
            {
                try
                {
                    var newItemDb = new TC_Classif_Rels()
                    {
                        IsDeleted = false,
                        SrcClassif = items[i].SrcClassif,
                        SrcVer = items[i].SrcVer,
                        SrcItemId = items[i].SrcItemId.Trim(),
                        DestClassif = items[i].DestClassif,
                        DestVer = items[i].DestVer,
                        DestItemId = items[i].DestItemId.Trim(),
                        RelationTypeId = items[i].RelationTypeId,
                        EnteredByUserId = items[i].EnteredByUserId,
                        EntryTime = items[i].EntryTime
                    };
                    await this.db.ClassRelations.AddAsync(newItemDb);
                    await this.db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    this.db.Database.RollbackTransaction();
                    return $"Ред N:{i} има грешка или релацията вече е добавена";
                }
            }
            this.db.Database.CommitTransaction();
            return "success";
        }

        private bool IsRelationExist(string relCode)
        {
            var result = this.db.ClassRelationsTypes.Where(c => c.Id == relCode).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<TC_Classif_Items>> GetTreeItemsAsync(string classCode, string versionCode, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await this.db.ClassItems
                                .Where(c => c.Classif == classCode && c.Version == versionCode && c.IsDeleted == false)
                                .Include(c => c.ChildItems)
                                .OrderBy(c => c.ItemCode)
                                .ToListAsync();


            }
            else
            {
                return await this.db.ClassItems
                                    .Where(c => c.Classif == classCode && c.Version == versionCode && (c.ItemCode.Contains(searchString) || c.Description.Contains(searchString) || c.DescriptionEng.Contains(searchString)) && c.IsDeleted == false)
                                    //.Include(c => c.ChildItems)
                                    .OrderBy(c => c.ItemCode)
                                    .ToListAsync();

            }

        }

        public async Task<AdminClassServiceMoedel> GetClassificationDetailsAsync(string classCode)
        {
            var result = await this.db.Classifications
                .Where(c => c.Id == classCode)
                .Include(c => c.Versions)
                .Include(c => c.AsSourceClassification)
                .Include(c => c.AsDestClassification)
                .ProjectTo<AdminClassServiceMoedel>()
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<AdminRelationsDetailsServiceModel> GetRelationDetailsAsync(string relCode)
        {
            var result = await this.db.ClassRelationsTypes
                .Where(c => c.Id == relCode)
                .ProjectTo<AdminRelationsDetailsServiceModel>()
                .FirstOrDefaultAsync();

            result.Relations = await this.db.ClassRelations
                .Where(r => r.RelationTypeId == relCode)
                .OrderByDescending(r => r.EntryTime)
                .ThenByDescending(r => r.SrcItemId)
                .ThenBy(r => r.DestItemId)
                .ToListAsync();

            return result;
        }


        public async Task<string> UpdateClassificationDetailsAsync(string id, string name, string nameEng, string remarks)
        {
            var currentclass = await this.db.Classifications.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (currentclass == null)
            {
                return "Няма класификация с код: " + id;
            }
            currentclass.Name = name;
            currentclass.NameEng = nameEng;
            currentclass.Remarks = remarks;
            await this.db.SaveChangesAsync();

            return $"Редакцията на класификация {id} беше успешна";
        }



        public async Task<string> DeleteClasificationAsync(string classId)
        {
            if (classId == null)
            {
                return "Грешка! Нулев код на класификация.";
            }

            if (!this.IsClassificationExist(classId))
            {
                return $"Грешка!!! Няма класификация с код: {classId}";
            }

            if (this.IsClassificationDeleted(classId))
            {
                return $"Информация!!! Класификация с код: {classId} вече е била маркирана като изтрита успешно";
            }

            await this.db.Database.BeginTransactionAsync();
            try
            {

                this.db.ClassRelations.Where(r => r.SrcClassif == classId || r.DestClassif == classId).ToList()
                    .ForEach(r => r.IsDeleted = true);
                await this.db.SaveChangesAsync();
                this.db.ClassRelationsTypes.Where(r => r.SrcClassifId == classId || r.DestClassifId == classId).ToList()
                    .ForEach(r => r.IsDeleted = true);
                await this.db.SaveChangesAsync();
                this.db.ClassItems.Where(i => i.Classif == classId).ToList().ForEach(r => r.IsDeleted = true);
                await this.db.SaveChangesAsync();
                this.db.ClassVersions.Where(i => i.Classif == classId).ToList().ForEach(r => r.isDeleted = true);
                await this.db.SaveChangesAsync();
                var currentClass = await this.db.Classifications.Where(i => i.Id == classId).FirstOrDefaultAsync();
                currentClass.isDeleted = true;
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return $"Грешка!!! Възникна проблем при изтриването на класификацията.";
            }
            this.db.Database.CommitTransaction();
            return $"Класификация с код: {classId} беше изтрита успешно";
        }

        public async Task<string> RestoreClasificationAsync(string classId)
        {
            if (classId == null)
            {
                return "Грешка! Нулев код на класификация.";
            }

            if (!this.IsClassificationExist(classId))
            {
                return $"Грешка!!! Няма класификация с код: {classId}";
            }

            if (!this.IsClassificationDeleted(classId))
            {
                return $"Информация!!! Класификация с код: {classId} е действаща";
            }


            await this.db.Database.BeginTransactionAsync();
            try
            {
                var currentClass = await this.db.Classifications.Where(i => i.Id == classId).FirstOrDefaultAsync();
                currentClass.isDeleted = false;
                await this.db.SaveChangesAsync();
                this.db.ClassVersions.Where(i => i.Classif == classId).ToList().ForEach(r => r.isDeleted = false);
                await this.db.SaveChangesAsync();
                this.db.ClassItems.Where(i => i.Classif == classId).ToList().ForEach(r => r.IsDeleted = false);
                await this.db.SaveChangesAsync();
                this.db.ClassRelationsTypes.Where(r => r.SrcClassifId == classId || r.DestClassifId == classId).ToList()
                    .ForEach(r => r.IsDeleted = false);
                await this.db.SaveChangesAsync();
                this.db.ClassRelations.Where(r => r.SrcClassif == classId || r.DestClassif == classId).ToList()
                    .ForEach(r => r.IsDeleted = false);
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return $"Грешка!!! Възникна проблем при възстановяването на класификацията.";
            }
            this.db.Database.CommitTransaction();
            return $"Класификация с код: {classId} беше възстановена успешно";
        }

        public async Task<string> UpdateRelationAsync(string id, string description, DateTime valid_From, DateTime? valid_To)
        {
            try
            {
                var currentRelation = await this.db.ClassRelationsTypes.Where(r => r.Id == id).FirstOrDefaultAsync();
                if (currentRelation == null)
                {
                    return $"Няма релация с Id: {id}";
                }

                currentRelation.Description = description;
                currentRelation.Valid_From = valid_From.Date;
                currentRelation.Valid_To = valid_To;
                await this.db.SaveChangesAsync();
                return "Промените са записани успешно.";

            }
            catch (Exception)
            {
                return "Грешка при запис!";
            }
         }

    }
}

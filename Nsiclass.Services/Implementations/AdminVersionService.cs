using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Nsiclass.Data;
using Nsiclass.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsiclass.Services.Implementations
{
   public class AdminVersionService : IAdminVersionService
    {
        private readonly ClassDbContext db;
        public AdminVersionService(ClassDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<string>> GetVersionNamesByClassAsync(string classCode)
        {
            var names = await this.db.ClassVersions.Where(c => c.isDeleted == false & c.Classif == classCode).Select(c => c.Version).ToListAsync();
            return names;
        }


        public async Task<string> DeleteClassVersionAsync(string classCode, string versionCode)
        {
            if (await this.IsClassVersionExistAsync(classCode, versionCode) == false)
            {
                return $"Грешка!!! Няма версия с код: {classCode} {versionCode}";
            }
            if (await this.IsClassVersionDeletedAsync(classCode, versionCode))
            {
                return $"Информация!!! Версия с код: {classCode} {versionCode} вече е била маркирана като изтрита успешно";
            }

            await this.db.Database.BeginTransactionAsync();
            try
            {

                this.db.ClassRelations.Where(r => (r.SrcClassif == classCode && r.SrcVer == versionCode) || (r.DestClassif == classCode && r.DestVer == versionCode)).ToList().ForEach(r => r.IsDeleted = true);
                await this.db.SaveChangesAsync();
                this.db.ClassRelationsTypes.Where(r => (r.SrcClassifId == classCode && r.SrcVersionId == versionCode) || (r.DestClassifId == classCode && r.DestVersionId == versionCode)).ToList()
                    .ForEach(r => r.IsDeleted = true);
                await this.db.SaveChangesAsync();
                this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode).ToList().ForEach(r => r.IsDeleted = true);
                await this.db.SaveChangesAsync();
                this.db.ClassVersions.Where(i => i.Classif == classCode && i.Version == versionCode).ToList().ForEach(r => r.isDeleted = true);
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return $"Грешка!!! Възникна проблем при изтриването на версията.";
            }
            this.db.Database.CommitTransaction();
            return $"Версия с код: {classCode} {versionCode} беше изтрита успешно";


        }

        public async Task<string> RestoreClassVersionAsync(string classCode, string versionCode)
        {
            if (await this.IsClassVersionExistAsync(classCode, versionCode) == false)
            {
                return $"Грешка!!! Няма версия с код: {classCode} {versionCode}";
            }
            if (!await this.IsClassVersionDeletedAsync(classCode, versionCode))
            {
                return $"Информация!!! Версия с код: {classCode} {versionCode} е активна";
            }
            await this.db.Database.BeginTransactionAsync();
            try
            {
                this.db.ClassVersions.Where(i => i.Classif == classCode && i.Version == versionCode).ToList().ForEach(r => r.isDeleted = false);
                await this.db.SaveChangesAsync();
                this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode).ToList().ForEach(r => r.IsDeleted = false);
                await this.db.SaveChangesAsync();
                this.db.ClassRelationsTypes.Where(r => (r.SrcClassifId == classCode && r.SrcVersionId == versionCode) || (r.DestClassifId == classCode && r.DestVersionId == versionCode)).ToList()
                      .ForEach(r => r.IsDeleted = false);
                await this.db.SaveChangesAsync();
                this.db.ClassRelations.Where(r => (r.SrcClassif == classCode && r.SrcVer == versionCode) || (r.DestClassif == classCode && r.DestVer == versionCode)).ToList().ForEach(r => r.IsDeleted = false);
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return $"Грешка!!! Възникна проблем при възстановяването на версията.";
            }
            this.db.Database.CommitTransaction();
            return $"Версия с код: {classCode} {versionCode} беше възстановена успешно";
        }


        public async Task<bool> IsClassVersionDeletedAsync(string classCode, string versionCode)
        {
            var result = await this.db.ClassVersions.Where(c => c.Classif == classCode && c.Version == versionCode && c.isDeleted == true).FirstOrDefaultAsync();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public async Task<string> EditVersionAsync(string classif, string version, string parent, string publications, string remarks, string byLow, DateTime valid_From, DateTime? valid_To, string useAreas)
        {
            try
            {
                var currentVersion = await this.db.ClassVersions.Where(v => v.Classif == classif && v.Version == version).FirstOrDefaultAsync();
                if (currentVersion == null)
                {
                    return $"Няма класификационна версия {classif} {version}";
                }

                currentVersion.Valid_From = valid_From;
                currentVersion.Valid_To = valid_To;
                currentVersion.Parent = parent;
                currentVersion.Publications = publications;
                currentVersion.Remarks = remarks;
                currentVersion.ByLow = byLow;
                currentVersion.UseAreas = useAreas;
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return $"Неуспешен запис";
            }

            return "Редакцията е успешна";
        }

        public async Task<VersionServiceModel> GetVersionDetailsAsync(string classCode, string versionCode)
        {
            var result = await this.db.ClassVersions
                .Where(c => c.Classif == classCode && c.Version == versionCode)
                .ProjectTo<VersionServiceModel>()
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> IsClassVersionExistAsync(string classCode, string versionCode)
        {
            var currentVersion = await this.db.ClassVersions.Where(v => v.Classif == classCode && v.Version == versionCode).FirstOrDefaultAsync();
            if (currentVersion == null)
            {
                return false;
            }
            return true;
        }

    }
}

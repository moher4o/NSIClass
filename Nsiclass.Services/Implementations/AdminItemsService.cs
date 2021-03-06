﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Nsiclass.Data;
using Nsiclass.Data.Models;
using Nsiclass.Services.Models;

namespace Nsiclass.Services.Implementations
{
    public class AdminItemsService : IAdminItemsService
    {
        private readonly ClassDbContext db;
        private readonly IAdminVersionService versions;
        public AdminItemsService(ClassDbContext db, IAdminVersionService versions)
        { 
            this.db = db;
            this.versions = versions;
        }

        public async Task<bool> ItemExistAsync(string classCode, string versionCode, string itemCode)
        {
            var item = await this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode && i.ItemCode == itemCode).FirstOrDefaultAsync();
            if (item == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<ClassItemServiceModel> GetItemInfoAsync(string classCode, string versionCode, string itemCode)
        {
            var result = await this.db.ClassItems
            .Where(i => i.Classif == classCode && i.Version == versionCode && i.ItemCode == itemCode)
            .Include(c => c.SrcRelations)
            .Include(c => c.DestRelations)
            .Include(c => c.Includes)
            .OrderBy(c => c.Description)
            .ProjectTo<ClassItemServiceModel>()
            .FirstOrDefaultAsync();

            return result;
        }

        public async Task<string> EditItemDetailsAsync(string classCode, string versionCode, string itemCode, string newItemCode, string description, string descriptionShort, string descriptionEng, string include, string includeMore, string includeNo, string userId, DateTime editTime, bool isLeaf)
        {
            var item = await this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode && i.ItemCode == itemCode).Include(i=>i.ChildItems).FirstOrDefaultAsync();
            if (newItemCode.Trim() != itemCode)
            {
                try
                {
                    if (item.IsLeaf != isLeaf)
                    {
                        if (isLeaf && item.ChildItems.Count != 0)
                        {
                            return "Грешка! Елемента е родителски за други елементи и не може да бъде маркиран като листо.";
                        }
                    }

                    item.ItemCode = newItemCode.Trim();
                    item.Description = description;
                    item.DescriptionShort = descriptionShort;
                    item.DescriptionEng = descriptionEng;
                    item.Includes = include;
                    item.IncludesMore = includeMore;
                    item.IncludesNo = includeNo;
                    item.ModifiedByUserId = userId;
                    item.ModifyTime = editTime;
                    item.IsLeaf = isLeaf;
                    await this.db.SaveChangesAsync();
                    return "Редакцията на елемента беше успешна.";
                }
                catch (Exception)
                {
                    return "Грешка! Проверете новия код дали вече не се ползва.";
                }
            }
            else
            {
                try
                {
                    if (item.IsLeaf != isLeaf)
                    {
                        if (isLeaf && item.ChildItems.Count != 0)
                        {
                            return "Грешка! Елемента е родителски за други елементи и не може да бъде маркиран като листо.";
                        }
                    }

                    item.Description = description;
                    item.DescriptionShort = descriptionShort;
                    item.DescriptionEng = descriptionEng;
                    item.Includes = include;
                    item.IncludesMore = includeMore;
                    item.IncludesNo = includeNo;
                    item.ModifiedByUserId = userId;
                    item.ModifyTime = editTime;
                    item.IsLeaf = isLeaf;
                    await this.db.SaveChangesAsync();
                    return "Редакцията на елемента беше успешна.";
                }
                catch (Exception)
                {
                    return "Грешка! Наличие на дълго поле.";
                }

            }

        }

        public async Task<string> DeleteItemAsync(string classCode, string versionCode, string itemCode, string userId)
        {
            var item = await this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode && i.ItemCode == itemCode).FirstOrDefaultAsync();
            if (!item.IsLeaf)
            {
                return $"Грешка!!! Елемента не е листо в сттруктурата";
            }
            if (await this.ItemExistAsync(classCode, versionCode, itemCode) == false)
            {
                return $"Грешка!!! Няма елемент с код: {classCode} {versionCode} {itemCode}";
            }
            if (await this.IsItemDeletedAsync(classCode, versionCode, itemCode))
            {
                return $"Информация!!! Елемент с код: {classCode} {versionCode} {itemCode} вече е бил маркиран като изтрит успешно";
            }
            await this.db.Database.BeginTransactionAsync();
            try
            {
                this.db.ClassRelations.Where(r => (r.SrcClassif == classCode && r.SrcVer == versionCode && r.SrcItemId == itemCode) || (r.DestClassif == classCode && r.DestVer == versionCode && r.DestItemId == itemCode)).ToList().ForEach(r => r.IsDeleted = true);
                await this.db.SaveChangesAsync();
                item.IsDeleted = true;
                item.ModifiedByUserId = userId;
                item.ModifyTime = DateTime.Now;
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return $"Грешка!!! Възникна проблем при изтриването на елемента.";
            }
            this.db.Database.CommitTransaction();
            return $"Елемент с код: {classCode} {versionCode} {itemCode} беше изтрит успешно";

        }

        public async Task<bool> IsItemDeletedAsync(string classCode, string versionCode, string itemCode)
        {
            var item = await this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode && i.IsDeleted).FirstOrDefaultAsync();
            if (item!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> RestoreItemAsync(string classCode, string versionCode, string itemCode, string userId)
        {
            if (await this.ItemExistAsync(classCode, versionCode, itemCode) == false)
            {
                return $"Грешка!!! Няма елемент с код: {classCode} {versionCode} {itemCode}";
            }
            if (!await this.IsItemDeletedAsync(classCode, versionCode, itemCode))
            {
                return $"Информация!!! Елемент с код: {classCode} {versionCode} {itemCode} вече е бил възстановен успешно";
            }


            await this.db.Database.BeginTransactionAsync();
            try
            {
                var allReltypes = await this.db.ClassRelations.Where(i => (i.SrcClassif == classCode && i.SrcVer == versionCode && i.SrcItemId == itemCode && i.IsDeleted) || (i.DestClassif == classCode && i.DestVer == versionCode && i.DestItemId == itemCode && i.IsDeleted)).Select(r => r.RelationTypeId).ToListAsync();
                var activeRelTypesIds = new HashSet<string>(allReltypes);
                var activeRelTypes = await this.db.ClassRelationsTypes.Where(t => activeRelTypesIds.Contains(t.Id) && t.IsDeleted == false).Select(t => t.Id).ToListAsync();
                if (await this.versions.IsClassVersionDeletedAsync(classCode,versionCode))
                {
                    return "Възстановяването невъзможно, поради йерархична зависимост.";
                }
                this.db.ClassRelations.Where(r => ((r.SrcClassif == classCode && r.SrcVer == versionCode && r.SrcItemId == itemCode) || (r.DestClassif == classCode && r.DestVer == versionCode && r.DestItemId == itemCode)) && activeRelTypes.Contains(r.RelationTypeId)).ToList().ForEach(r => r.IsDeleted = false);
                await this.db.SaveChangesAsync();
                var item = await this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode && i.ItemCode == itemCode).FirstOrDefaultAsync();
                item.IsDeleted = false;
                item.ModifiedByUserId = userId;
                item.ModifyTime = DateTime.Now;
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                this.db.Database.RollbackTransaction();
                return $"Грешка!!! Възникна проблем при възстановяването на елемента.";
            }
            this.db.Database.CommitTransaction();
            return $"Елемент с код: {classCode} {versionCode} {itemCode} беше възстановен успешно";
        }

        public async Task<string> ChangeRelationStatus(string relTypeId, string srcclassCode, string srcversionCode, string srcitemCode, string destclassCode, string destversionCode, string destitemCode)
        {
            
            var currentRelation = await this.db.ClassRelations.Where(r => r.SrcClassif == srcclassCode && r.SrcVer == srcversionCode && r.SrcItemId == srcitemCode && r.DestClassif == destclassCode && r.DestVer == destversionCode && r.DestItemId == destitemCode && r.RelationTypeId == relTypeId).FirstOrDefaultAsync();
            if (currentRelation == null)
            {
                return "error";
            }
            string result = string.Empty;
            if (currentRelation.IsDeleted)
            {
                var relation = await this.db.ClassRelationsTypes.Where(t => t.Id == relTypeId).FirstOrDefaultAsync();
                if (relation.IsDeleted)
                {
                    return "Елемента е част от изтрита релация. Първо възстановете релацията.";
                }
                currentRelation.IsDeleted = false;
                result = "restored";
            }
            else
            {
                currentRelation.IsDeleted = true;
                result = "deleted";
            }
            await this.db.SaveChangesAsync();
            return result;
        }

        public async Task<string> AddNewItemAsync(string classCode, string versionCode, string newItemId, string description, string parentItemCode, bool isLeaf, string userId)
        {
            var checkMetaData = await this.versions.IsClassVersionExistAsync(classCode, versionCode);
            if (!checkMetaData)
            {
                return $"Грешка! Няма такава версия {classCode} {versionCode}";
            }

            checkMetaData = await this.ItemExistAsync(classCode, versionCode, newItemId);
            if (checkMetaData)
            {
                return $"Грешка! Вече има елемент с този код: {newItemId}";
            }
            int maxLevel = this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode).Select(i => i.ItemLevel).DefaultIfEmpty(0).Max();

            var newItem = new TC_Classif_Items();
            try
            {
                newItem.Classif = classCode;
                newItem.Version = versionCode;
                newItem.ItemCode = newItemId;
                newItem.Description = description;
                newItem.DescriptionShort = description;
                newItem.EnteredByUserId = userId;
                if (!string.IsNullOrEmpty(parentItemCode))
                {
                    var parentItem = await this.db.ClassItems.Where(i => i.Classif == classCode && i.Version == versionCode && i.ItemCode == parentItemCode).FirstOrDefaultAsync();
                    if (parentItem != null)
                    {
                        newItem.ParentItemCode = parentItemCode;
                        newItem.ItemLevel = parentItem.ItemLevel + 1;
                    }
                    else
                    {
                        return $"Няма родителски елемент с код {parentItemCode}";
                    }
                    
                }
                else
                {
                    newItem.ItemLevel = 1;
                }
                newItem.EntryTime = DateTime.Now;

                if (isLeaf && newItem.ItemLevel < maxLevel && maxLevel >= 1)
                {
                    return $"Има листа с по-висок ранг! Листата трябва да са с еднакъв ранг.";
                }
                newItem.IsLeaf = isLeaf;
                await this.db.ClassItems.AddAsync(newItem);
                await this.db.SaveChangesAsync();
                return "Създаването на елемент беше успешно.";
            }
            catch (Exception)
            {
                return "Грешка! Наличие на дълго поле.";
            }


        }
    }
}

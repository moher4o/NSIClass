using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.XSSF.UserModel;
using Nsiclass.Client.Models.ClassClient;
using Nsiclass.Data.Models;
using Nsiclass.Services;
using Nsiclass.Services.Models;
using OfficeOpenXml;
using static Nsiclass.Common.DataConstants;

namespace Nsiclass.Client.Controllers
{
    public class ClassClientController : Controller
    {
        private readonly IAdminClassService classification;
        private readonly IAdminItemsService items;
        private readonly IAdminLinksService links;
        private readonly UserManager<User> userManager;

        public ClassClientController(UserManager<User> userManager, IAdminClassService classification, IAdminItemsService items, IAdminLinksService links)
        {
            this.classification = classification;
            this.userManager = userManager;
            this.items = items;
            this.links = links;
        }

        public async Task<IActionResult> ClassList(string searchString)
        {
                return View(await this.classification.GetClassCodesNamesVersions(searchString));
           
        }

        public async Task<IActionResult> Relations(string searchString)
        {
            return View(await this.classification.GetAllRelationListAsync(searchString));
        }

        public async Task<IActionResult> GetRelInfoByCode(string relCode)
        {
            var result = await this.classification.GetRelInfoAsync(relCode);
            return Json(result);
        }

        public async Task<IActionResult> ExportRelation(string relCode)
        {
            try
            {
                var relItemsList = await this.classification.ExportRelationAsync(relCode);
                if (relItemsList.Count < 1)
                {
                    TempData[ErrorMessageKey] = "Релацията няма елементи!";
                    return RedirectToAction("Relations", "ClassClient", new {searchString = relCode });
                }
                var stream = new System.IO.MemoryStream();
                using (ExcelPackage package = new ExcelPackage(stream))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Релация");
                        worksheet.Cells[1, 1].Value = "Релация " + relItemsList[0].RelationTypeName;
                        worksheet.Row(1).Style.Font.Size = 16;
                        worksheet.Row(1).Style.Font.Bold = true;

                        worksheet.Cells[2, 1].Value = "Код на класификация източник";
                        worksheet.Cells[2, 2].Value = "Код на версия източник";
                        worksheet.Cells[2, 3].Value = "Код на елемент източник";
                        worksheet.Cells[2, 4].Value = "Име на елемент източник";
                        worksheet.Cells[2, 5].Value = "Код на класификация цел";
                        worksheet.Cells[2, 6].Value = "Код на версия цел";
                        worksheet.Cells[2, 7].Value = "Код на елемент цел";
                        worksheet.Cells[2, 8].Value = "Име на елемент цел";
                        worksheet.Row(2).Style.Font.Size = 12;
                        worksheet.Row(2).Style.Font.Bold = true;

                        for (int c = 3; c < relItemsList.Count + 3; c++)
                        {
                            worksheet.Cells[c, 1].Value = relItemsList[c - 3].SrcClassif;
                            worksheet.Cells[c, 2].Value = relItemsList[c - 3].SrcVer;
                            worksheet.Cells[c, 3].Value = relItemsList[c - 3].SrcItemId;
                            worksheet.Cells[c, 4].Value = relItemsList[c - 3].SrcItemName;
                            worksheet.Cells[c, 5].Value = relItemsList[c - 3].DestClassif;
                            worksheet.Cells[c, 6].Value = relItemsList[c - 3].DestVer;
                            worksheet.Cells[c, 7].Value = relItemsList[c - 3].DestItemId;
                            worksheet.Cells[c, 8].Value = relItemsList[c - 3].DestItemName;

                            worksheet.Row(c).Style.Font.Size = 12;
                        }

                    
                    package.Save();
                }

                string fileName = @"Relations.xlsx";
                string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                stream.Position = 0;
                return File(stream, fileType, fileName);
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Възникна неочаквана грешка!";
                return RedirectToAction("Relations", "ClassClient");
            }

        }

        public async Task<IActionResult> ExportVersion(string classCode, string versionCode)
        {
            try
            {
                string className = await this.classification.GetClassNameByIdAsync(classCode);
                if (className == null)
                {
                    TempData[ErrorMessageKey] = $"Няма класификация с код {classCode}";
                    return RedirectToAction("ClassItemsList", "ClassClient", new { classCode, versionCode });
                }
                var versionItemsList = await this.classification.ExportClassVersionAsync(classCode,versionCode);
                if (versionItemsList.Count < 1)
                {
                    TempData[ErrorMessageKey] = "Класификацията няма елементи!";
                    return RedirectToAction("ClassItemsList", "ClassClient", new { classCode, versionCode });
                }
                var stream = new System.IO.MemoryStream();
                using (ExcelPackage package = new ExcelPackage(stream))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Класификация");
                    worksheet.Cells[1, 1].Value = className + " " + versionCode;
                    worksheet.Row(1).Style.Font.Size = 16;
                    worksheet.Row(1).Style.Font.Bold = true;

                    worksheet.Cells[2, 1].Value = "Код на класификация";
                    worksheet.Cells[2, 2].Value = "Код на версия";
                    worksheet.Cells[2, 3].Value = "Код на елемент";
                    worksheet.Cells[2, 4].Value = "Описание на елемент";
                    worksheet.Cells[2, 5].Value = "English description";
                    worksheet.Cells[2, 6].Value = "Пореден номер";
                    worksheet.Cells[2, 7].Value = "Код на елемент родител";
                    worksheet.Cells[2, 8].Value = "Ниво на елемента";
                    worksheet.Cells[2, 9].Value = "Листо ли е";
                    worksheet.Row(2).Style.Font.Size = 12;
                    worksheet.Row(2).Style.Font.Bold = true;

                    for (int c = 3; c < versionItemsList.Count + 3; c++)
                    {
                        worksheet.Cells[c, 1].Value = versionItemsList[c - 3].Classif;
                        worksheet.Cells[c, 2].Value = versionItemsList[c - 3].Version;
                        worksheet.Cells[c, 3].Value = versionItemsList[c - 3].ItemCode;
                        worksheet.Cells[c, 4].Value = versionItemsList[c - 3].Description;
                        worksheet.Cells[c, 5].Value = versionItemsList[c - 3].DescriptionEng;
                        worksheet.Cells[c, 6].Value = versionItemsList[c - 3].OrderNo;
                        worksheet.Cells[c, 7].Value = versionItemsList[c - 3].ParentItemCode;
                        worksheet.Cells[c, 8].Value = versionItemsList[c - 3].ItemLevel;
                        worksheet.Cells[c, 9].Value = versionItemsList[c - 3].IsLeaf;

                        worksheet.Row(c).Style.Font.Size = 12;
                    }


                    package.Save();
                }

                string fileName = $@"{classCode}_{versionCode}.xlsx";
                string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                stream.Position = 0;
                return File(stream, fileType, fileName);
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Възникна неочаквана грешка!";
                return RedirectToAction("ClassItemsList", "ClassClient", new { classCode, versionCode });
            }

        }

        public async Task<IActionResult> ClassItemsList(string classCode, string versionCode, string searchString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(classCode) || string.IsNullOrWhiteSpace(versionCode))
                {
                    TempData[ErrorMessageKey] = "Невалиден код на класификация или версия!";
                    return RedirectToAction("ClassList", "ClassClient");
                }
                var result = new ItemsTreeViewModel();
                if (string.IsNullOrWhiteSpace(searchString))
                {
                    searchString = null;
                }
                else
                {
                    result.isSearchResult = true;
                }

                result.ClassCode = classCode;
                result.VersionCode = versionCode;
                result.Items = await this.classification.GetTreeItemsAsync(classCode, versionCode, searchString);
                return View(result);

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Възникна неочаквана грешка!";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> GetClassInfoByCode(string classCode)
        {
            var result = await this.classification.GetClassInfoAsync(classCode);
            return Json(result);
        }

        public async Task<IActionResult> GetItemInfoByCode(string classCode, string versionCode, string itemCode)
        {
            ClassItemServiceModel result = await this.items.GetItemInfoAsync(classCode, versionCode, itemCode);
            return Json(result);

        }

        public IActionResult LinksList()
        {
            return View(this.links.GetAllLinks());
        }

        //public async Task<IActionResult> TreeDownload()
        //{
        //    var result = await this.classification.GetTreeItemsAsync();

        //    return Json(result);
        //}


    }
}
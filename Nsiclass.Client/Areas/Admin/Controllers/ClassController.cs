using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nsiclass.Client.Areas.Admin.Models.Class;
using Nsiclass.Data.Models;
using Nsiclass.Services;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using static Nsiclass.Common.DataConstants;
using Nsiclass.Services.Models;
using Microsoft.AspNetCore.Http;
using NPOI.XSSF.UserModel;
using System.Threading;
using Newtonsoft.Json;

namespace Nsiclass.Client.Areas.Admin.Controllers
{

    public class ClassController : BaseController
    {
        private readonly IAdminClassService classification;
        private readonly UserManager<User> userManager;

        public ClassController(UserManager<User> userManager, IAdminClassService classification)
        {
            this.classification = classification;
            this.userManager = userManager;
        }


        public IActionResult AddNewClassName()
        {
            var newClassification = new ClassificationAttr();
            return View(newClassification);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewClassName(ClassificationAttr model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var existingClassificationsCodes = this.classification.GetClassNames();
                if (existingClassificationsCodes.Contains(model.Id))
                {
                    TempData[ErrorMessageKey] = "Има класификация с такова име";
                    return View(model);
                }


                var result = await this.classification.AddNewClassification(model.Id, model.Version, model.Name, model.NameEng, model.Remarks, model.IsHierachy, model.Valid_From.Date, model.Valid_To);

                if (result)
                {
                    TempData[SuccessMessageKey] = $"Класификацията {model.Id} {model.Name} е създадена успешно";
                }
                else
                {
                    TempData[ErrorMessageKey] = "Класификацията не е създадена(възможно е да има изтрита такава със същия код)";
                    return View(model);
                }

                return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка! Класификацията не е създадена";
                return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
            }
        }

        public IActionResult AddNewClassVersion()
        {
            var newClassVersion = new AddNewClassVersionViewModel();

            newClassVersion.ClassCodes = this.classification.GetClassNames()
                                               .Select(a => new SelectListItem
                                               {
                                                   Text = a,
                                                   Value = a
                                               })
                                               .ToList();
            newClassVersion.ClassCodes.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });


            return View(newClassVersion);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewClassVersion(AddNewClassVersionViewModel model, string IdCode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.ClassCodes = this.classification.GetClassNames()
                                       .Select(a => new SelectListItem
                                       {
                                           Text = a,
                                           Value = a
                                       })
                                       .ToList();
                    model.ClassCodes.Insert(0, new SelectListItem
                    {
                        Text = ChooseValue,
                        Value = ChooseValue,
                        Selected = true
                    });


                    return View(model);
                }
                var result = await this.classification.AddNewClassVersionAsync(IdCode, model.Version, model.Remarks, model.Valid_From.Date, model.Valid_To);

                if (result)
                {
                    TempData[SuccessMessageKey] = $"Версията {model.Version} е създадена успешно";
                }
                else
                {
                    TempData[ErrorMessageKey] = "Версията не е създадена";
                }


                return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка! Версията не е създадена";
                return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
            }
        }

        public IActionResult ChooseClass()
        {
            var selectedClass = new List<SelectListItem>();

            selectedClass = this.classification.GetClassNames()
                                               .Select(a => new SelectListItem
                                               {
                                                   Text = a,
                                                   Value = a
                                               })
                                               .ToList();
            selectedClass.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });

            return View(selectedClass);
        }

        public IActionResult SelectVersion(string classCode)
        {
            var newClassVersionSelected = new SelectedClassVersionViewModel()
            {
                ClassCode = classCode,
                VersionCodes = this.classification.GetClassVersionCodes(classCode)
                                    .Select(a => new SelectListItem
                                    {
                                        Text = a,
                                        Value = a
                                    })
                                    .ToList()
            };
            newClassVersionSelected.VersionCodes.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });
            return View(newClassVersionSelected);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectVersion(IFormFile newItems, string classCode, string versionCode)
        {
            if (classCode == null || versionCode == null)
            {
                TempData[ErrorMessageKey] = "Не е избран код или версия на класификация";
            }
            if (newItems == null)
            {
                TempData[ErrorMessageKey] = "Не е избран файл";
                return RedirectToAction("SelectVersion", "Class", new { classCode });
            }

            var fileType = GetMIMEType(newItems.FileName);
            if (fileType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || fileType == "application/vnd.ms-excel")
            {
                //return RedirectToAction("ImportClassItems", "Class", new { newItems, classCode, versionCode });
                return await ImportClassItemsVer2(newItems, classCode, versionCode);
            }
            else
            {
                TempData[ErrorMessageKey] = "Не е избран Excel файл";
                return RedirectToAction("SelectVersion", "Class", new { classCode });
            }
        }

        //private async Task<IActionResult> ImportClassItems(IFormFile file, string classCode, string versionCode)
        //{
        //    try
        //    {
        //        using (var fs = file.OpenReadStream())
        //        {

        //            int orderNo;
        //            var currentuser = await this.userManager.GetUserAsync(User);
        //            var items = new List<AddNewClassItemsServiceModel>();
        //            //HSSFWorkbook hssfwb = new HSSFWorkbook(fs); //чете xls
        //            //XSSFWorkbook xssfwb = new XSSFWorkbook(fs); // чете xlsx
        //            var wb = WorkbookFactory.Create(fs); // фабрика, която създава IWorkbook от xls или xlsx(удобно, защото не се знае какъв файл ще зареди потребителя)
        //            ISheet sheet = wb.GetSheetAt(0);

        //            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++) //Read Excel File
        //            {
        //                IRow row = sheet.GetRow(i);
        //                if (row == null) continue;
        //                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

        //                try
        //                {
        //                    var currentItem = new AddNewClassItemsServiceModel();

        //                    currentItem.Classif = classCode;
        //                    currentItem.Version = versionCode;
        //                    currentItem.ItemCode = row.GetCell(0).ToString();
        //                    currentItem.Description = row.GetCell(1).ToString();
        //                    currentItem.DescriptionShort = currentItem.Description;
        //                    currentItem.DescriptionEng = row.GetCell(2).ToString();
        //                    bool intParseSuccessfull = int.TryParse(row.GetCell(3).ToString(), out orderNo);
        //                    if (intParseSuccessfull)
        //                    {
        //                        currentItem.OrderNo = orderNo;
        //                    }

        //                    if (this.classification.IsClassificationHierachical(classCode))
        //                    {
        //                        currentItem.ParentItemCode = row.GetCell(4) != null ? row.GetCell(4).ToString() : null;
        //                    }


        //                    intParseSuccessfull = int.TryParse(row.GetCell(5).ToString(), out orderNo);
        //                    if (intParseSuccessfull)
        //                    {
        //                        currentItem.ItemLevel = orderNo;
        //                    }
        //                    else
        //                    {
        //                        currentItem.ItemLevel = 9999;
        //                    }
        //                    string isLeaf = row.GetCell(6).ToString();
        //                    if (isLeaf == "Y" || isLeaf == "1")
        //                    {
        //                        currentItem.IsLeaf = true;
        //                    }
        //                    else
        //                    {
        //                        currentItem.IsLeaf = false;
        //                    }

        //                    currentItem.EnteredByUserId = currentuser.Id;

        //                    currentItem.EntryTime = DateTime.UtcNow;

        //                    items.Add(currentItem);
        //                }
        //                catch (Exception)
        //                {
        //                    TempData[ErrorMessageKey] = $"Възникна проблем на ред {i + 1} при четене от файла";
        //                    break;
        //                }

        //            }
        //            var result = await classification.AddItemsCollection(items);
        //            if (result != "success")
        //            {
        //                TempData[ErrorMessageKey] = result;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        TempData[ErrorMessageKey] = "Основна грешка!";
        //    }
        //    if (TempData[ErrorMessageKey] == null)
        //    {
        //        TempData[SuccessMessageKey] = "Елементите на класификацията са добавени успешно!";
        //    }
        //    return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
        //}

        public async Task<IActionResult> ClassDetails(string classCode)
        {
            var result = await this.classification.GetClassificationDetailsAsync(classCode);
            return View(result);
        }

        private async Task<IActionResult> ImportClassItemsVer2(IFormFile file, string classCode, string versionCode)
        {
            try
            {
                using (var fs = file.OpenReadStream())
                {

                    int level;

                    var currentuser = await this.userManager.GetUserAsync(User);
                    var items = new List<AddNewClassItemsServiceModel>();
                    //HSSFWorkbook hssfwb = new HSSFWorkbook(fs); //чете xls
                    //XSSFWorkbook xssfwb = new XSSFWorkbook(fs); // чете xlsx
                    var wb = WorkbookFactory.Create(fs); // фабрика, която създава IWorkbook от xls или xlsx(удобно, защото не се знае какъв файл ще зареди потребителя)
                    ISheet sheet = wb.GetSheetAt(0);

                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        try
                        {
                            var currentItem = new AddNewClassItemsServiceModel();

                            currentItem.Classif = classCode;
                            currentItem.Version = versionCode;
                            currentItem.ItemCode = row.GetCell(0).ToString();
                            currentItem.Description = row.GetCell(3).ToString();
                            currentItem.DescriptionShort = row.GetCell(4).ToString();
                            currentItem.Includes = row.GetCell(5).ToString();
                            currentItem.IncludesMore = row.GetCell(6).ToString();
                            currentItem.IncludesNo = row.GetCell(7).ToString();

                            if (this.classification.IsClassificationHierachical(classCode))
                            {
                                currentItem.ParentItemCode = row.GetCell(1) != null ? row.GetCell(1).ToString() : null;
                            }

                            bool intParseSuccessfull = int.TryParse(row.GetCell(2).ToString(), out level);

                            if (intParseSuccessfull)
                            {
                                currentItem.ItemLevel = level;
                                if (level == 4)
                                {
                                    currentItem.IsLeaf = true;
                                }
                                else
                                {
                                    currentItem.IsLeaf = false;
                                }
                            }
                            else
                            {
                                currentItem.ItemLevel = 9999;
                            }

                            currentItem.EnteredByUserId = currentuser.Id;

                            currentItem.EntryTime = DateTime.UtcNow;

                            items.Add(currentItem);
                        }
                        catch (Exception)
                        {
                            TempData[ErrorMessageKey] = $"Възникна проблем на ред {i + 1} при четене от файла";
                            break;
                        }

                    }
                    var result = await classification.AddItemsCollection(items);
                    if (result != "success")
                    {
                        TempData[ErrorMessageKey] = result;
                    }
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Основна грешка!";
            }
            if (TempData[ErrorMessageKey] == null)
            {
                TempData[SuccessMessageKey] = "Елементите на класификацията са добавени успешно!";
            }
            return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
        }


        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassDetails(AdminClassServiceMoedel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(await this.classification.GetClassificationDetailsAsync(model.Id));
                }

                string result = await this.classification.UpdateClassificationDetailsAsync(model.Id, model.Name, model.NameEng, model.Remarks);
                if (result.Contains("Редакцията"))
                {
                    TempData[SuccessMessageKey] = result;
                }
                else
                {
                    TempData[ErrorMessageKey] = result;
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка!!! Редакцията е неуспешна.";
            }
            return RedirectToAction("AdminTasks", "Users");
        }

        [HttpPost]
        // [Authorize(Roles = "Администратор, Програмист")]
        [Authorize(Policy = "MustBeAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DestroyClass(string classId)
        {
            string result = await this.classification.DeleteClasificationAsync(classId);
            if (result.Contains("успешно"))
            {
                TempData[SuccessMessageKey] = result;
            }
            else
            {
                TempData[ErrorMessageKey] = result;
            }

            return RedirectToAction("AdminTasks", "Users");
        }

        [HttpPost]
        // [Authorize(Roles = "Администратор, Програмист")] - същото
        [Authorize(Policy = "MustBeAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreClass(string classId)
        {
            string result = await this.classification.RestoreClasificationAsync(classId);
            if (result.Contains("успешно"))
            {
                TempData[SuccessMessageKey] = result;
            }
            else
            {
                TempData[ErrorMessageKey] = result;
            }

            return RedirectToAction("AdminTasks", "Users");
        }

        public async Task<IActionResult> ClassList(string searchString)
        {
            var classList = await this.classification.GetAllClassListAsync(searchString);
            return View(classList);
        }

    }
}
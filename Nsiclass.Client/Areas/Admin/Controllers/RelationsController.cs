using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using Nsiclass.Client.Areas.Admin.Models.Relations;
using Nsiclass.Data.Models;
using Nsiclass.Services;
using Nsiclass.Services.Models;
using static Nsiclass.Common.DataConstants;
using Microsoft.AspNetCore.Http;

namespace Nsiclass.Client.Areas.Admin.Controllers
{
    public class RelationsController : BaseController
    {
        private readonly IAdminClassService classification;
        private readonly IAdminVersionService versions;
        private readonly IAdminItemsService items;
        private readonly UserManager<User> userManager;

        public RelationsController(UserManager<User> userManager, IAdminClassService classification, IAdminVersionService versions, IAdminItemsService items)
        {
            this.classification = classification;
            this.versions = versions;
            this.items = items;
            this.userManager = userManager;
        }

        public IActionResult CreateNewClassRelType()
        {
            var newRelation = new AddNewClassRelTypeViewModel();

            newRelation = RelTypeModelPrepareForView(newRelation);

            return View(newRelation);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewClassRelType(AddNewClassRelTypeViewModel model, string SourceCode, string DestCode, string SourceVersionCode, string DestVersionCode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model = RelTypeModelPrepareForView(model);
                    return View(model);
                }
                var existingClassificationsCodes = this.classification.GetClassNames();
                if (!existingClassificationsCodes.Contains(SourceCode) || !existingClassificationsCodes.Contains(DestCode))
                {
                    TempData[ErrorMessageKey] = "Няма класификация с такъв код";
                    model = RelTypeModelPrepareForView(model);
                    return View(model);
                }
                if (!await this.versions.IsClassVersionExistAsync(SourceCode, SourceVersionCode) || !await this.versions.IsClassVersionExistAsync(DestCode,DestVersionCode))
                {
                    TempData[ErrorMessageKey] = "Няма класификационна версия с такъв код";
                    model = RelTypeModelPrepareForView(model);
                    return View(model);
                }


                bool result = await this.classification.AddRelationTypeAsync(SourceCode, SourceVersionCode, DestCode, DestVersionCode, model.Description, model.Valid_From, model.Valid_To);
                if (result)
                {
                    TempData[SuccessMessageKey] = $"Релацията {model.Description} е създадена успешно";
                }
                else
                {
                    TempData[ErrorMessageKey] = "Релацията не е създадена";
                    model = RelTypeModelPrepareForView(model);
                    return View(model);
                }

                return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка! Релацията не е създадена";
                return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
            }
        }

        public IActionResult ImportRel()
        {
            var relTypes = new ImportNewRelItemsViewModel()
            {
                RelCodes = this.classification.GetRelationNames()
                .Select(a => new SelectListItem
                {
                    Text = a,
                    Value = a
                }).ToList()

            };
            relTypes.RelCodes.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });

            return View(relTypes);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportRel(IFormFile newItems, string relName)
        {
            try
            {
                TC_Classif_Rel_Types relationType = this.classification.GetRelationTypeByName(relName);
                if (relationType == null)
                {
                    TempData[ErrorMessageKey] = $"Няма дефинирана релация {relName}";
                    return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
                }

                using (var fs = newItems.OpenReadStream())
                {
                    var currentuser = await this.userManager.GetUserAsync(User);
                    var items = new List<AddNewRelItemsServiceModel>();
                    var wb = WorkbookFactory.Create(fs); // фабрика, която създава IWorkbook от xls или xlsx(удобно, защото не се знае какъв файл ще зареди потребителя)
                    ISheet sheet = wb.GetSheetAt(0);

                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        try
                        {
                            var currentItem = new AddNewRelItemsServiceModel();
                            currentItem.SrcClassif = relationType.SrcClassifId;
                            currentItem.SrcVer = relationType.SrcVersionId;
                            currentItem.SrcItemId = row.GetCell(0).ToString();
                            currentItem.DestClassif = relationType.DestClassifId;
                            currentItem.DestVer = relationType.DestVersionId;
                            currentItem.DestItemId = row.GetCell(1).ToString();
                            currentItem.RelationTypeId = relationType.Id;
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
                    string result = await this.classification.AddRelItemsCollection(items);
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
                TempData[SuccessMessageKey] = "Елементите на релацията са добавени успешно!";
            }
            return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
        }

        private AddNewClassRelTypeViewModel RelTypeModelPrepareForView(AddNewClassRelTypeViewModel newRelation)
        {
            newRelation.SourceClassId = this.classification.GetClassNames()
                                               .Select(a => new SelectListItem
                                               {
                                                   Text = a,
                                                   Value = a
                                               })
                                               .ToList();
            newRelation.SourceClassId.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });
            newRelation.SourceClassVersionId.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });

            newRelation.DestClassId = this.classification.GetClassNames()
                                   .Select(a => new SelectListItem
                                   {
                                       Text = a,
                                       Value = a
                                   })
                                   .ToList();
            newRelation.DestClassId.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });
            newRelation.DestClassVersionId.Insert(0, new SelectListItem
            {
                Text = ChooseValue,
                Value = ChooseValue,
                Selected = true
            });

            //newRelation.Valid_From = DateTime.UtcNow.Date;

            return newRelation;
        }

        public async Task<IActionResult> GetClassVersionsByClassCode(string classCode)
        {
            var result = await this.versions.GetVersionNamesByClassAsync(classCode);
            return Json(result);
        }

        public async Task<IActionResult> RelList(string searchString)
        {
            var relList = await this.classification.GetAllRelsNames(searchString);
            return View(relList);
        }

        public async Task<IActionResult> RelDetails(string relCode)
        {
            AdminRelationsDetailsServiceModel result = await this.classification.GetRelationDetailsAsync(relCode);
            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RelDetails(AdminRelationsDetailsServiceModel model, string Id)
        {
            try
            {
                string result = await this.classification.UpdateRelationAsync(Id, model.Description, model.Valid_From, model.Valid_To);
                if (result.Contains("успешно"))
                {
                    TempData[SuccessMessageKey] = result;
                    return RedirectToAction("RelDetails", "Relations", new { relCode = Id });
                }
                else
                {
                    TempData[ErrorMessageKey] = result;
                    return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка! Основна грешка при обработка на заявката за запис.";
                return RedirectToAction("AdminTasks", "Users", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> UpdateRelationStatus(string relTypeId, string srcclassCode, string srcversionCode, string srcitemCode, string destclassCode, string destversionCode, string destitemCode)
        {
            var result = await this.items.ChangeRelationStatus(relTypeId, srcclassCode, srcversionCode, srcitemCode, destclassCode, destversionCode, destitemCode);
            return Json(result);
        }

        public IActionResult AddRelationItem(string relId)
        {
            var model = new AddRelationItemViewModel { };

           // var currentRelation = await this.classification.GetRelShortInfoAsync(relId);

            model.RelationId = relId;

            return PartialView("_RelationItemModalPartial", model);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRelationItem(AddRelationItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentuser = await this.userManager.GetUserAsync(User);
                bool result = await this.classification.AddRelationItemAsync(model.RelationId, model.SrcItemId, model.DestItemId, currentuser.Id);
                if (result)
                {
                    TempData[SuccessMessageKey] = "Връзката е добавена успешно!";
                }
                else
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Проверете дали кодовете на елементите са реални и дали няма вече такава връзка.";
                }
                //return RedirectToAction("RelDetails", "Relations", new { relCode = model.RelationId });
            }
            return PartialView("_RelationItemModalPartial", model);
        }

    }
}
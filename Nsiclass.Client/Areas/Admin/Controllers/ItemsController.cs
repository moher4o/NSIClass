using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nsiclass.Data.Models;
using Nsiclass.Services;
using Nsiclass.Services.Models;
using static Nsiclass.Common.DataConstants;

namespace Nsiclass.Client.Areas.Admin.Controllers
{
    public class ItemsController : BaseController
    {

        private readonly IAdminItemsService items;
        
        private readonly UserManager<User> userManager;

        public ItemsController(UserManager<User> userManager, IAdminItemsService items)
        {
            this.items = items;
            this.userManager = userManager;
        }

        public async Task<IActionResult> ItemDetails(string classCode, string versionCode, string itemCode)
        {
            try
            {
                ClassItemServiceModel result = await this.items.GetItemInfoAsync(classCode, versionCode, itemCode);
                if (result == null)
                {
                    TempData[ErrorMessageKey] = $"Възникна грешка при извличнето на информация за елемента {classCode} {versionCode} {itemCode}";
                    return RedirectToAction("ClassItemsList", "ClassClient", new {area="", classCode, versionCode });
                }
               result.OtherCode = result.ItemCode;
               return View(result);
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Грешка!!! Възникна грешка при извличнето на информация за елемент {classCode} {versionCode} {itemCode}";
                return RedirectToAction("AdminTasks", "Users");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemDetails(ClassItemServiceModel model, string classCode, string versionCode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (!await this.items.ItemExistAsync(classCode,versionCode,model.ItemCode))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация, версия или елемент.";
                    return RedirectToAction("AdminTasks", "Users");
                }
                var currentUser = await this.userManager.GetUserAsync(User);

                var result = await this.items.EditItemDetailsAsync(model.Classif, model.Version, model.ItemCode, model.OtherCode, model.Description, model.DescriptionShort, model.DescriptionEng, model.Includes, model.IncludesMore, model.IncludesNo,currentUser.Id, DateTime.UtcNow);
                if (result.Contains("успешна"))
                {
                    TempData[SuccessMessageKey] = result;
                    return RedirectToAction("ItemDetails", "Items", new { classCode, versionCode, itemCode = model.OtherCode.Trim() });
                }
                else
                {
                    TempData[ErrorMessageKey] = result;
                    return RedirectToAction("ItemDetails", "Items", new { classCode, versionCode, itemCode = model.ItemCode });
                }
                
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка!!! Възникна грешка при редакция";

                return RedirectToAction("AdminTasks", "Users");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DestroyItem(string classCode, string versionCode, string itemCode)
        {
            try
            {
                if (!await this.items.ItemExistAsync(classCode, versionCode, itemCode))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация, версия или елемент.";
                    return RedirectToAction("AdminTasks", "Users");
                }
                var currentUser = await this.userManager.GetUserAsync(User);
                string result = await this.items.DeleteItemAsync(classCode, versionCode, itemCode, currentUser.Id);
                if (result.Contains("успешно"))
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
                TempData[ErrorMessageKey] = "Грешка!!! Възникна неочаквана грешка.";
                return RedirectToAction("AdminTasks", "Users");
            }
            return RedirectToAction("ItemDetails", "Items", new { classCode, versionCode, itemCode});
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreItem(string classCode, string versionCode, string itemCode)
        {
            try
            {
                if (!await this.items.ItemExistAsync(classCode, versionCode, itemCode))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация, версия или елемент.";
                    return RedirectToAction("AdminTasks", "Users");
                }
                var currentUser = await this.userManager.GetUserAsync(User);
                string result = await this.items.RestoreItemAsync(classCode, versionCode, itemCode, currentUser.Id);
                if (result.Contains("успешно"))
                {
                    TempData[SuccessMessageKey] = result;
                }
                else
                {
                    TempData[ErrorMessageKey] = result;
                }
                return RedirectToAction("ItemDetails", "Items", new { classCode, versionCode, itemCode });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка!!! Възникна неочаквана грешка.";
                return RedirectToAction("AdminTasks", "Users");
            }
        }

        public async Task<IActionResult> UpdateRelationStatus(string relTypeId, string srcclassCode, string srcversionCode, string srcitemCode, string destclassCode, string destversionCode, string destitemCode)
        {
            var result = await this.items.ChangeRelationStatus(relTypeId, srcclassCode, srcversionCode, srcitemCode, destclassCode, destversionCode, destitemCode);
            return Json(result);
        }


    }
}
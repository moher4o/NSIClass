using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nsiclass.Data.Models;
using Nsiclass.Services;
using Nsiclass.Services.Models;
using static Nsiclass.Common.DataConstants;

namespace Nsiclass.Client.Areas.Admin.Controllers
{
    public class VersionController : BaseController
    {

        private readonly IAdminVersionService versions;
        private readonly UserManager<User> userManager;
        private readonly IManageFilesService files;

        public VersionController(UserManager<User> userManager, IAdminVersionService versions, IManageFilesService files)
        {
            this.versions = versions;
            this.userManager = userManager;
            this.files = files;
        }


        public async Task<IActionResult> VersionDetails(string classCode, string versionCode)
        {
            try
            {
                VersionServiceModel result = await this.versions.GetVersionDetailsAsync(classCode, versionCode);
                if (result == null)
                {
                    TempData[ErrorMessageKey] = $"Възникна грешка при извличнето на информация за класификационна версия {classCode} {versionCode}";
                    return RedirectToAction("ClassDetails", "Class", new { classCode });
                }

                return View(result);
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Грешка!!! Възникна грешка при извличнето на информация за класификационна версия {classCode} {versionCode}";
                return RedirectToAction("AdminTasks", "Users");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VersionDetails(VersionServiceModel model)
        {
            try
            {
                if (!await this.versions.IsClassVersionExistAsync(model.Classif, model.Version))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация или версия.";
                    return RedirectToAction("AdminTasks", "Users");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = await this.versions.EditVersionAsync(model.Classif, model.Version, model.Parent, model.Publications, model.Remarks, model.ByLow, model.Valid_From, model.Valid_To, model.UseAreas);
                if (result.Contains("успешна"))
                {
                    TempData[SuccessMessageKey] = $"Редакцията на {model.Classif} {model.Version} беше успешна";
                }
                else
                {
                    TempData[ErrorMessageKey] = result;
                    return RedirectToAction("AdminTasks", "Users");
                }
                return RedirectToAction("VersionDetails", "Version", new { classCode = model.Classif, versionCode = model.Version });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка!!! Възникна грешка при редакция";

                return RedirectToAction("AdminTasks", "Users");
            }
            
            
        }

        [HttpPost]
        [Authorize(Roles = "Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DestroyVersion(string classCode, string versionCode)
        {
            try
            {
                if (!await this.versions.IsClassVersionExistAsync(classCode, versionCode))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация или версия.";
                    return RedirectToAction("AdminTasks", "Users");
                }

                var result = await this.versions.DeleteClassVersionAsync(classCode, versionCode);
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
            return RedirectToAction("VersionDetails", "Version", new { classCode, versionCode});
        }

        [HttpPost]
        [Authorize(Roles = "Програмист")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreVersion(string classCode, string versionCode)
        {
            try
            {
                if (!await this.versions.IsClassVersionExistAsync(classCode,versionCode))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация или версия.";
                    return RedirectToAction("AdminTasks", "Users");
                }

                string result = await this.versions.RestoreClassVersionAsync(classCode, versionCode);
                if (result.Contains("успешно"))
                {
                    TempData[SuccessMessageKey] = result;
                }
                else
                {
                    TempData[ErrorMessageKey] = result;
                }
                return RedirectToAction("VersionDetails", "Version", new { classCode, versionCode });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка!!! Възникна неочаквана грешка.";
                return RedirectToAction("AdminTasks", "Users");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Програмист, Администратор")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateVersion(string classCode, string versionCode)
        {
            try
            {
                if (!await this.versions.IsClassVersionExistAsync(classCode, versionCode))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация или версия.";
                    return RedirectToAction("AdminTasks", "Users");
                }

                string result = await this.versions.DeactivateClassVersionAsync(classCode, versionCode);
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
            return RedirectToAction("VersionDetails", "Version", new { classCode, versionCode });
        }


        [HttpPost]
        [Authorize(Roles = "Програмист, Администратор")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateVersion(string classCode, string versionCode)
        {
            try
            {
                if (!await this.versions.IsClassVersionExistAsync(classCode, versionCode))
                {
                    TempData[ErrorMessageKey] = "Грешка!!! Грешен код на класификация или версия.";
                    return RedirectToAction("AdminTasks", "Users");
                }

                string result = await this.versions.ActivateClassVersionAsync(classCode, versionCode);
                if (result.Contains("успешно"))
                {
                    TempData[SuccessMessageKey] = result;
                }
                else
                {
                    TempData[ErrorMessageKey] = result;
                }
                return RedirectToAction("VersionDetails", "Version", new { classCode, versionCode });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Грешка!!! Възникна неочаквана грешка.";
                return RedirectToAction("AdminTasks", "Users");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file1, string classCode, string versionCode)
        {

            if (file1 != null)
            {
                var result = await this.files.AddFile(classCode, versionCode, file1);
                if (result)
                {
                    return Json($"{file1.FileName}");
                }
                return Json(false);
            }
            else
            {
                return Json(false);
            }
        }

        public IActionResult FilesList(string classCode, string versionCode)
        {
            var result = this.files.GetFilesInDirectory(classCode, versionCode);
            return Json(result);
        }

        
        public IActionResult DeleteFile(string classCode, string versionCode, string fileName)
        {
            bool result = this.files.DeleteFile(classCode, versionCode, fileName);
            return Json(result);
        }

        public IActionResult Temporary()
        {
            return View();
        }


        //public IActionResult UploadFiles()
        //{
        //    string FileName = "";
        //    var files = Request.Form.Files;
        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
        //        string filename = Path.GetFileName(Request.Files[i].FileName);

        //        IFormFile file = files[i];
        //        string fname;
        //        // Checking for Internet Explorer    
        //        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //        {
        //            string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //            fname = testfiles[testfiles.Length - 1];
        //        }
        //        else
        //        {
        //            fname = file.FileName;
        //            FileName = file.FileName;
        //        }

        //        fname = file.FileName;
        //        FileName = file.FileName;


        //        //Get the complete folder path and store the file inside it.
        //        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
        //        file.SaveAs(fname);
        //    }
        //    return Json(FileName);
        //}

    }
}
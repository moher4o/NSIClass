using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nsiclass.Services;
using Nsiclass.Services.Models;
using static Nsiclass.Common.DataConstants;

namespace Nsiclass.Client.Areas.Admin.Controllers
{
    [Authorize(Roles = "Администратор, Програмист")]
    public class LinksController : BaseController
    {
        private readonly IAdminLinksService links;
        public LinksController(IAdminLinksService links)
        {
            this.links = links;
        }

        public IActionResult AllLinks()
        {
            var currentLinks = links.GetAllLinks();
            return View(currentLinks);
        }

        public IActionResult AddSystem()
        {
            return View(new AdminLinksServiceModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddSystem(AdminLinksServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var result = await this.links.CreateLink(model.Name, model.Link);
                if (!result)
                {
                    TempData[ErrorMessageKey] = $"Неуспешно създаване на връзка \"{model.Name}\"";
                    return RedirectToAction(nameof(AllLinks));
                }
                else
                {
                    TempData[SuccessMessageKey] = "Записа е успешен";
                    return RedirectToAction(nameof(AllLinks));
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Неуспешно създаване на връзка \"{model.Name}\"";
                return RedirectToAction(nameof(AllLinks));
            }
          
        }

        public IActionResult EditLink(int linkId)
        {

            return View(this.links.GetLinkById(linkId));
        }

        [HttpPost]
        public async Task<IActionResult> EditLink(AdminLinksServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool result = await this.links.EditLink(model);
                if (!result)
                {
                    TempData[ErrorMessageKey] = $"Неуспешно редактиране на \"{model.Name}\"";
                    return RedirectToAction(nameof(AllLinks));
                }
                else
                {
                    TempData[SuccessMessageKey] = "Записа е успешен";
                    return RedirectToAction(nameof(AllLinks));
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Неуспешно редактиране на \"{model.Name}\"";
                return RedirectToAction(nameof(AllLinks));
            }

        }

        [HttpPost]
        public async Task<IActionResult> DestroyLink(string linkId)
        {
            try
            {
                if (linkId != null)
                {
                    bool result = await this.links.DeleteLink(int.Parse(linkId));

                    if (result)
                    {
                        TempData[SuccessMessageKey] = "Връзката е изтрита";
                        return RedirectToAction(nameof(AllLinks));
                    }
                    else
                    {
                        TempData[ErrorMessageKey] = $"Неуспешно изтриване";
                        return RedirectToAction(nameof(AllLinks));
                    }
                }
                else
                {
                    TempData[ErrorMessageKey] = $"Несъществуваш номер на връзка";
                    return RedirectToAction(nameof(AllLinks));
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Error! Нещо се обърка...";
                return RedirectToAction(nameof(AllLinks));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreLink(string linkId)
        {
            try
            {
                if (linkId != null)
                {
                    bool result = await this.links.RestoreLink(int.Parse(linkId));

                    if (result)
                    {
                        TempData[SuccessMessageKey] = "Връзката е възстановена";
                        return RedirectToAction(nameof(AllLinks));
                    }
                    else
                    {
                        TempData[ErrorMessageKey] = $"Неуспешно възстановяване";
                        return RedirectToAction(nameof(AllLinks));
                    }
                }
                else
                {
                    TempData[ErrorMessageKey] = $"Несъществуваш номер на връзка";
                    return RedirectToAction(nameof(AllLinks));
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Error! Нещо се обърка...";
                return RedirectToAction(nameof(AllLinks));
            }
        }


    }
}
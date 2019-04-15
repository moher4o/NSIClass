using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nsiclass.Client.Models;
using Nsiclass.Services.Models;
using Nsiclass.Services;

namespace Nsiclass.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminLinksService links;

        public HomeController(IAdminLinksService links)
        {
            this.links = links;
        }

        public IActionResult OtherLinks()
        {
            var currentLinks = links.GetAllLinks();
            return View(currentLinks);
        }



        public IActionResult Index()
        {
            return RedirectToAction("ClassList", "ClassClient", new { area = ""});
        }

        public IActionResult About()
        {
            ViewData["Message"] = "ИС Класификации в процес на разработка";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "avukov@nsi.bg";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

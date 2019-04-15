using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nsiclass.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Администратор, Програмист")]
    public class BaseController : Controller
    {
    }
}

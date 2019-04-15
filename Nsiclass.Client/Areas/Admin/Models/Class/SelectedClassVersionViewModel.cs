using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Nsiclass.Client.Areas.Admin.Models.Class
{
    public class SelectedClassVersionViewModel
    {
        public string ClassCode { get; set; }

        public IList<SelectListItem> VersionCodes { get; set; } = new List<SelectListItem>();
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Nsiclass.Client.Areas.Admin.Models.Relations
{
    public class ImportNewRelItemsViewModel
    {
        public IList<SelectListItem> RelCodes { get; set; } = new List<SelectListItem>();

    }
}

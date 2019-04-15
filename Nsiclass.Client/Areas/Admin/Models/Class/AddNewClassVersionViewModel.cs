using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nsiclass.Client.Areas.Admin.Models.Class
{
    public class AddNewClassVersionViewModel
    {

        [Required]
        [Display(Name = "Класификация *")]
        public IList<SelectListItem> ClassCodes { get; set; } = new List<SelectListItem>();

        [MaxLength(250)]
        [Display(Name = "Допълнителна информация")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Версия на класификацията")]
        public string Version { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Valid_From { get; set; } = DateTime.UtcNow.Date;

        [DataType(DataType.Date)]
        public DateTime? Valid_To { get; set; } = null;

    }
}

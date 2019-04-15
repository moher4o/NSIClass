using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Client.Areas.Admin.Models.Relations
{
    public class AddNewClassRelTypeViewModel
    {
        [Required]
        [MaxLength(250)]
        [Display(Name = "Описание *")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Код на класификация източник *")]
        public IList<SelectListItem> SourceClassId { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Код на класификационна версия източник *")]
        public IList<SelectListItem> SourceClassVersionId { get; set; } = new List<SelectListItem>();


        [Required]
        [Display(Name = "Код на класификация приемник *")]
        public IList<SelectListItem> DestClassId { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Код на класификационна версия приемник *")]
        public IList<SelectListItem> DestClassVersionId { get; set; } = new List<SelectListItem>();

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Начална дата *")]
        public DateTime Valid_From { get; set; } = DateTime.UtcNow.Date;

        [DataType(DataType.Date)]
        [Display(Name = "Крайна дата *")]
        public DateTime? Valid_To { get; set; }

    }
}

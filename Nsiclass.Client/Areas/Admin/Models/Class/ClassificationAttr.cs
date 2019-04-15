using System;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Client.Areas.Admin.Models.Class
{
    public class ClassificationAttr
    {
        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Код на класификацията")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(250)]
        [Display(Name = "Име на български")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(250)]
        [Display(Name = "Име на английски")]
        public string NameEng { get; set; }

        [Display(Name = "Йерархична ли е")]
        public bool IsHierachy { get; set; } = false;

        [MaxLength(5000)]
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

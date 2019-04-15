using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Client.Areas.Admin.Models.Relations
{
    public class AddRelationItemViewModel
    {
        [Required]
        public string RelationId { get; set; }

        //[Required]
        //public string SrcClassId { get; set; }

        //[Required]
        //public string SrcVersionId { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Код на елемент източник:")]
        public string SrcItemId { get; set; }

        //[Required]
        //public string DestClassId { get; set; }

        //[Required]
        //public string DestVersionId { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Код на елемент цел:")]
        public string DestItemId { get; set; }

    }
}

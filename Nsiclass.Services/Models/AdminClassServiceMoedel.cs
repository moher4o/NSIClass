using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Services.Models
{
   public class AdminClassServiceMoedel : IMapFrom<TC_Classifications>
    {
        [Display(Name = "Код на класификацията")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(250)]
        [Display(Name = "Име на български")]
        public string Name { get; set; }

        [MaxLength(250)]
        [Display(Name = "Име на английски")]
        public string NameEng { get; set; }

        [Display(Name = "Йерархична ли е")]
        public bool IsHierachy { get; set; }

        [MaxLength(5000)]
        [Display(Name = "Допълнителна информация")]
        public string Remarks { get; set; }

        [Display(Name = "Изтрита ли е")]
        public bool isDeleted { get; set; }

        public IList<TC_Classif_Vers> Versions { get; set; } = new List<TC_Classif_Vers>();

        public IList<TC_Classif_Rel_Types> AsSourceClassification { get; set; } = new List<TC_Classif_Rel_Types>();

        public IList<TC_Classif_Rel_Types> AsDestClassification { get; set; } = new List<TC_Classif_Rel_Types>();

    }
}

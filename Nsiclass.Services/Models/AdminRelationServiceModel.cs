using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Services.Models
{
    public class AdminRelationServiceModel : IMapFrom<TC_Classif_Rel_Types>
    {
        public string Id { get; set; }

        [Display(Name = "Име на релацията")]
        public string Description { get; set; }

        [Display(Name = "Код на класификацията източник")]
        public string SrcClassifId { get; set; }

        [Display(Name = "Код на версията източник")]
        public string SrcVersionId { get; set; }

        [Display(Name = "Име на класификацията източник")]
        public string SourceClassificationName { get; set; }

        [Display(Name = "Код на класификацията цел")]
        public string DestClassifId { get; set; }

        [Display(Name = "Код на версията цел")]
        public string DestVersionId { get; set; }

        [Display(Name = "Име на класификацията цел")]
        public string DestClassificationName { get; set; }

        [Display(Name = "Статус")]
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Брой на редовете")]
        public string RelationsCount { get; set; }


    }
}

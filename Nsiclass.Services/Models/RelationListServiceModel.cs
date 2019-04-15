using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Services.Models
{
    public class RelationListServiceModel : AdminRelationServiceModel, IMapFrom<TC_Classif_Rel_Types>, IHaveCustomMapping
    {
        [Display(Name = "Валидна от")]
        [DataType(DataType.Date)]
        public string Valid_From_Date { get; set; }

        [Display(Name = "Валидна до")]
        [DataType(DataType.Date)]
        public string Valid_To_Date { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<TC_Classif_Rel_Types, RelationListServiceModel>()
                   .ForMember(u => u.Valid_From_Date, cfg => cfg.MapFrom(t => t.Valid_From.Date.ToShortDateString()))
                   .ForMember(u => u.Valid_To_Date, cfg => cfg.MapFrom(t => t.Valid_To.Value.Date))
                   .ForMember(u => u.RelationsCount, cfg => cfg.MapFrom(t => t.Relations.Count))
                   .ForMember(u => u.SourceClassificationName, cfg => cfg.MapFrom(t => t.SourceClassification.Name))
                   .ForMember(u => u.DestClassificationName, cfg => cfg.MapFrom(t => t.DestClassification.Name));
        }
    }
}

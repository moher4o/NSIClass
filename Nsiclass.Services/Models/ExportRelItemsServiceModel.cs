using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nsiclass.Services.Models
{
   public class ExportRelItemsServiceModel : IMapFrom<TC_Classif_Rels>, IHaveCustomMapping
    {
        public string SrcClassif { get; set; }

        public string SrcVer { get; set; }

        public string SrcItemId { get; set; }

        public string SrcItemName { get; set; }

        public string DestClassif { get; set; }

        public string DestVer { get; set; }

        public string DestItemId { get; set; }

        public string DestItemName { get; set; }

        public string RelationTypeName { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<TC_Classif_Rels, ExportRelItemsServiceModel>()
            .ForMember(i => i.SrcItemName, cfg => cfg.MapFrom(t => t.SrcItem.Description ))
            .ForMember(i => i.DestItemName, cfg => cfg.MapFrom(t => t.DestItem.Description))
            .ForMember(i => i.RelationTypeName, cfg => cfg.MapFrom(t => t.RelationType.Description));
        }
    }
}

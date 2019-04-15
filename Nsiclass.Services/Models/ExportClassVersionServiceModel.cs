using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nsiclass.Services.Models
{
    public class ExportClassVersionServiceModel : IMapFrom<TC_Classif_Items>, IHaveCustomMapping
    {
        public string Classif { get; set; }

        public string Version { get; set; }

        public string ItemCode { get; set; }

        public string Description { get; set; }

        public string DescriptionEng { get; set; }

        public string OrderNo { get; set; }

        public string ParentItemCode { get; set; }

        public string ItemLevel { get; set; }

        public string IsLeaf { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<TC_Classif_Items, ExportClassVersionServiceModel>()
            .ForMember(i => i.IsLeaf, cfg => cfg.MapFrom(i => i.IsLeaf ? "Yes" : "No"))
            .ForMember(i => i.OrderNo, cfg => cfg.MapFrom(i => i.OrderNo.HasValue ? i.OrderNo.Value.ToString() : string.Empty))
            .ForMember(i => i.ItemLevel, cfg => cfg.MapFrom(i => i.ItemLevel.ToString()));
        }
    }
}

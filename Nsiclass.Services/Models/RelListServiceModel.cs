using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;

namespace Nsiclass.Services.Models
{
    public class RelListServiceModel : AdminRelationServiceModel, IMapFrom<TC_Classif_Rel_Types>, IHaveCustomMapping
    {
        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<TC_Classif_Rel_Types, RelListServiceModel>()
                   .ForMember(u => u.RelationsCount, cfg => cfg.MapFrom(t => t.Relations.Count));
        }

    }
}

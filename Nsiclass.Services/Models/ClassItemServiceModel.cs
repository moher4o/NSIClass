using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Services.Models
{
    public class ClassItemServiceModel : IMapFrom<TC_Classif_Items>, IHaveCustomMapping
    {
        public string Classif { get; set; }

        public string Version { get; set; }

        [Display(Name = "Код на елемента:")]
        public string ItemCode { get; set; }

        [Display(Name = "Код на елемента:")]
        public string OtherCode { get; set; }

        [Display(Name = "Име:")]
        public string Description { get; set; }

        [Display(Name = "Стандартно кратко име:")]
        public string DescriptionShort { get; set; }

        [Display(Name = "Name:")]
        public string DescriptionEng { get; set; }

        [Display(Name = "Включва:")]
        public string Includes { get; set; }

        [Display(Name = "Включва още:")]
        public string IncludesMore { get; set; }

        [Display(Name = "Не включва:")]
        public string IncludesNo { get; set; }

        public string ParentItemCode { get; set; }

        [Display(Name = "Име на родителския елемент:")]
        public string ParentItemName { get; set; }

        [Display(Name = "Включва елементите:")]
        public ICollection<TC_Classif_Items> ChildItems { get; set; } = new List<TC_Classif_Items>();

        [Display(Name = "Релации в които е цел:")]
        public IList<ItemsRelationServiceModel> SrcRelations { get; set; }

        [Display(Name = "Релации в които е източник:")]
        public IList<ItemsRelationServiceModel> DestRelations { get; set; }

        [Display(Name = "Ранг на елемента:")]
        public int ItemLevel { get; set; }

        [Display(Name = "Листо ли е:")]
        public bool IsLeaf { get; set; }

        [Display(Name = "Създаден от потребител:")]
        public string EnteredByUserName { get; set; }

        [Display(Name = "Въведен на:")]
        [DataType(DataType.Date)]
        public DateTime EntryTime { get; set; }

        [Display(Name = "Променен от потребител:")]
        public string ModifiedByUserName { get; set; }

        [Display(Name = "Променен на:")]
        [DataType(DataType.Date)]
        public DateTime? ModifyTime { get; set; }

        public bool IsDeleted { get; set; } = false;

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<TC_Classif_Items, ClassItemServiceModel>()
                   .ForMember(u => u.DestRelations, cfg => cfg.MapFrom(t => t.SrcRelations.OrderBy(sr => sr.RelationTypeId).Select(sr => new ItemsRelationServiceModel
                   {
                       Id = sr.RelationTypeId,
                       ClassCode = sr.DestClassif,
                       VersionCode = sr.DestVer,
                       ItemCode = sr.DestItemId,
                       ItemName = sr.DestItem.Description,
                       relationTypeName = sr.RelationType.Description,
                       isDeleted = sr.IsDeleted
                   }).ToList()))
                                      .ForMember(u => u.SrcRelations, cfg => cfg.MapFrom(t => t.DestRelations.OrderBy(sr => sr.RelationTypeId).Select(sr => new ItemsRelationServiceModel
                                      {
                                          Id = sr.RelationTypeId,
                                          ClassCode = sr.SrcClassif,
                                          VersionCode = sr.SrcVer,
                                          ItemCode = sr.SrcItemId,
                                          ItemName = sr.SrcItem.Description,
                                          relationTypeName = sr.RelationType.Description,
                                          isDeleted = sr.IsDeleted
                                      }).ToList()))
                   .ForMember(u => u.EnteredByUserName, cfg => cfg.MapFrom(t => t.CreateUser.Name))
                   .ForMember(u => u.ParentItemName, cfg => cfg.MapFrom(t => t.ParentItem.Description))
                   .ForMember(u => u.ModifiedByUserName, cfg => cfg.MapFrom(t => t.ModifyUser.Name));

        }
    }
}

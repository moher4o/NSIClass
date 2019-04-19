using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Services.Models
{
   public class VersionServiceModel : IMapFrom<TC_Classif_Vers>, IHaveCustomMapping
    {
        [Display(Name = "Код на класификацията")]
        public string Classif { get; set; }

        [Display(Name = "Име на класификацията")]
        public string ClassifName { get; set; }

        [Display(Name = "Версия на класификацията")]
        public string Version { get; set; }

        [Display(Name = "Валидна от")]
        [DataType(DataType.Date)]
        public DateTime Valid_From { get; set; }

        [Display(Name = "Валидна до")]
        [DataType(DataType.Date)]
        public DateTime? Valid_To { get; set; }

        [MaxLength(5000)]
        [Display(Name = "Общо описание")]
        public string Remarks { get; set; }

        [Display(Name = "Изтрита ли е?")]
        public bool isDeleted { get; set; }

        [Display(Name = "Статус")]
        public bool isActive { get; set; }

        [Display(Name = "Брой елементи")]
        public int ItemsCount { get; set; }

        [MaxLength(50)]
        [Display(Name = "Предшественик")]
        public string Parent { get; set; }

        [MaxLength(500)]
        [Display(Name = "Правно основание")]
        public string ByLow { get; set; }

        [MaxLength(500)]
        [Display(Name = "Публикации")]
        public string Publications { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Области на приложение")]
        public string UseAreas { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<TC_Classif_Vers, VersionServiceModel>()
            .ForMember(v => v.ClassifName, cfg => cfg.MapFrom(t => t.Classification.Name))
            .ForMember(v => v.ItemsCount, cfg => cfg.MapFrom(t => t.Items.Count));
        }
    }
}

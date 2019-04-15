using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Services.Models
{
    public class AdminRelationsDetailsServiceModel : AdminRelationServiceModel, IMapFrom<TC_Classif_Rel_Types>
    {
        [Display(Name = "Валидна от")]
        [DataType(DataType.Date)]
        public DateTime Valid_From { get; set; }

        [Display(Name = "Валидна до")]
        [DataType(DataType.Date)]
        public DateTime? Valid_To { get; set; }

        public IList<TC_Classif_Rels> Relations { get; set; } = new List<TC_Classif_Rels>();


    }
}

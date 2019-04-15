using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nsiclass.Services.Models
{
    public class ClassListServiceModel : IMapFrom<TC_Classifications>
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string NameEng { get; set; }

        public string Remarks { get; set; }

        public bool IsHierachy { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<TC_Classif_Vers> Versions { get; set; } = new List<TC_Classif_Vers>();

    }
}

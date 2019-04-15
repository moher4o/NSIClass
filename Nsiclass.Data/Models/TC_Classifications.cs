using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Data.Models
{
    public class TC_Classifications
    {

        public string Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string NameEng { get; set; }

        public bool IsHierachy { get; set; } = false;

        [MaxLength(5000)]
        public string Remarks { get; set; }

        public bool isDeleted { get; set; } = false;

        public ICollection<TC_Classif_Vers> Versions { get; set; } = new List<TC_Classif_Vers>();

        public ICollection<TC_Classif_Rel_Types> AsSourceClassification { get; set; } = new List<TC_Classif_Rel_Types>();

        public ICollection<TC_Classif_Rel_Types> AsDestClassification { get; set; } = new List<TC_Classif_Rel_Types>();

    }
}

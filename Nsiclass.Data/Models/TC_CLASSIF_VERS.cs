using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Data.Models
{
    public class TC_Classif_Vers
    {
        public string Classif { get; set; }

        public TC_Classifications Classification { get; set; }

        public string Version { get; set; }

        public DateTime Valid_From { get; set; }

        public DateTime? Valid_To { get; set; }

        [MaxLength(5000)]
        public string Remarks { get; set; }

        public bool isDeleted { get; set; } = false;

        public ICollection<TC_Classif_Items> Items { get; set; } = new List<TC_Classif_Items>();

        public ICollection<TC_Classif_Rel_Types> AsSourceClassificationVersion { get; set; } = new List<TC_Classif_Rel_Types>();

        public ICollection<TC_Classif_Rel_Types> AsDestClassificationVersion { get; set; } = new List<TC_Classif_Rel_Types>();

        [MaxLength(50)]
        public string Parent { get; set; }

        [MaxLength(500)]
        public string ByLow { get; set; }

        [MaxLength(500)]
        public string Publications { get; set; }

        [MaxLength(1000)]
        public string UseAreas { get; set; }

        public bool isActive { get; set; } = true;

    }
}

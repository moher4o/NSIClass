using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Data.Models
{
    public class TC_Classif_Rel_Types
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public string SrcClassifId { get; set; }

        [Required]
        public string SrcVersionId { get; set; }

        public TC_Classifications SourceClassification { get; set; }

        public TC_Classif_Vers SourceClassificationVersion { get; set; }

        [Required]
        public string DestClassifId { get; set; }

        [Required]
        public string DestVersionId { get; set; }

        public TC_Classifications DestClassification { get; set; }

        public TC_Classif_Vers DestClassificationVersion { get; set; }

        public DateTime Valid_From { get; set; }

        public DateTime? Valid_To { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<TC_Classif_Rels> Relations { get; set; } = new List<TC_Classif_Rels>();
    }
}

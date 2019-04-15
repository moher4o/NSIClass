using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Data.Models
{
    public class TC_Classif_Rels
    {
        public string SrcClassif { get; set; }

        public string SrcVer { get; set; }

        public string SrcItemId { get; set; }

        public TC_Classif_Items SrcItem { get; set; }

        public string DestClassif { get; set; }

        public string DestVer { get; set; }

        public string DestItemId { get; set; }

        public TC_Classif_Items DestItem { get; set; }

        [Required]
        public string RelationTypeId { get; set; }

        public TC_Classif_Rel_Types RelationType { get; set; }

        public string EnteredByUserId { get; set; }

        public User CreateUser { get; set; }

        public DateTime EntryTime { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}

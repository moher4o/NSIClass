using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Data.Models
{
    public class TC_Classif_Items
    {
        public string Classif { get; set; }

        public string Version { get; set; }

        public TC_Classif_Vers ClassifVersion  { get; set; }

        public string ItemCode { get; set; }

        public string OtherCode { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public string DescriptionShort { get; set; }

        [MaxLength(1000)]
        public string DescriptionEng { get; set; }

        [MaxLength(5000)]
        public string Includes { get; set; }

        [MaxLength(1000)]
        public string IncludesMore { get; set; }

        [MaxLength(1000)]
        public string IncludesNo { get; set; }

        public int? OrderNo { get; set; }

        public string ParentItemCode { get; set; }

        public TC_Classif_Items ParentItem { get; set; }

        public ICollection<TC_Classif_Items> ChildItems { get; set; } = new List<TC_Classif_Items>();

        public ICollection<TC_Classif_Rels> SrcRelations { get; set; } = new List<TC_Classif_Rels>();

        public ICollection<TC_Classif_Rels> DestRelations { get; set; } = new List<TC_Classif_Rels>();

        public int ItemLevel { get; set; }

        public bool IsLeaf { get; set; }

        public string EnteredByUserId { get; set; }

        public User CreateUser { get; set; }

        public DateTime EntryTime { get; set; }

        public string ModifiedByUserId { get; set; }

        public User ModifyUser { get; set; }

        public DateTime? ModifyTime { get; set; }

        public bool IsDeleted { get; set; } = false;


    }
}

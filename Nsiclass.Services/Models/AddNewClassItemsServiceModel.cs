using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nsiclass.Services.Models
{
    public class AddNewClassItemsServiceModel
    {
        public string Classif { get; set; }

        public string Version { get; set; }

        public string ItemCode { get; set; }

        public string OtherCode { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string DescriptionEng { get; set; }

        public int? OrderNo { get; set; }

        public string ParentItemCode { get; set; }

        public int ItemLevel { get; set; }

        public bool IsLeaf { get; set; }

        public string EnteredByUserId { get; set; }

        public DateTime EntryTime { get; set; }

    }
}

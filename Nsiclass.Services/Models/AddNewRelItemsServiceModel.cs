using System;
using System.Collections.Generic;
using System.Text;

namespace Nsiclass.Services.Models
{
    public class AddNewRelItemsServiceModel
    {
        public string SrcClassif { get; set; }

        public string SrcVer { get; set; }

        public string SrcItemId { get; set; }

        public string DestClassif { get; set; }

        public string DestVer { get; set; }

        public string DestItemId { get; set; }

        public string RelationTypeId { get; set; }

        public string EnteredByUserId { get; set; }

        public DateTime EntryTime { get; set; }

    }
}

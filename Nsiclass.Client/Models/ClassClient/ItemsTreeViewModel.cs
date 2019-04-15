using Nsiclass.Data.Models;
using System.Collections.Generic;

namespace Nsiclass.Client.Models.ClassClient
{
    public class ItemsTreeViewModel
    {
        public string ClassCode { get; set; }

        public string VersionCode { get; set; }

        public bool isSearchResult { get; set; } = false;

        public ICollection<TC_Classif_Items> Items { get; set; } = new List<TC_Classif_Items>();
    }
}

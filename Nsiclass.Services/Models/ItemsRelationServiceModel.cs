using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nsiclass.Services.Models
{
    public class ItemsRelationServiceModel
    
    {
        public string Id { get; set; }
        public string ClassCode { get; set; }
        public string VersionCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public bool isDeleted { get; set; }
        public string relationTypeName { get; set; }

     }
}

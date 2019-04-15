using AutoMapper;
using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System;

namespace Nsiclass.Services.Models
{
    public class AdminUserServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public string ParentUser { get; set; }

        public string DepartmentName { get; set; }

        public string Phone { get; set; }

        public bool isDeleted { get; set; }



        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<User, AdminUserServiceModel>()
                   .ForMember(u => u.ParentUser, cfg => cfg.MapFrom(t => t.CreateUser.UserName))
                   .ForMember(u => u.DepartmentName, cfg => cfg.MapFrom(t => t.Department.Name));
        }
    }
}

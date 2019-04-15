using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Nsiclass.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public string CreateUserId { get; set; }

        public User CreateUser { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public bool isDeleted { get; set; } = false;

        public ICollection<User> UsersCreated { get; set; } = new List<User>();

        public ICollection<TC_Classif_Items> ItemsCreated { get; set; } = new List<TC_Classif_Items>();

        public ICollection<TC_Classif_Items> ItemsModified { get; set; } = new List<TC_Classif_Items>();

        public ICollection<TC_Classif_Rels> RelsCreated { get; set; } = new List<TC_Classif_Rels>();
    }
}

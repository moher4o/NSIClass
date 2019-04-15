using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Data.Models
{
    public class Department
    {
        
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<User> Employies { get; set; } = new List<User>();

        public bool isDeleted { get; set; } = false;
    }
}

using Nsiclass.Common.Mapping;
using Nsiclass.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Services.Models
{
    public class AdminLinksServiceModel : IMapFrom<PartnersLinks>
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Име на връзката")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Интернет адрес")]
        public string Link { get; set; }

        public bool isDeleted { get; set; }
    }
}

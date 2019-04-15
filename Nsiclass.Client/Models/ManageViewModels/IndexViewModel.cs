using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nsiclass.Client.Models.ManageViewModels
{
    public class IndexViewModel
    {

        [Display(Name = "Потребителско име")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Електронна поща")]
        public string Email { get; set; }

        [Display(Name = "Мобилен телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Служебен телефон")]
        public string Phone { get; set; }

        public string StatusMessage { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Client.Areas.Admin.Models.Users
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Поща")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Име на потребителя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Потребителско име:")]
        public string Username { get; set; }

        [Display(Name = "Служебен телефон")]
        public string Phone { get; set; }

        [Display(Name = "Мобилен телефон")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Месторабота")]
        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "Роли на потребителя")]
        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}

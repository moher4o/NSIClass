using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Client.Areas.Admin.Models.Users
{
    public class CreateUserViewModel
    {
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

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди паролата")]
        [Compare("Password", ErrorMessage = "Паролата и потвърждението не съвпадат!")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Месторабота")]
        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "Роли на потребителя")]
        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}

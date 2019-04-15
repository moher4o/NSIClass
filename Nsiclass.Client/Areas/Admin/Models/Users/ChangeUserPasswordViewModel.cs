using System.ComponentModel.DataAnnotations;

namespace Nsiclass.Client.Areas.Admin.Models.Users
{
    public class ChangeUserPasswordViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Потребителско име:")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди паролата")]
        [Compare("Password", ErrorMessage = "Паролата и потвърждението не съвпадат!")]
        public string ConfirmPassword { get; set; }

    }
}

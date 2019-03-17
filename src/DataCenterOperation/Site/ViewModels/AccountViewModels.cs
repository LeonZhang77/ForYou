using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Site.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string Username { get; set; }
    }
}

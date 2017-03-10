using System.ComponentModel.DataAnnotations;

namespace WebDemo.Web.Areas.Admin.ViewModels.Account
{
    public class LoginViewModel
    {
        public string TenancyName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
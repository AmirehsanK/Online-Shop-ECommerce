using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Security
{
    public class ReCaptcha
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Token { get; set; }
    }

    public class GoogleRecaptchaViewModel
    {
        public static string SiteKey { get; set; }

        public static string SecretKey { get; set; }
    }

    public class GoogleRecaptchaForViewViewModel
    {
        public string SiteKey { get; set; }

    }
}

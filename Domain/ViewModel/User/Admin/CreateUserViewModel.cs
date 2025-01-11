using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User.Admin;

public class CreateUserViewModel
{
    [MaxLength(200)]
    [Display(Name = "نام")]

    public string? FirstName { get; set; }

    [Display(Name = " نام خانوادگی")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]

    public string? LastName { get; set; }

    [Display(Name = "ایمیل")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]

    public string Email { get; set; }

    [Display(Name = "کلمه عبور")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    [DataType(DataType.Password)]

    public string Password { get; set; }

    [Display(Name = "ادمین")] public bool IsAdmin { get; set; }

    [Display(Name = "فعال است")] public bool IsEmailActive { get; set; }
}
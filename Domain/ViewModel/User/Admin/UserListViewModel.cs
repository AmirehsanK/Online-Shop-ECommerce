using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User.Admin;

public class UserListViewModel
{
    public int Id { get; set; }

    [Display(Name = "نام")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    public string? FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    public string? LastName { get; set; }

    [Display(Name = "ایمیل")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل شما معتبر نیست")]
    public string Email { get; set; }

    public bool IsDeleted { get; set; }
}
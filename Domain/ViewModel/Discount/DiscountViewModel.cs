using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Discount;

public class DiscountViewModel
{
    public int Id { get; set; }

    [Display(Name = "کد تخفیف")] public bool IsDeleted { get; set; }

    public string? Code { get; set; }

    [Required(ErrorMessage = "لطفاً نوع تخفیف را مشخص کنید")]
    [Display(Name = "نوع تخفیف")]
    public bool IsPercentage { get; set; }

    [Required(ErrorMessage = "لطفاً مقدار تخفیف را وارد کنید")]
    [Range(0.01, double.MaxValue, ErrorMessage = "مقدار تخفیف باید بیشتر از صفر باشد")]
    [Display(Name = "مقدار تخفیف")]
    public int Value { get; set; }

    [Display(Name = "تاریخ شروع")] public string? StartDate { get; set; }

    [Display(Name = "تاریخ پایان")] public string? EndDate { get; set; }

    [Display(Name = "وضعیت فعال بودن")] public bool IsActive { get; set; } = true;

    [Display(Name = "تعداد استفاده")] public int? UsageLimit { get; set; }
}
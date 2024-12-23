using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Domain.ViewModel.Discount
{
    public class DiscountEditViewModel
    {
            public int Id { get; set; }

            [Display(Name = "کد تخفیف")]
            public string? Code { get; set; }

            [Required(ErrorMessage = "لطفاً نوع تخفیف را مشخص کنید")]
            [Display(Name = "نوع تخفیف")]
            public bool IsPercentage { get; set; }

            [Required(ErrorMessage = "لطفاً مقدار تخفیف را وارد کنید")]
            [Range(0.01, double.MaxValue, ErrorMessage = "مقدار تخفیف باید بیشتر از صفر باشد")]
            [Display(Name = "مقدار تخفیف")]
            public decimal Value { get; set; }

            [Display(Name = "تاریخ شروع")]
            public DateTime? StartDate { get; set; }

            [Display(Name = "تاریخ پایان")]
            public DateTime? EndDate { get; set; }

            [Display(Name = "وضعیت فعال بودن")]
            public bool IsActive { get; set; } = true;

            [Display(Name = "تعداد استفاده")]
            public int? UsageLimit { get; set; }
        
        //// Related Users
        //public List<int> SelectedUserIds { get; set; } = new List<int>();
        //public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();

        //// Related Products
        //public List<int> SelectedProductIds { get; set; } = new List<int>();
        //public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();

        //// Related Categories
        //public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}

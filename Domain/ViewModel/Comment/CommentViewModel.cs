using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Shared;


namespace Domain.ViewModel.Comment
{
    
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "عنوان نظر")]
        [Required(ErrorMessage = "وارد کردن عنوان نظر اجباری است.")]
        [StringLength(100, ErrorMessage = "عنوان نظر نباید بیش از 100 کاراکتر باشد.")]
        public string Title { get; set; }

        [Display(Name = "متن نظر")]
        [Required(ErrorMessage = "وارد کردن متن نظر اجباری است.")]
        [StringLength(1000, ErrorMessage = "متن نظر نباید بیش از 1000 کاراکتر باشد.")]
        public string Content { get; set; } 

        [Display(Name = "نقاط قوت")]
        public string Strengths { get; set; } 
        [Display(Name = "نقاط ضعف")]
        public string Weaknesses { get; set; } 

        [Display(Name = "وضعیت تایید")]
        public bool IsApproved { get; set; } 

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "امتیازات")]
        public List<CommentRatingViewModel> Ratings { get; set; }

        [Display(Name = "تعداد پسندها")] 
        public int Likes { get; set; } = 0;

        [Display(Name = "تعداد نپسندها")]
        public int Dislikes { get; set; } = 0;
    }

    public class CommentRatingViewModel
    {
        [Display(Name = "دسته‌بندی")]
        [Required(ErrorMessage = "دسته‌بندی امتیاز اجباری است.")]
        public RatingCategory Category { get; set; } 

        [Display(Name = "امتیاز")]
        [Required(ErrorMessage = "وارد کردن امتیاز اجباری است.")]
        [Range(1, 5, ErrorMessage = "امتیاز باید بین 1 تا 5 باشد.")]
        public int Score { get; set; } 
    }

    public class FilterCommentViewModel : Paging.BasePaging<CommentViewModel>
    {
        public string Filter { get; set; } = "notApproved";
    }
}

using System.ComponentModel.DataAnnotations;
using Domain.Entities.Question;
using Domain.Shared;

namespace Domain.ViewModel.Question;

public class QuestionListViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    public int UserId { get; set; }
    public string Question { get; set; }

    public string? Answer { get; set; }
    public bool IsClosed { get; set; }
    public bool IsConfimed { get; set; }

    public QuestionStatus QuestionStatus { get; set; }

    public DateTime DateTime { get; set; }
}

public class FilterQuestionListViewModel : Paging.BasePaging<QuestionListViewModel>
{
    public int? ProductId { get; set; }


    public ConfirmQuestion ConfirmQuestion { get; set; }

    public CloseQuestion CloseQuestion { get; set; }
}

public enum ConfirmQuestion
{
    [Display(Name = "همه")] All,
    [Display(Name = "تایید شده")] IsConfirmed,
    [Display(Name = "تایید نشده")] NotAccess
}

public enum CloseQuestion
{
    [Display(Name = "همه")] All,
    [Display(Name = "بسته شده")] IsClosed,
    [Display(Name = "باز")] Open
}
using System.ComponentModel.DataAnnotations;
using Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Domain.ViewModel.Image;

public class BannerViewModel
{
    [Key] public int Id { get; set; }

    [Required] public string Title { get; set; }

    [Required(ErrorMessage = "لطفا لینک مورد نظر را وارد کنید")]
    public string Link { get; set; }

    public string? ExpirationDate { get; set; }

    [Required(ErrorMessage = "لطفا عکس خود را اپلود کنید")]
    public IFormFile? Image { get; set; }
}

public class FilterBannerViewModel : Paging
{
    public string? Title { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
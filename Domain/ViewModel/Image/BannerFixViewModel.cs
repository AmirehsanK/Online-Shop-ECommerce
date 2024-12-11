using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.ViewModel.Image;

public class BannerFixViewModel
{
    [Key] public int Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreateDate { get; set; }

    [Required] public string Title { get; set; }

    public string Link { get; set; }
    
    public IFormFile? Image { get; set; }
    
    public string Position{get;set;}
}
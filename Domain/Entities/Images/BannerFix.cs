using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;

namespace Domain.Entities.Images;

public class BannerFix : BaseEntity
{
    [Required] public string Title { get; set; }

    public string Link { get; set; }
    
    public string Position{get;set;}

}
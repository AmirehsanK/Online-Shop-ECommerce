using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities.Images;

public class BannerFix : BaseEntity
{
    [Required] public string Title { get; set; }

    public string Link { get; set; }

    public ImageEnum.Banner Position { get; set; }
}
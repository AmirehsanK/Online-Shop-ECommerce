using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;

namespace Domain.Entities.Images;

public class Banner : BaseEntity
{
    [Required] public string Title { get; set; }

    [Required] public string Link { get; set; }

    public DateTime? ExpirationDate { get; set; }
}
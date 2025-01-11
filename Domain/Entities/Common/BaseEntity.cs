using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Common;

public abstract class BaseEntity
{
    [Key] public int Id { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime CreateDate { get; set; }
}
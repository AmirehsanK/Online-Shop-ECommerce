using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Faq;

public class FaqQuestion : BaseEntity
{
    [ForeignKey(nameof(CategoryId))] public int CategoryId { get; set; }

    public string Question { get; set; }

    public string Description { get; set; }

    #region Relation

    public FaqCategory Category { get; set; }

    #endregion
}
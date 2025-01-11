using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;

namespace Domain.Entities.Faq;

public class FaqCategory : BaseEntity
{
    [MaxLength(200)] public string Title { get; set; }

    #region Relation

    public ICollection<FaqQuestion> FaqQuestions { get; set; }

    #endregion
}
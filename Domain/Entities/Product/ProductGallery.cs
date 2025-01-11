using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Product;

public class ProductGallery : BaseEntity
{
    [ForeignKey(nameof(Product))] public int ProductId { get; set; }


    [Required] public string Image { get; set; }

    #region Relation

    public Product Product { get; set; }

    #endregion
}
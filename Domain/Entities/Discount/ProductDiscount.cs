using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Discount;

public class ProductDiscount
{
    public int Id { get; set; }

    [ForeignKey(nameof(Product))] public int ProductId { get; set; }

    [ForeignKey(nameof(Discount))] public int DiscountId { get; set; }

    #region Relations

    public Discount Discount { get; set; }
    public Product.Product Product { get; set; }

    #endregion
}
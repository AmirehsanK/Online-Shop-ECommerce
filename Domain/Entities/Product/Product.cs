using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
using Domain.Entities.Discount;
using Domain.Entities.Favorites;
using Domain.Entities.Orders;
using Domain.Entities.Question;

namespace Domain.Entities.Product;

public class Product : BaseEntity
{
    [MaxLength(200)] public string ProductName { get; set; }

    public string ShortDescription { get; set; }

    public string? Review { get; set; }

    public string? ExpertReview { get; set; }

    public string ImageName { get; set; }

    [MaxLength(200)] public int Price { get; set; }

    public int Inventory { get; set; } = 0;

    [ForeignKey(nameof(Category))] public int CategoryId { get; set; }

    #region Relation

    public ProductCategory Category { get; set; }
    public ICollection<ProductGallery> ProductGalleries { get; set; }

    public ICollection<ProductColor> ProductColors { get; set; }

    public ICollection<ProductSpecification> ProductSpecifications { get; set; }

    public ICollection<ProductDiscount> ProductDiscount { get; set; }

    public ICollection<QuestionAnswer> QuestionAnswers { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
    
    public ICollection<UserProductFavorites> FavoritedByUsers { get; set; }


    #endregion
}
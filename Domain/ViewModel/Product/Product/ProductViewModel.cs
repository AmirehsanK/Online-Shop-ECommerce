using Domain.Entities.Common;
using Domain.Shared;
using Domain.ViewModel.Product.CategoryAdmin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Product.Product
{
    public class ProductViewModel
    {
    
            public int Id { get; set; }
            [MaxLength(200)]
            public string ProductName { get; set; }

            public bool IsDeleted { get; set; }

            [MaxLength(200)]
            public string ShortDescription { get; set; }

            public string Review { get; set; }

            public string ExpertReview { get; set; }

            [MaxLength(200)]
            public string ImageName { get; set; }

            public IFormFile Image { get; set; }

            public int Price { get; set; }

            public int OffPrice { get; set; } = 0;

            public int Inventory { get; set; }

            public int SubCategoryId { get; set; }

            public string SubCategoryTitle { get; set; }

            public List<CategoryListViewModel> SubCategories { get; set; }

        #region Relation

        public ICollection<Domain.Entities.Product.ProductGallery> ProductGalleries { get; set; }

        #endregion
    }

    public class FilterProductViewModel : Paging.BasePaging<ProductViewModel>
    {
        public int? Inventory { get; set; }

        public int? StartPrice { get; set; }

        public int? EndPrice { get; set; }
        public string? SubCategoryTitle { get; set; }
        public int? Price { get; set; }

        public int? OffPrice { get; set; } = 0;

        public string? ProductName { get; set;  }

    }
}


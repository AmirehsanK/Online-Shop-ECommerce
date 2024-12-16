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
    public class AddProductViewModel
    {
        [MaxLength(200)]
        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string? Review { get; set; }

        public string? ExpertReview { get; set; }

        public IFormFile Image { get; set; }

        public int Price { get; set; } = 0;

        public int SubCategoryId {  get; set; }

        public List<CategoryListViewModel> SubCategories { get; set; }
    }
}

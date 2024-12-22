using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModel.Discount
{
    public class DiscountProductViewModel
    {
        public int DiscountId {  get; set; }

        // Related Products
        public List<int> SelectedProductIds { get; set; } = new List<int>();
        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();

        // Related Categories
        //public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}

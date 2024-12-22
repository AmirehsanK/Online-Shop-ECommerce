using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.ViewModel.Discount
{
    public class DiscountUserViewModel
    {
        public int DiscountId { get; set; }
        // Related Users
        public List<int> SelectedUserIds { get; set; } = new List<int>();
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();

    }
}



using Domain.Shared;

namespace Domain.ViewModel.Product.ProductSpecification
{


    public class FilterProductSpecification: Paging.BasePaging<ShowProductSpecificationViewModel>
    {
        public string? Title { get; set; }

        public string? Value { get; set; }

    }
    public class ShowProductSpecificationViewModel
    {
        public int ProductId { get; set; }
        public int  Id { get; set; }
        public string Title { get; set; }

        public string Values { get; set; }
    }
}

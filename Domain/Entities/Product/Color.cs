
using Domain.Entities.Common;

namespace Domain.Entities.Product
{
    public class Color : BaseEntity
    {
        public string ColorCode { get; set; }

        public string Title { get; set; }

        #region Realtion

        public ICollection<ProductColor> ProductColors { get; set; }



        #endregion
    }
}

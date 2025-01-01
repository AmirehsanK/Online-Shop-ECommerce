

using Application.Services.Interfaces;
using Domain.Entities.Orders;
using Domain.Interface;
using Domain.ViewModel.Order;

namespace Application.Services.Impelementation
{
    public class OrderService
        (IOrderRepository orderRepository,
            IProductColorRepository colorRepository) : IOrderService
    {
        public async Task AddProductToOrder(int productId, int userId,int? productColorId, int count = 1)
        {
            var OpenOrder = await orderRepository.GetUserLatestOpenOrder(userId);
            var exsistOrderDetial = await orderRepository.GetExistOrderDetail(productId, productColorId.Value);
            if (exsistOrderDetial==null)
            {
                var orderdetail = new OrderDetail()
                {
                    ProductId = productId,
                    Count = count,
                    OrderId = OpenOrder.Id,
                    IsDeleted = false,
                    CreateDate = DateTime.Now,
        
                    ProductColorId = productColorId

                };
                
                if (productColorId.HasValue)
                {
                    var colorprice = await colorRepository.GetProductColorWithid(productColorId.Value);
                    var color = colorprice.Price;
                    orderdetail.ColorPrice = color;
                }
                
                await orderRepository.AddOrderDetail(orderdetail);
                await orderRepository.Save();
            }
            else
            {
                exsistOrderDetial.Count=exsistOrderDetial.Count+1;
                orderRepository.UpdateOrderDetail(exsistOrderDetial);
                await orderRepository.Save();
            }
      
    

        }

        public async Task MinuesColorCount(int productColorId)
        {
            var product = await colorRepository.GetProductColorWithid(productColorId);
            product.Count = product.Count - 1;
            colorRepository.UpdateProductColor(product);
            await colorRepository.SaveChangeAsync();
        }

        public async Task<List<BasketDetailViewModel>> GetBasketDetail(int userId)
        {
            var details = await orderRepository.GetUserBasketDetail(userId);
            var detailss = new List<BasketDetailViewModel>();

            foreach (var item in details.OrderDetails)
            {
                var colorcode = await colorRepository.GetProductColorWithid(item.ProductColorId.Value);
                var code = colorcode.Color.ColorCode;
                var name = colorcode.Color.Title;

                var newdetail = new BasketDetailViewModel
                {
                    ColorCode = code,
                    Title = item.Product.ProductName,
                    ImageName = item.Product.ImageName,
                    FinallPrice = (item.ColorPrice + item.Product.Price) * item.Count,
                    OrderDetailId = item.Id,
                    ColorName = name,
                    ProductCount = item.Count

                };

                detailss.Add(newdetail);


            }

            return detailss;

        }
    }
}

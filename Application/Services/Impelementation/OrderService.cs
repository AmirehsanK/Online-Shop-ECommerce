using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Orders;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Order;
using Domain.ViewModel.User;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Impelementation
{
    public class OrderService
        (IOrderRepository orderRepository,
            IProductColorRepository colorRepository,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository) : IOrderService
    {



    
    #region Add Product to Order

    public async Task<AddToBasketResult> AddProductToOrder(int productId, int userId, int? productColorId,
        int count = 1)
    {
        var openOrder = await orderRepository.GetUserLatestOpenOrder(userId);
        var existOrderDetail = await orderRepository.GetExistOrderDetail(productId, productColorId,openOrder.Id);
        if (existOrderDetail == null)
        {
            var orderDetail = new OrderDetail()
            {
                ProductId = productId,
                Count = count,
                OrderId = openOrder.Id,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                ProductColorId = productColorId
            };

            if (productColorId.HasValue)
            {
                var colorPrice = await colorRepository.GetProductColorWithid(productColorId.Value);
                var color = colorPrice.Price;
                orderDetail.ColorPrice = color;
            }

            await orderRepository.AddOrderDetail(orderDetail);
            await orderRepository.Save();
        }
        else
        {
            existOrderDetail.Count = existOrderDetail.Count + 1;
            orderRepository.UpdateOrderDetail(existOrderDetail);
            await orderRepository.Save();
        }

        return AddToBasketResult.Success;

    }

    
    #endregion

    #region Update Product Color Count

  

    public async Task MinuesColorCount(int productColorId)
    {
        var product = await colorRepository.GetProductColorWithid(productColorId);
        product.Count = product.Count - 1;
        colorRepository.UpdateProductColor(product);
        await colorRepository.SaveChangeAsync();
    }
    #endregion

        public async Task<List<BasketDetailViewModel?>> GetBasketDetail(int userId)
        {
            var details = await orderRepository.GetUserBasketDetail(userId);
            if (details == null)
            {
                return null;
            }
            var detailss = new List<BasketDetailViewModel>();
            string color=null;
            string colorcodes=null;
            foreach (var item in details.OrderDetails)
            {

                var colorcode = await colorRepository.GetProductColorWithid(item.ProductColorId);
                if (colorcode!=null)
                {
                    color = colorcode.Color.Title;
                    colorcodes = colorcode.Color.ColorCode;
                }
          
                var newdetail = new BasketDetailViewModel
                {
                    ColorCode = colorcodes,
                    Title = item.Product.ProductName,
                    ImageName = item.Product.ImageName,
                    FinallPrice = (item.ColorPrice + item.Product.Price) * item.Count,
                    OrderDetailId = item.Id,
                    ColorName = color,
                    ProductCount = item.Count,
                    ProductId = item.ProductId
                };
                detailss.Add(newdetail);
            }
            return detailss;

        }
    

    #region User Address for Order
    public async Task<GetUserAddressForOrderViewModel> GetUserAddressForOrder(int userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        var userAddress = new GetUserAddressForOrderViewModel()
        {
            Address = user.Address!,
            FullName = user.FirstName + " " + user.LastName
        };
        return userAddress;
    }

    public async Task AddUserAddressForOrder(GetUserAddressForOrderViewModel model, int userid)
    {
        var user = await userRepository.GetUserByIdAsync(userid);
        user.Address = model.Address;
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }
    #endregion

        public async Task CloseOrder(int userId, int transId)
        {
            var order = await orderRepository.GetUserLatestOpenOrder(userId);
            if (order.OrderDetails.Count != 0)
            {
                order.IsFinally = true;
                order.PaymentDate = DateTime.Now;
                orderRepository.UpdateOrder(order);
                await orderRepository.Save();
            }


            var trans = await transactionRepository.GetTransactionById(transId);
            trans.IsPay = true;
            transactionRepository.UpdateTransaction(trans);
            await transactionRepository.Save();

        }

        public async Task ChangeTransactionStatus(int transid)
        {
            var transaction = await transactionRepository.GetTransactionById(transid);
            transaction.IsPay = false;
            transactionRepository.UpdateTransaction(transaction);
            await transactionRepository.Save();
        }
    }
}

    

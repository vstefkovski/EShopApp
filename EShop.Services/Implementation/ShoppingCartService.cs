using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<EmailMessage> mailRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<Order> orderRepository, IRepository<ProductInOrder> productInOrderRepository, IUserRepository userRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _userRepository = userRepository;
            _mailRepository = mailRepository;
        }

        public ShoppingCartDTO getShoppingCartInfo(string userId)
        {

            var loggedInUser = this._userRepository.Get(userId);

            var userCard = loggedInUser.UserCart;

            var allProducts = userCard.ProductInShoppingCarts.ToList();

            var allProductPrices = allProducts.Select(z => new
            {
                ProductPrice = z.Product.ProductPrice,
                Quantity = z.Quantity
            }).ToList();

            double totalPrice = 0.0;

            foreach (var item in allProductPrices)
            {
                totalPrice += item.Quantity * item.ProductPrice;
            }

            var result = new ShoppingCartDTO
            {
                ProductInShoppingCarts = allProducts,
                TotalPrice = totalPrice
            };

            return result;
        }

        public bool deleteProductFromShoppingCart(string userId, Guid productId)
        {
            if (!string.IsNullOrEmpty(userId) && productId != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.ProductInShoppingCarts.Where(z => z.Id.Equals(productId)).FirstOrDefault();

                userShoppingCart.ProductInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userCard = loggedInUser.UserCart;

                EmailMessage message = new EmailMessage();
                message.MailTo = loggedInUser.Email;
                message.Subject = "Sucessfully created order";
                message.Status = false;


                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepository.Insert(order);

                List<ProductInOrder> productInOrders = new List<ProductInOrder>();

                var result = userCard.ProductInShoppingCarts.Select(z => new ProductInOrder
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.Product.Id,
                    SelectedProduct = z.Product,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();
                var totalPrice = 0.0;
                sb.Append("Your order is completed. The order contains: ");
                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];
                    totalPrice += item.Quantity * item.SelectedProduct.ProductPrice;
                    sb.AppendLine(i.ToString() + "." + item.SelectedProduct.ProductName + " with the price of: " + item.SelectedProduct.ProductPrice + " and quantity of: " + item.Quantity);

                }
                sb.AppendLine("Total price: " + totalPrice.ToString());

                message.Content = sb.ToString();

                productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepository.Insert(item);
                }



                loggedInUser.UserCart.ProductInShoppingCarts.Clear();

                this._mailRepository.Insert(message);  

                this._userRepository.Update(loggedInUser);

                return true;

            }
            return false;
        }
    }
}

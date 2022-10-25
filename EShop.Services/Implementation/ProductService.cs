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
    public class ProductService : IProductService
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IRepository<Product> productRepository, IUserRepository userRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
        }


        public bool AddToShoppingCart(AddToShoppingCartDTO item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCart = user.UserCart;

            var product = this.GetDetailsForProduct(item.SelectedProductId);

            if (product != null && userShoppingCart != null)
            {
                ProductInShoppingCart itemToAdd = new ProductInShoppingCart
                {
                    ProductId = product.Id,
                    ShoppingCartId = userShoppingCart.Id,
                    Product = product,
                    ShoppingCart = userShoppingCart,
                    Quantity = item.Quantity
                };
                this._productInShoppingCartRepository.Insert(itemToAdd);
                return true;

            }
            return false;
        }

        public void CreateNewProduct(Product p)
        {
            this._productRepository.Insert(p);
        }

        public void DeleteProduct(Guid id)
        {
            var product = this.GetDetailsForProduct(id);
            this._productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            return this._productRepository.GetAll().ToList();

        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return _productRepository.Get(id);
        }

        public AddToShoppingCartDTO GetShoppingCartInfo(Guid? id)
        {
            var product = this.GetDetailsForProduct(id);

            AddToShoppingCartDTO model = new AddToShoppingCartDTO
            {
                SelectedProduct = product,
                SelectedProductId = product.Id,
                Quantity = 1
            };
            return model;
        }

        public void UpdeteExistingProduct(Product p)
        {
            this._productRepository.Update(p);
        }
    }
}

using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDTO getShoppingCartInfo(string userId);
        bool deleteProductFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId); 

    }
}

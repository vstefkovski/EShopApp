using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Web.Models.Domain
{
    public class ProductInShoppingCart
    {
        public Guid ProductId { get; set; }
        public Guid ShoppingCartId { get; set; }
        public Product Product { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }

    }
}

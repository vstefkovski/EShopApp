using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public Guid Id { get; set; }

        public string OwnerId { get; set; }

        public EShopApplicationUser Owner { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Web.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]

        public string ProductName { get; set; }
        [Required]

        public string ProductImage { get; set; }
        [Required]

        public string ProductDescription { get; set; }
        [Required]
        public int ProductPrice { get; set; }
        [Required]

        public int Rating { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual ICollection<ProductInOrder> Orders { get; set; }


    }
}

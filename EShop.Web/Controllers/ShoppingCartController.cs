using EShop.Web.Data;
using EShop.Web.Models.Domain;
using EShop.Web.Models.DTO;
using EShop.Web.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EShopApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context, UserManager<EShopApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users
                .Where(z => z.Id.Equals(userId))
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.ProductInShoppingCarts)
                .Include("UserCart.ProductInShoppingCarts.Product")
                .FirstOrDefaultAsync();

            var userShoppingCart = loggedInUser.UserCart;

            var productsPrice = userShoppingCart.ProductInShoppingCarts.Select(z => new
            {
                ProductPrice = z.Product.ProductPrice,
                Quantity = z.Quantity
            }).ToList();

            double totalPrice = 0;

            foreach (var item in productsPrice)
            {
                totalPrice += item.ProductPrice * item.Quantity;
            }

            ShoppingCartDTO shoppingCartDTOitem = new ShoppingCartDTO
            {
                ProductInShoppingCarts = userShoppingCart.ProductInShoppingCarts.ToList(),
                TotalPrice = totalPrice
            };

            return View(shoppingCartDTOitem);
        }

        public async Task<IActionResult> DeleteProductFromShoppingCart(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users
                .Where(z => z.Id.Equals(userId))
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.ProductInShoppingCarts)
                .Include("UserCart.ProductInShoppingCarts.Product")
                .FirstOrDefaultAsync();

            var userShoppingCart = loggedInUser.UserCart;

            var productToRemove = userShoppingCart.ProductInShoppingCarts.Where(z => z.ProductId == productId).FirstOrDefault();

            userShoppingCart.ProductInShoppingCarts.Remove(productToRemove);

            _context.Update(userShoppingCart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ShoppingCart");
        }

        public async Task<IActionResult> OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users
                .Where(z => z.Id.Equals(userId))
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.ProductInShoppingCarts)
                .Include("UserCart.ProductInShoppingCarts.Product")
                .FirstOrDefaultAsync();

            var userShoppingCart = loggedInUser.UserCart;

            Order orderItem = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                User = loggedInUser
            };

            _context.Add(orderItem);
            await _context.SaveChangesAsync();

            List<ProductInOrder> productInOrders = new List<ProductInOrder>();

            productInOrders = userShoppingCart.ProductInShoppingCarts
                .Select(z => new ProductInOrder
                {
                    OrderId = orderItem.Id,
                    ProductId = z.ProductId,
                    SelectedProduct = z.Product,
                    UserOrder = orderItem
                }).ToList();

            foreach (var item in productInOrders)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
            }

            loggedInUser.UserCart.ProductInShoppingCarts.Clear();

            _context.Update(loggedInUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}

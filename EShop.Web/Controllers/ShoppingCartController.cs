using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Domain.Identity;
using EShop.Repository;
using EShop.Services.Interface;
using EShop.Web.Data;
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

        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }

        public IActionResult DeleteProductFromShoppingCart(Guid id)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this._shoppingCartService.deleteProductFromShoppingCart(userId, id);

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.orderNow(userId);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");

            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
        }

    }
}

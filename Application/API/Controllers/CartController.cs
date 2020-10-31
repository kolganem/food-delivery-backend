using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application;
using Domain.Helpers;
using Domain.Model;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("cartitems")]
        public IEnumerable<CartItem> GetCartItems()
        {
            var items = _cartService.GetItems();
            return items;
        }
        
        [HttpPost]
        public void AddToCart(Guid productId)
        {
            if (_cartService.ProductExist(productId))
            {
                Cart cart = GetCart();
                _cartService.SetCart(cart);
                _cartService.AddToCart(productId);
                SaveCart(cart);
            }            
            
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart) 
        {
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }


    }
}
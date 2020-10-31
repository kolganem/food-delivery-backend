using Domain.Application;
using Domain.Helpers;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class CartService : ICartService
    {       
        private FoodDeliveryDbContext _context;
        public Cart shopingCart;

        public CartService(FoodDeliveryDbContext context)
        {
            _context = context;

        }
        public void AddToCart(Guid productId)
        {
            var product = _context.Products.Find(productId);
            var cartItem = _context.CartItems.SingleOrDefault(s => s.ProductId == productId); 
            
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = shopingCart.Id,
                    Product = product,
                    ProductId = productId,
                    Quantity = 1,
                
                };
                _context.CartItems.Add(cartItem);                
            }
            else
            {
                cartItem.Quantity++;
            }
            _context.SaveChanges();

        }

        public List<CartItem> GetItems()
        {
            var cartItems = _context.CartItems.ToList(); // need add some condition with cartId
            return cartItems;
        }

        public void RemoveFromCart(Product product)
        {
            var cartItem = _context.CartItems.SingleOrDefault(s => s.ProductId == product.Id);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
            }
            _context.SaveChanges();
        }
        public bool ProductExist(Guid productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
                return true;
            else return false;
        }
        public void SetCart(Cart cart)
        {
            shopingCart = cart;
        }

       
    }

       
}

using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Application
{
    public interface ICartService
    {
        void AddToCart(Guid productId);
        void RemoveFromCart(Product product);
        bool ProductExist(Guid productID);
        void SetCart(Cart cart);

        List<CartItem> GetItems();

    }
}

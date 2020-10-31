using Domain.Model;
using System.Collections.Generic;

namespace Domain.Application
{
    public interface IMainService
    {
        List<Product> GetProducts();
        List<ProductCategory> GetProductCategories();
        List<RestaurantCategory> GetRestaurantCategories();
        List<Restaurant> GetRestaurants();
    }
}

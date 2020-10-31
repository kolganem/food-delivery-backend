using System.Collections.Generic;
using Domain.Application;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MainController : Controller
    {

        public MainController(IMainService mainService)
        {
            _mainService = mainService;
        }

        [HttpGet("restaurants")]
        public IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = _mainService.GetRestaurants();

            return restaurants;
        }

        [HttpGet("restaurant-categories")]
        public IEnumerable<RestaurantCategory> GetRestaurantCategories()
        {
            var categories = _mainService.GetRestaurantCategories();

            return categories;
        }

        [HttpGet("products")]
        public IEnumerable<Product> GetProducts()
        {
            var products = _mainService.GetProducts();

            return products;
        }

        [HttpGet("product-categories")]
        public IEnumerable<ProductCategory> GetProductCategories()
        {
            var categories = _mainService.GetProductCategories();

            return categories;
        }
        private readonly IMainService _mainService;
    }
}
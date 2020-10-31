using Domain.Application;
using Domain.Model;
using Infrastructure.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Service
{
    public class MainService : IMainService
    {
        public MainService(IRepository<ProductCategory> productCategoriesRepository,
                            IRepository<Product> productRepository,
                            IRepository<Restaurant> restaurantsRepository,
                            IRepository<RestaurantCategory> restaurantCategoryRepository)
        {
            _productCategoriesRepository = productCategoriesRepository;
            _restaurantsRepository = restaurantsRepository;
            _restaurantCategoryRepository = restaurantCategoryRepository;
            _productRepository = productRepository;
        }

        public List<ProductCategory> GetProductCategories()
        {
            var categories = _productCategoriesRepository.GetAll().ToList();
            
            return categories;
        }
        public List<RestaurantCategory> GetRestaurantCategories()
        {
            var categories = _restaurantCategoryRepository.GetAll().ToList();

            return categories;
        }

        public List<Restaurant> GetRestaurants()
        {
            var restaurants = _restaurantsRepository.GetAll().ToList();
            return restaurants;
        }

        public List<Product> GetProducts()
        {
            var products = _productRepository.GetAll().ToList();
            return products;
        }


        private readonly IRepository<RestaurantCategory> _restaurantCategoryRepository;
        private readonly IRepository<ProductCategory> _productCategoriesRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Restaurant> _restaurantsRepository;
    }
}

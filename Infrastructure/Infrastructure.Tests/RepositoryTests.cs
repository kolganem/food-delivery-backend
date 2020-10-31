using System;
using System.Linq;
using Domain.Model;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Infrastructure.Tests
{
    public class RepositoryTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<RepositoryTests>()
                .Build();

            var options = new DbContextOptionsBuilder<FoodDeliveryDbContext>()
                .UseSqlServer(config.GetConnectionString("MockFoodDeliveryDBConnectionString"))
                .Options;

            _context = new FoodDeliveryDbContext(options);
            var mockDataInitializer = new MockDataInitializer();
            mockDataInitializer.Seed(_context);
        }

        [Test]
        [Order(1)]
        public void CanReturnAllProducts()
        {
            var repository = new GenericRepository<Product>(_context);
            var products = repository.GetAll().ToArray();

            Assert.IsNotNull(products);
            Assert.AreEqual(20, products.Count());
            Assert.IsTrue(products.All(x=>x.Restaurant==null));
        }
        [Test]
        [Order(2)]
        public void CanReturnAllProductsIncludeProperties()
        {
            var repository = new GenericRepository<Product>(_context);

            var products = repository.Get(includeProperties:"Restaurant,Category").ToArray();

            Assert.IsNotNull(products);
            Assert.AreEqual(20, products.Count());
            Assert.IsTrue(products.All(x => x.Restaurant != null));
            Assert.IsTrue(products.All(x => x.Category != null));
        }

        [Test]
        [Order(3)]
        [TestCaseSource("_productsGuid")]
        public void CanReturnById(Guid id)
        {
            var repository = new GenericRepository<Product>(_context);
            var product = repository.GetById(id);

            Assert.IsNotNull(product);
            Assert.IsInstanceOf<Product>(product);
            Assert.AreEqual(id, product.Id);
        }
        
        [Test]
        [Order(4)]
        public void CanReturnByExpression()
        {
            var repository = new GenericRepository<Product>(_context);

            var products = repository.Get(x => x.Restaurant.Name.Contains("a"),
                                                    queryable => queryable.OrderBy(x => x.Name), 
                                                    includeProperties:"Restaurant").ToList();

            Assert.IsNotNull(products);
            Assert.IsTrue(products.All(p=>p.Restaurant.Name.Contains("a")));
            Assert.That(products, Is.Ordered.By("Name"));
        }
        
        [Test]
        [Order(5)]
        public void CanInsert()
        {
            var repository = new GenericRepository<RestaurantCategory>(_context);
            var restaurantCategory = new RestaurantCategory() {Name = "My new restaurant"};

            var insertedRestaurantCategoryResult = repository.Insert(restaurantCategory);
            Assert.AreEqual(1, insertedRestaurantCategoryResult);

            var category = repository.Get(x => x.Name == "My new restaurant").FirstOrDefault();

            Assert.IsNotNull(category);
            Assert.IsNotNull(category.Id);
            Assert.AreEqual(restaurantCategory.Name,category.Name);
        }
        
        [Test]
        [Order(6)]
        [TestCaseSource("_productsGuid")]
        public void CanUpdate(Guid id)
        {
            var repository = new GenericRepository<Product>(_context);
            var actualProduct = repository.GetById(id);

            actualProduct.Name = "Hello, world!";

            var updatedProductResult = repository.Update(actualProduct);
            Assert.AreEqual(1, updatedProductResult);

            var updatedProduct = repository.GetById(id);
            Assert.AreEqual("Hello, world!", updatedProduct.Name);
        }
        
        [Test]
        [Order(7)]
        [TestCaseSource("_productsGuid")]
        public void CanDelete(Guid id)
        {
            var repository = new GenericRepository<Product>(_context);
            var productDeleteCandidate = repository.GetById(id);

            var deletedProductResult = repository.Delete(productDeleteCandidate);
            Assert.AreEqual(1, deletedProductResult);

            var deletedProduct = repository.GetById(id);
            Assert.AreEqual(default(Product), deletedProduct);
        }
        
        private FoodDeliveryDbContext _context;

        private static readonly Guid[] _productsGuid = new[]
        {
            new Guid("FF2B25E6-6831-9C70-CD51-06CC10BAC92B"),
            new Guid("6E1A6C4C-8D73-FA78-371F-43D5093CB5DE")
        };
    }
}
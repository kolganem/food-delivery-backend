using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? RestaurantId { get; set; }
        public Guid? CategoryId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual ProductCategory Category { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public byte[] Photo { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public Product()
        {
            OrderDetails = new Collection<OrderDetail>();
            CartItems = new Collection<CartItem>();
        }
    }
}

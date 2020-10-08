using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        [Required]
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        [Required]
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CartId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [ForeignKey(nameof(CartId))]
        public virtual Product Product { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Restaurant
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual RestaurantCategory Category { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public byte[] Logo { get; set; }
        public byte[] Cover { get; set; }
    }
}

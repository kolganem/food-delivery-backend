using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    public class RestaurantCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}

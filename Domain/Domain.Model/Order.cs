using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Model.Identity;

namespace Domain.Model
{
    public class Order
    {
        [Key] public Guid Id { get; set; }

        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required] 
        [Range(0, double.MaxValue)]
        public decimal OrderTotal { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(UserId))] 
        public virtual User User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

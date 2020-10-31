using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model.Verification
{
    public class VerificationState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }

        [Required]
        public string VerificationStatus { get; set; }

        [Required] 
        public DateTime CreatedDateTime { get; set; }
    }
}

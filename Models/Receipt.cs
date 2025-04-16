using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Receipt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int TotalPrice { get; set; }
        [Required]
        public string PaymentType { get; set; }

        [Required]
        public bool IsPaid { get; set; } = false;
        public ICollection<ReceiptDetail>? ReceiptDetails { get; set; }

        public int? ComboFoodId { get; set; }
        [ForeignKey(nameof(ComboFoodId))]
        public virtual Food? GetFood { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? GetUser { get; set; }
    }
}

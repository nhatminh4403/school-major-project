using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace school_major_project.Models
{
    public class Promotion
    {
        [Key]
        [Required]
        [DisplayName("Id mã giảm giá")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Mã giảm giá")]
        [Required]
        public string Code { get; set; }
        [DisplayName("Mô tả mã giảm giá")]
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Ngày bắt đầu")]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayName("Ngày kết thúc")]
        public DateTime EndDate { get; set; }
        [DisplayName("Mức giảm giá")]
        [Required]
        public double DiscountRate { get; set; }
        [Required]
        [DisplayName("Điểm để đổi")]
        public int RedemptionPoint { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

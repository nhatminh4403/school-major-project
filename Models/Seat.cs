using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace school_major_project.Models
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã ghế")]
        public int SeatId { get; set; }
        [Required]
        [DisplayName("Số ghế")]
        public string? SeatNumber { get; set; }
        [Required]
        [DisplayName("Giá")]
        public decimal SeatPrice { get; set; }
        [DisplayName("Tình trạng ghế")]
        [Required]
        public bool Status { get; set; }

        [Required]
        public string? SeatImage { get; set; }

        [Required]
        public int RoomId { get; set; }
        [Required]
        public int SeatTypeId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; }

        [ForeignKey(nameof(SeatTypeId))]
        public virtual SeatType SeatType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class ReceiptDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string filmName { get; set; }
        [Required] 
        public string cinemaName { get; set; }
        [Required]
        public string roomName { get; set; }

        [Required]
        public string cinemaAddress { get; set; }
        [Required]
        public DateTime? startTime { get; set; }
        [Required]
        public int pricePerSeat { get; set; }

        [Required]
        public int ReceiptId { get; set; }
        [Required]
        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ReceiptId))]
        public virtual Receipt Receipt { get; set; }

        [ForeignKey(nameof(ScheduleId))]
        public virtual Schedule Schedule { get; set; }

        [Required]
        public int SeatId { get; set; }
        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }
    }
}

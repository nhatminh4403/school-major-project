using Microsoft.EntityFrameworkCore;
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
        public int SeatId { get; set; }
        [Required]
        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ReceiptId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]  // Add this
        public virtual Receipt Receipt { get; set; }

        [ForeignKey(nameof(ScheduleId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]  // Add this
        public virtual Schedule Schedule { get; set; }

        [ForeignKey(nameof(SeatId))]  // Changed to use nameof for consistency
        [DeleteBehavior(DeleteBehavior.NoAction)]  // Add this
        public virtual Seat Seat { get; set; }
    }
}

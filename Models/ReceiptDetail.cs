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
        public string FilmName { get; set; }
        [Required] 
        public string CinemaName { get; set; }
        [Required]
        public string RoomName { get; set; }

        [Required]
        public string CinemaAddress { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public int PricePerSeat { get; set; }

        [Required]
        public int ReceiptId { get; set; }
        [Required]
        public int SeatId { get; set; }
        [Required]
        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ReceiptId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]  // Add this
        public virtual Receipt? Receipt { get; set; }

        [ForeignKey(nameof(ScheduleId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]  // Add this
        public virtual Schedule? Schedule { get; set; }

        [ForeignKey(nameof(SeatId))]  // Changed to use nameof for consistency
        [DeleteBehavior(DeleteBehavior.NoAction)]  // Add this
        public virtual Seat? Seat { get; set; }
    }
}

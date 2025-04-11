using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Room
    {
        [Key]
        [Required]
        [DisplayName("Mã phòng")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Tên phòng")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Mô tả phòng")]
        public string Description { get; set; }

        [Required]
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public virtual Cinema? Cinema { get; set; }
        public virtual ICollection<Seat>? Seats { get; set; }
        public virtual ICollection<Schedule>? Schedules { get; set; }

    }
}

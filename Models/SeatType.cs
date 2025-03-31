using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class SeatType
    {
        [Key]
        [Required]
        [DisplayName("Mã loại ghế")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Mô tả loại ghế")]
        public string? TypeDescription { get; set; }

        [Required]
        [DisplayName("Giá loại ghế")]
        public long? Price { get; set; }
        [Required]
        [DisplayName("Điểm thưởng")]
        public int? PointGiving { get; set; }
        [DisplayName("Hình ảnh mô tả")]
        public string? ImageDescription { get; set; } 

        public ICollection<Seat>? Seats { get; set; }
    }
}

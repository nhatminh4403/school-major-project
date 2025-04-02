using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace school_major_project.Models
{
    public class Food
    {
        [Key]
        [Required]
        [DisplayName("Mã loại phim")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Tên combo")]
        public string? ComboName { get; set; }
        [Required]
        [DisplayName("Giá combo")]
        public long Price { get; set; }
        [Required]
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Hình ảnh")]
        public string? Poster
        {
            get; set;
        }

        public ICollection<Receipt>? Receipt { get; set; }
    }
}

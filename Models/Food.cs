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
        private string? comboName { get; set; }
        [Required]
        [DisplayName("Giá combo")]
        private long price { get; set; }
        [Required]
        [DisplayName("Mô tả")]
        private string description { get; set; }
        [Required]
        [DisplayName("Hình ảnh")]
        private string poster
        {
            get; set;
        }

        public ICollection<Receipt>? Receipt { get; set; }
    }
}

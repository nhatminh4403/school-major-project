using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Blog
    {
        [Required]
        [Key]
        [DisplayName("Mã blog")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Tên blog")]
        public string BlogTitle { get; set; }

        [Required]
        [DisplayName("Nội dung blog")]
        public string BlogContent { get; set; }

        [Required]
        [DisplayName("Ngày tạo blog")]
        public DateTime BlogCreatedDate { get; set; }

        [Required]
        [DisplayName("Ảnh mô tả")]
        public string BlogPoster { get; set; }

        public ICollection<Comment>? comments { get; set; }
    }
}

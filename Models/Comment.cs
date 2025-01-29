using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace school_major_project.Models
{
    public class Comment
    {
        [Key]
        [Required]
        [DisplayName("Id mã giảm giá")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Id mã giảm giá")]
        public string content { get; set; }
        [Required]
        public DateTime dateComment { get; set; }

        [Required]
        public int BlogId { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }  
    }
}

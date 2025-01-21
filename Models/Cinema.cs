using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace school_major_project.Models
{
    public class Cinema
    {
        [DisplayName("Mã rạp")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [DisplayName("Địa chỉ rạp")]
        [Required]
        public string? Location { get; set; }

        [Required]
        [DisplayName("Địa chỉ trên Map")]
        private string? Map { get; set; }

    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Cinema
    {
        [DisplayName("Mã rạp")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Địa chỉ rạp")]
        [Required]
        public string Location { get; set; }

        [Required]
        [DisplayName("Địa chỉ trên Map")]
        public string Map { get; set; }

        public virtual ICollection<Room>? Rooms { get; set; }
    }
}

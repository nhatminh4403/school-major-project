using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Country
    {
        [Key]
        [Required]
        [DisplayName("Mã quốc gia")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Tên quốc gia")]
        public string? Name { get; set; }

        public ICollection<Film>? films { get; set; }
    }
}

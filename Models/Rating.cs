using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public string RatingContent { get; set; }
        [Required]
        public DateTime RatingDate { get; set; }
        [Required]
        public int Star {  get; set; }

        [Required]
        public int FilmId { get; set; }
        [ForeignKey(nameof(FilmId))]
        public virtual Film Film { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}

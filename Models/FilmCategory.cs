using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class FilmCategory
    {
        public int FilmId { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(FilmId))]
        public virtual Film Film { get; set; }
    }
}

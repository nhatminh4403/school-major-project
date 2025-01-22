using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class PromotionUser
    {
        public string UserId { get; set; }
        public int PromotionId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("PromotionId")]
        public virtual Promotion Promotion { get; set; }
    }
}

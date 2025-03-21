using System.ComponentModel.DataAnnotations;

namespace school_major_project.ViewModel
{
    public class ContactViewModel
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}

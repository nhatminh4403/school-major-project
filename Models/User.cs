using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace school_major_project.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }

        [DisplayName("Tuổi")]
        public int age { get; set; }
        [DisplayName("Ngày sinh")]
        public string? birthday { get; set; }

        [DisplayName("Điểm tích lũy")]
        public long pointSaving { get; set; } = 0;


        public bool isStudent { get; set; } = false;
    }
}

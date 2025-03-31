using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace school_major_project.Models
{
    public class User : IdentityUser
    {
        [StringLength(50)]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [DisplayName("Tuổi")]
        public int age { get; set; }
        [DisplayName("Ngày sinh")]
        public string? Birthday { get; set; }

        [DisplayName("Điểm tích lũy")]
        public long PointSaving { get; set; } = 0;

        public bool IsStudent { get; set; } = false;

        public int ScanningCount { get; set; } = 0;
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Receipt>? Receipts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Promotion> Promotions { get; set; }
    }
}

using school_major_project.Models;

namespace school_major_project.Areas.Admin.Data
{
    public class AccountVM
    {
        public User User { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

    }
}

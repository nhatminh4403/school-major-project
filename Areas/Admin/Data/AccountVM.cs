using school_major_project.Models;

namespace school_major_project.Areas.Admin.Data
{
    public class AccountVM
    {
        public IEnumerable<User> Users { get; set; }
        public User User { get; set; }
    }
}

using school_major_project.Models;

namespace school_major_project.ViewModel
{
    public class HistoryVM
    {
        public User User { get; set; }
        public IEnumerable<Receipt> Receipts { get; set; }

    }
}

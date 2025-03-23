using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace school_major_project.ViewModel
{
    public class UserVM
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int age { get; set; }
        public string birthday { get; set; }
        public long pointSaving { get; set; } = 0;
        public bool isStudent { get; set; } = false;
    }
}

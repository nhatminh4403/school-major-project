using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace school_major_project.ViewModel
{
    public class UserVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int age { get; set; }
        public string birthday { get; set; }
        public bool isStudent { get; set; } = false;
    }
}

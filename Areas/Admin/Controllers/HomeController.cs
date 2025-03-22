using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area(Role.Role_Admin)]
    [Authorize(Roles = Role.Role_Admin)]

    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }
    }
}

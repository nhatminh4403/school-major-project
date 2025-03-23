using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using school_major_project.Models;

namespace school_major_project.Controllers
{
    [Authorize(Roles = Role.Role_Customer)]
    [Route("tai-khoan")]
    public class UserController : Controller
    {
        // GET: UserController
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("chi-tiet-tai-khoan")]
        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [Route("chinh-sua-tai-khoan")]
        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [Route("xoa-tai-khoan")]
        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [Route("lich-su-dat-ve")]
        public IActionResult History()
        {
            return View();
        }

    }
}

using System.Web.Mvc;
using Proiect_TW.Web.Models;
using eUseControl.Data.Services;

namespace Proiect_TW.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _service = new UserService();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (_service.IsUserRegistered(model.Username))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login.";
            return View(model);
        }
    }
} 
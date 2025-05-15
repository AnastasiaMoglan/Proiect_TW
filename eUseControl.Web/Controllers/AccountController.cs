using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            // TODO: Add authentication logic here
            // If successful:
            // return RedirectToAction("Index", "Home");
            // If failed:
            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string name, string username, string email, string password, string confirmPassword, string phone)
        {
            // TODO: Add registration logic here
            // If successful:
            // return RedirectToAction("Login");
            // If failed:
            ViewBag.Error = "Registration failed.";
            return View();
        }
    }
}
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models;
using System.Web.Security;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        // Constructor corect cu injecție dependență
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userService.ValidateUser(model.Email, model.Password);
            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;

                FormsAuthentication.SetAuthCookie(user.Username, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View(model);
            }

            bool exists = _userService.UserExists(model.Username, model.Email);
            if (exists)
            {
                ModelState.AddModelError("", "Username or Email already exists.");
                return View(model);
            }

            User success = _userService.RegisterUser(model.Email, model.Username, model.Password);
            if (success==null)
            {
                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View(model);
            }

            TempData["Success"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }

        private ActionResult View(RegisterViewModel viewName)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

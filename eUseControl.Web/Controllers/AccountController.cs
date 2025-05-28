using System;
using System.Web.Mvc;
using System.Web.Security;
using eUseControl.BusinessLogic.Helpers;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

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

            var user = _userService.ValidateUser(model.UsernameOrEmail, model.Password);
            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;

                FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username/email or password.");
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

            if (_userService.UserExists(model.Username, model.Email))
            {
                ModelState.AddModelError("", "Username or Email already exists.");
                return View(model);
            }

            var (hash, salt) = PasswordHasher.HashPassword(model.Password);

            var user = new User
            {
                Email = model.Email,
                Username = model.Username,
                PasswordHash = hash,
                Salt = salt,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var registeredUser = _userService.RegisterUser(
                model.Email,
                model.Username,
                model.Password
            );


            TempData["Success"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

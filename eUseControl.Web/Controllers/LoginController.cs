using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private string GetClientIPAddress()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(ip))
                ip = Request.UserHostAddress;

            if (string.IsNullOrEmpty(ip) || ip == "::1" || ip == "127.0.0.1")
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        ip = client.DownloadString("https://api.ipify.org");
                    }
                }
                catch
                {
                    ip = "Unknown";
                }
            }

            return ip;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var authCookie = Request.Cookies["UserAuth"];
            if (authCookie != null)
            {
                try
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        return ticket.UserData == "Admin"
                            ? RedirectToAction("Dashboard", "Admin")
                            : RedirectToAction("Index", "Home");
                    }
                }
                catch
                {
                    // Invalidate corrupted or expired cookie
                    var expiredCookie = new HttpCookie("UserAuth", "")
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };
                    Response.Cookies.Add(expiredCookie);
                }
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = _userService.ValidateUser(model.Email, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                var ticket = new FormsAuthenticationTicket(
                    version: 1,
                    name: user.Email,
                    issueDate: DateTime.Now,
                    expiration: DateTime.Now.AddDays(7),
                    isPersistent: model.RememberMe,
                    userData: user.Role
                );

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var authCookie = new HttpCookie("UserAuth", encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };

                if (model.RememberMe)
                {
                    authCookie.Expires = DateTime.Now.AddDays(7);
                }

                Response.Cookies.Add(authCookie);

                // Store session data
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.Username;
                Session["UserRole"] = user.Role;

                // Redirect based on role or returnUrl
                if (user.Role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");

                var returnUrl = Request.QueryString["returnUrl"];
                return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? Redirect(returnUrl)
                    : RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                System.Diagnostics.Debug.WriteLine("Login Exception: " + ex);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            // Remove custom auth cookie
            if (Request.Cookies["UserAuth"] != null)
            {
                var expiredCookie = new HttpCookie("UserAuth", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                Response.Cookies.Add(expiredCookie);
            }

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }
    }
}

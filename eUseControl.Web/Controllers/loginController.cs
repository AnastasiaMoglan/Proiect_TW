using System;
using System.Web.Mvc;
using eUseControl.Data.Services;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;
using System.Web.Security;
using System.Web;
using System.Net;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        
        public LoginController()
        {
            _userService = new UserService();
            EnsureAdminExists();
        }
        
        private void EnsureAdminExists()
        {
            try
            {
                // Check if admin exists
                if (!_userService.IsEmailRegistered("admin@admin"))
                {
                    // Create admin user with hashed password
                    _userService.RegisterUser("admin@admin", "Administrator", "admin1");
                    
                    // Update admin role (this would require extending UserService, but simplified here)
                    var adminUser = _userService.ValidateUser("admin@admin", "admin1");
                    if (adminUser != null)
                    {
                        // This would typically be handled through a repository update
                        adminUser.Role = "Admin";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ensuring admin exists: " + ex.ToString());
            }
        }
        
        private string GetClientIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }

            // If still empty or localhost, try to get the actual IP
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "127.0.0.1")
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        ipAddress = client.DownloadString("https://api.ipify.org");
                    }
                }
                catch
                {
                    ipAddress = "Unknown";
                }
            }

            return ipAddress;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            // Check if user is already authenticated via cookie
            if (Request.Cookies["UserAuth"] != null)
            {
                var authCookie = Request.Cookies["UserAuth"];
                try
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        // User is already authenticated, redirect to appropriate page
                        if (ticket.UserData == "Admin")
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (ArgumentException)
                {
                    // Invalid cookie, clear it
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
            {
                return View(model);
            }

            try
            {
                // Validate user credentials
                var user = _userService.ValidateUser(model.Email, model.Password);
                
                // Record login attempt
                _userService.RecordLogin(model.Email, GetClientIPAddress(), user != null);
                
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                // Create authentication ticket
                var authTicket = new FormsAuthenticationTicket(
                    1,                              // Version
                    user.Email,                     // User name
                    DateTime.Now,                   // Issue time
                    DateTime.Now.AddDays(7),        // Expiration time (7 days)
                    model.RememberMe,               // Persistent
                    user.Role                       // User data (role)
                );

                // Encrypt the ticket
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                // Create the cookie
                var authCookie = new HttpCookie("UserAuth", encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                
                // Set cookie expiration if "Remember me" is checked
                if (model.RememberMe)
                {
                    authCookie.Expires = DateTime.Now.AddDays(7);
                }

                // Add the cookie to the response
                Response.Cookies.Add(authCookie);

                // Set session variables
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.Username;
                Session["UserRole"] = user.Role;

                // If user is admin, redirect to admin dashboard
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                // Check if there's a return URL for non-admin users
                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Login Error: " + ex.ToString());
                return View(model);
            }
        }
        
        public ActionResult Logout()
        {
            // Sign out from forms authentication
            FormsAuthentication.SignOut();

            // Clear the authentication cookie
            if (Request.Cookies["UserAuth"] != null)
            {
                var authCookie = new HttpCookie("UserAuth", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                Response.Cookies.Add(authCookie);
            }

            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Redirect to home page
            return RedirectToAction("Index", "Home");
        }
        
        // GET: Login/ViewLoginHistory
        public ActionResult ViewLoginHistory()
        {
            // Check if user is admin
            if (Session["UserRole"]?.ToString() != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var loginRecords = _userService.GetLoginHistory();
                ViewBag.LastUpdated = DateTime.Now;
                return View(loginRecords);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while accessing the login history.";
                ViewBag.DetailedError = ex.Message;
                return View(new System.Collections.Generic.List<eUseControl.Domain.Models.LoginRecord>());
            }
        }
    }
}
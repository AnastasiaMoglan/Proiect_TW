using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eUseControl.Domain.Models;
using eUseControl.Web.Data;
using eUseControl.Web.Services;
using System.Web.Security;

namespace eUseControl.Web.Controllers
{
    public class transferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TransferCardService _transferService;

        public transferController()
        {
            _context = new ApplicationDbContext();
            _transferService = new TransferCardService();
        }

        private bool IsUserAuthenticated()
        {
            // Check session first
            if (Session["UserId"] != null)
                return true;

            // Check authentication cookie
            if (Request.Cookies["UserAuth"] != null)
            {
                try
                {
                    var authCookie = Request.Cookies["UserAuth"];
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        // Restore session data from cookie
                        var userEmail = ticket.Name;
                        using (var context = new ApplicationDbContext())
                        {
                            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
                            if (user != null)
                            {
                                Session["UserId"] = user.Id;
                                Session["UserEmail"] = user.Email;
                                Session["UserName"] = user.Name;
                                Session["UserRole"] = user.Role;
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                    // If there's any error with the cookie, treat as not authenticated
                    return false;
                }
            }

            return false;
        }

        private ActionResult RedirectToLogin()
        {
            return new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
                    { "controller", "Login" },
                    { "action", "Index" },
                    { "returnUrl", Request.RawUrl }
                });
        }

        // GET: transfer
        public ActionResult Index()
        {
            // Allow viewing the index page without authentication
            ViewBag.IsAuthenticated = IsUserAuthenticated();
            return View();
        }

        public ActionResult A2A()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult P2P()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult T2C()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult AFM()
        {
            if (!IsUserAuthenticated())
                return RedirectToLogin();
            return View();
        }

        public ActionResult ViewTransferA2A()
        {
            if (Session["UserRole"]?.ToString() != "Admin")
                return RedirectToAction("Index", "Login", new { returnUrl = Request.RawUrl });
            var transfers = _transferService.GetAllTransfers();
            return View(transfers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateA2ATransfer(TransferCard transfer)
        {
            if (!IsUserAuthenticated())
                return Json(new { success = false, error = "Authentication required" });

            // Set names from session BEFORE validation
            if (Session["UserName"] != null)
            {
                transfer.SenderName = Session["UserName"].ToString();
                transfer.BeneficiaryName = Session["UserName"].ToString();
            }
            else
            {
                return Json(new { success = false, error = "User session expired" });
            }

            // Remove validation errors for all possible keys
            foreach (var key in ModelState.Keys.ToList())
            {
                if (key.EndsWith("SenderName") || key.EndsWith("BeneficiaryName"))
                {
                    ModelState.Remove(key);
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    transfer.BankSender = "Maib";
                    transfer.BankBeneficiary = "Maib";
                    transfer.TransferDate = DateTime.Now;

                    _transferService.CreateTransfer(transfer);
                    return Json(new { success = true, message = "Transfer created successfully" });
                }
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, error = string.Join(", ", errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateP2PTransfer(TransferCard transfer)
        {
            if (!IsUserAuthenticated())
                return Json(new { success = false, error = "Authentication required" });

            try
            {
                if (ModelState.IsValid)
                {
                    transfer.BankSender = "Maib";
                    transfer.BankBeneficiary = "Maib";
                    transfer.TransferDate = DateTime.Now;
                    // Uncomment if you add TransferType to the model:
                    // transfer.TransferType = "P2P";

                    _transferService.CreateTransfer(transfer);
                    return Json(new { success = true, message = "Transfer created successfully" });
                }
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, error = string.Join(", ", errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateT2CTransfer(TransferCard transfer)
        {
            if (!IsUserAuthenticated())
                return Json(new { success = false, error = "Authentication required" });

            try
            {
                if (ModelState.IsValid)
                {
                    transfer.BankSender = "Maib";
                    // Setează BankBeneficiary din caseta Bank
                    if (!string.IsNullOrEmpty(Request.Form["Bank"]))
                        transfer.BankBeneficiary = Request.Form["Bank"];
                    transfer.TransferDate = DateTime.Now;
                    _transferService.CreateTransfer(transfer);
                    return Json(new { success = true, message = "Transfer created successfully" });
                }
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, error = string.Join(", ", errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
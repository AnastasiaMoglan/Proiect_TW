using System;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Services;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        // Dependency Injection via constructor
        public ContactController(IContactService contactService)
        {
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        }

        // GET: Contact
        public ActionResult Index()
        {
            return View(new ContactViewModel());
        }

        // POST: Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string ipAddress = Request.UserHostAddress;
                
                // Submit the contact form
                int contactId = _contactService.SubmitContactForm(
                    model.FullName,
                    model.Email,
                    model.Subject,
                    model.Message,
                    ipAddress
                );
                
                // Set success flag to show confirmation message
                model.SubmissionSuccessful = true;
                
                // Clear form fields but keep name and email for convenience
                string fullName = model.FullName;
                string email = model.Email;
                model = new ContactViewModel
                {
                    FullName = fullName,
                    Email = email,
                    SubmissionSuccessful = true
                };
                
                TempData["SuccessMessage"] = "Thank you for contacting us! We will get back to you soon.";
                
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while submitting your message: " + ex.Message);
                return View(model);
            }
        }

        // Admin area actions
        [Authorize(Roles = "Admin")]
        public ActionResult List(string status = "")
        {
            try
            {
                var contacts = string.IsNullOrEmpty(status) 
                    ? _contactService.GetAllContacts() 
                    : _contactService.GetContactsByStatus(status);
                
                var viewModel = new ContactListViewModel
                {
                    Contacts = contacts,
                    CurrentStatus = status,
                    TotalContacts = contacts.Count,
                    UnreadCount = _contactService.GetUnreadContacts().Count
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading contacts: " + ex.Message;
                return View(new ContactListViewModel());
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            try
            {
                var contact = _contactService.GetContactById(id);
                
                if (contact == null)
                {
                    TempData["ErrorMessage"] = "Contact not found.";
                    return RedirectToAction("List");
                }
                
                // Mark as read if it's not already
                if (!contact.IsRead)
                {
                    _contactService.MarkAsRead(id);
                    // Refresh contact after marking as read
                    contact = _contactService.GetContactById(id);
                }
                
                var viewModel = new ContactDetailsViewModel
                {
                    Contact = contact
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult SendResponse(int id, string responseMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(responseMessage))
                {
                    TempData["ErrorMessage"] = "Response message cannot be empty.";
                    return RedirectToAction("Details", new { id });
                }
                
                var contact = _contactService.GetContactById(id);
                
                if (contact == null)
                {
                    TempData["ErrorMessage"] = "Contact not found.";
                    return RedirectToAction("List");
                }
                
                // In a real application, you would send an email here
                // For this example, we'll just mark it as responded
                
                _contactService.MarkResponseSent(id);
                
                TempData["SuccessMessage"] = "Response sent successfully.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string status)
        {
            try
            {
                _contactService.UpdateStatus(id, status);
                TempData["SuccessMessage"] = "Status updated successfully.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                bool result = _contactService.DeleteContact(id);
                
                if (result)
                {
                    TempData["SuccessMessage"] = "Contact deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete contact.";
                }
                
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("List");
            }
        }
    }
}
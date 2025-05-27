// eUseControl.Web/Controllers/MedicalConsultationController.cs
using System;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class MedicalConsultationController : Controller
    {
        private readonly IMedicalConsultationService _consultationService;

        // Dependency Injection via constructor
        public MedicalConsultationController(IMedicalConsultationService consultationService)
        {
            _consultationService = consultationService ?? throw new ArgumentNullException(nameof(consultationService));
        }

        // GET: MedicalConsultation
        public ActionResult Index()
        {
            return View(new MedicalConsultationViewModel
            {
                PreferredDate = DateTime.Now.AddDays(1)
            });
        }

        // POST: MedicalConsultation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MedicalConsultationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var consultation = new MedicalConsultation
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PreferredDate = model.PreferredDate,
                    ConsultationType = model.ConsultationType,
                    AdditionalNotes = model.AdditionalNotes
                };

                // Set UserId if user is logged in
                if (User.Identity.IsAuthenticated && Session["UserId"] != null)
                {
                    consultation.UserId = Convert.ToInt32(Session["UserId"]);
                }

                _consultationService.RequestConsultation(consultation);

                TempData["SuccessMessage"] = "Your consultation request has been submitted successfully. We will contact you shortly to confirm the appointment.";
                return RedirectToAction("ThankYou");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View(model);
            }
        }

        // GET: MedicalConsultation/ThankYou
        public ActionResult ThankYou()
        {
            return View();
        }

        // GET: MedicalConsultation/MyConsultations
        [Authorize]
        public ActionResult MyConsultations()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var consultations = _consultationService.GetConsultationsByUserId(userId);
                return View(consultations);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: MedicalConsultation/Cancel/5
        [Authorize]
        public ActionResult Cancel(int id)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var consultation = _consultationService.GetConsultationById(id);

                if (consultation == null || consultation.UserId != userId)
                {
                    return HttpNotFound();
                }

                if (consultation.Status == "Completed")
                {
                    TempData["ErrorMessage"] = "Cannot cancel a completed consultation.";
                    return RedirectToAction("MyConsultations");
                }

                _consultationService.CancelConsultation(id);
                TempData["SuccessMessage"] = "Consultation cancelled successfully.";
                return RedirectToAction("MyConsultations");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("MyConsultations");
            }
        }
    }
}
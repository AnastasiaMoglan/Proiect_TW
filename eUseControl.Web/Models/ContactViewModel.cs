using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name must be between 2 and 100 characters", MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email must be no more than 100 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject must be between 5 and 200 characters", MinimumLength = 5)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000, ErrorMessage = "Message must be between 10 and 2000 characters", MinimumLength = 10)]
        public string Message { get; set; }

        // Company contact information
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        
        // Flag to indicate if form submission was successful
        public bool SubmissionSuccessful { get; set; }
    }
}
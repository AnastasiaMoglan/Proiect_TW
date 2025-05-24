using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class MedicalConsultationViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Preferred Date")]
        [DataType(DataType.Date)]
        public DateTime PreferredDate { get; set; }

        [Required]
        [Display(Name = "Consultation Type")]
        public string ConsultationType { get; set; }

        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class OrderTrackingViewModel
    {
        [Required(ErrorMessage = "Order number is required")]
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public OrderViewModel Order { get; set; }

        public bool HasTrackedOrder => Order != null;
    }
}
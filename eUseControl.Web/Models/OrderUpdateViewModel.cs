using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class OrderUpdateViewModel
    {
        public int OrderId { get; set; }
        
        [Required(ErrorMessage = "Order status is required")]
        public string OrderStatus { get; set; }
        
        [Required(ErrorMessage = "Payment status is required")]
        public string PaymentStatus { get; set; }
        
        public string TrackingNumber { get; set; }
    }
}
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Models
{
    public class ContactListViewModel
    {
        public List<Contact> Contacts { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentStatus { get; set; }
        public int TotalContacts { get; set; }
        public int UnreadCount { get; set; }
    }

    public class ContactDetailsViewModel
    {
        public Contact Contact { get; set; }
        public string ResponseMessage { get; set; }
    }
}
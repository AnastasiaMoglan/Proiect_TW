using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Services
{
    public interface IContactService
    {
        List<Contact> GetAllContacts();
        Contact GetContactById(int id);
        int SubmitContactForm(string fullName, string email, string subject, string message, string ipAddress = null);
        void MarkAsRead(int id);
        void UpdateStatus(int id, string status);
        void MarkResponseSent(int id);
        List<Contact> GetContactsByStatus(string status);
        List<Contact> GetUnreadContacts();
        bool DeleteContact(int id);
    }
}
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Domain.Interfaces
{
    public interface IContactRepository
    {
        List<Contact> GetAllContacts();
        Contact GetContactById(int id);
        int CreateContact(Contact contact);
        void MarkAsRead(int id);
        void UpdateStatus(int id, string status);
        void MarkResponseSent(int id);
        List<Contact> GetContactsByStatus(string status);
        List<Contact> GetUnreadContacts();
        bool DeleteContact(int id);
    }
}
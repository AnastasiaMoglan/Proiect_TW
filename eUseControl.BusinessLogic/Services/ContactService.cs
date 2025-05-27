using System;
using System.Collections.Generic;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        }

        public List<Contact> GetAllContacts()
        {
            return _contactRepository.GetAllContacts();
        }

        public Contact GetContactById(int id)
        {
            return _contactRepository.GetContactById(id);
        }

        public int SubmitContactForm(string fullName, string email, string subject, string message, string ipAddress = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required");
                
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");
                
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Subject is required");
                
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message is required");
            
            // Create contact entity
            var contact = new Contact
            {
                FullName = fullName,
                Email = email,
                Subject = subject,
                Message = message,
                IPAddress = ipAddress,
                DateSubmitted = DateTime.Now
            };
            
            // Save to database
            return _contactRepository.CreateContact(contact);
        }

        public void MarkAsRead(int id)
        {
            _contactRepository.MarkAsRead(id);
        }

        public void UpdateStatus(int id, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty");
                
            _contactRepository.UpdateStatus(id, status);
        }

        public void MarkResponseSent(int id)
        {
            _contactRepository.MarkResponseSent(id);
        }

        public List<Contact> GetContactsByStatus(string status)
        {
            return _contactRepository.GetContactsByStatus(status);
        }

        public List<Contact> GetUnreadContacts()
        {
            return _contactRepository.GetUnreadContacts();
        }

        public bool DeleteContact(int id)
        {
            return _contactRepository.DeleteContact(id);
        }
    }
}
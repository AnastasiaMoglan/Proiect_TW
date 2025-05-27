// eUseControl.BusinessLogic/Services/MedicalConsultationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class MedicalConsultationService : IMedicalConsultationService
    {
        private readonly IMedicalConsultationRepository _consultationRepository;

        public MedicalConsultationService(IMedicalConsultationRepository consultationRepository)
        {
            _consultationRepository = consultationRepository ?? throw new ArgumentNullException(nameof(consultationRepository));
        }

        public List<MedicalConsultation> GetAllConsultations()
        {
            return _consultationRepository.GetAllConsultations();
        }

        public MedicalConsultation GetConsultationById(int id)
        {
            return _consultationRepository.GetConsultationById(id);
        }

        public List<MedicalConsultation> GetConsultationsByUserId(int userId)
        {
            return _consultationRepository.GetConsultationsByUserId(userId);
        }

        public List<MedicalConsultation> GetConsultationsByStatus(string status)
        {
            return _consultationRepository.GetConsultationsByStatus(status);
        }

        public int RequestConsultation(MedicalConsultation consultation)
        {
            // Business logic validation
            if (consultation.PreferredDate < DateTime.Now.Date)
            {
                throw new ArgumentException("Preferred date cannot be in the past.");
            }

            // Additional validation as needed
            if (string.IsNullOrWhiteSpace(consultation.FullName) || 
                string.IsNullOrWhiteSpace(consultation.Email) ||
                string.IsNullOrWhiteSpace(consultation.PhoneNumber) ||
                string.IsNullOrWhiteSpace(consultation.ConsultationType))
            {
                throw new ArgumentException("Required fields cannot be empty.");
            }

            return _consultationRepository.CreateConsultation(consultation);
        }

        public void UpdateConsultation(MedicalConsultation consultation)
        {
            // Business logic validation
            if (consultation.PreferredDate < DateTime.Now.Date)
            {
                throw new ArgumentException("Preferred date cannot be in the past.");
            }

            _consultationRepository.UpdateConsultation(consultation);
        }

        public void UpdateConsultationStatus(int id, string status)
        {
            // Validate status
            var validStatuses = new[] { "Pending", "Confirmed", "Completed", "Cancelled" };
            if (!validStatuses.Contains(status))
            {
                throw new ArgumentException("Invalid status value.");
            }

            _consultationRepository.UpdateConsultationStatus(id, status);
        }

        public void CancelConsultation(int id)
        {
            _consultationRepository.UpdateConsultationStatus(id, "Cancelled");
        }
    }
}
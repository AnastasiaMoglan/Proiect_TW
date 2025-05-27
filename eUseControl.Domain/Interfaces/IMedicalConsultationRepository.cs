// eUseControl.Domain/Interfaces/IMedicalConsultationRepository.cs
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Domain.Interfaces
{
    public interface IMedicalConsultationRepository
    {
        List<MedicalConsultation> GetAllConsultations();
        MedicalConsultation GetConsultationById(int id);
        List<MedicalConsultation> GetConsultationsByUserId(int userId);
        List<MedicalConsultation> GetConsultationsByStatus(string status);
        int CreateConsultation(MedicalConsultation consultation);
        void UpdateConsultation(MedicalConsultation consultation);
        void UpdateConsultationStatus(int id, string status);
        void DeleteConsultation(int id);
    }
}
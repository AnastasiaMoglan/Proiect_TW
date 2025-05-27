// eUseControl.BusinessLogic/Interfaces/IMedicalConsultationService.cs
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IMedicalConsultationService
    {
        List<MedicalConsultation> GetAllConsultations();
        MedicalConsultation GetConsultationById(int id);
        List<MedicalConsultation> GetConsultationsByUserId(int userId);
        List<MedicalConsultation> GetConsultationsByStatus(string status);
        int RequestConsultation(MedicalConsultation consultation);
        void UpdateConsultation(MedicalConsultation consultation);
        void UpdateConsultationStatus(int id, string status);
        void CancelConsultation(int id);
    }
}
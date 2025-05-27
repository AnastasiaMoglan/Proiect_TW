// eUseControl.Data/Repository/MedicalConsultationRepository.cs
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Data.Repository
{
    public class MedicalConsultationRepository : IMedicalConsultationRepository
    {
        private readonly AppDbContext _db;

        public MedicalConsultationRepository()
        {
            _db = new AppDbContext();
        }

        public List<MedicalConsultation> GetAllConsultations()
        {
            return _db.MedicalConsultation.Where(c => c.IsActive).OrderByDescending(c => c.CreatedAt).ToList();
        }

        public MedicalConsultation GetConsultationById(int id)
        {
            return _db.MedicalConsultation.FirstOrDefault(c => c.Id == id && c.IsActive);
        }

        public List<MedicalConsultation> GetConsultationsByUserId(int userId)
        {
            return _db.MedicalConsultation
                .Where(c => c.UserId == userId && c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public List<MedicalConsultation> GetConsultationsByStatus(string status)
        {
            return _db.MedicalConsultation
                .Where(c => c.Status == status && c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public int CreateConsultation(MedicalConsultation consultation)
        {
            consultation.CreatedAt = DateTime.Now;
            consultation.Status = "Pending";
            consultation.IsActive = true;
            
            _db.MedicalConsultation.Add(consultation);
            _db.SaveChanges();
            
            return consultation.Id;
        }

        public void UpdateConsultation(MedicalConsultation consultation)
        {
            var existingConsultation = _db.MedicalConsultation.Find(consultation.Id);
            
            if (existingConsultation != null)
            {
                existingConsultation.FullName = consultation.FullName;
                existingConsultation.Email = consultation.Email;
                existingConsultation.PhoneNumber = consultation.PhoneNumber;
                existingConsultation.PreferredDate = consultation.PreferredDate;
                existingConsultation.ConsultationType = consultation.ConsultationType;
                existingConsultation.AdditionalNotes = consultation.AdditionalNotes;
                existingConsultation.Status = consultation.Status;
                existingConsultation.UpdatedAt = DateTime.Now;
                
                _db.Entry(existingConsultation).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void UpdateConsultationStatus(int id, string status)
        {
            var consultation = _db.MedicalConsultation.Find(id);
            
            if (consultation != null)
            {
                consultation.Status = status;
                consultation.UpdatedAt = DateTime.Now;
                
                _db.Entry(consultation).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void DeleteConsultation(int id)
        {
            var consultation = _db.MedicalConsultation.Find(id);
            
            if (consultation != null)
            {
                consultation.IsActive = false;
                consultation.UpdatedAt = DateTime.Now;
                
                _db.Entry(consultation).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }
    }
}
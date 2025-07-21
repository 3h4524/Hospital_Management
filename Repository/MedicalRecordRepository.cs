using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class MedicalRecordRepository : BaseRepository<MedicalRecord>
    {
        public MedicalRecordRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByAppointment(int appointmentId)
        {
            return await _dbSet
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                .Include(mr => mr.Specialization)
                .Where(mr => mr.AppointmentId == appointmentId)
                .OrderBy(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatient(int patientId)
        {
            return await _dbSet
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                .Include(mr => mr.Specialization)
                .Where(mr => mr.Appointment.PatientId == patientId)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<MedicalRecord?> GetMedicalRecordWithDetails(int recordId)
        {
            return await _dbSet
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                .Include(mr => mr.Specialization)
                .FirstOrDefaultAsync(mr => mr.RecordId == recordId);
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByDoctor(int doctorId, DateOnly? date = null)
        {
            var query = _dbSet
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                .Include(mr => mr.Specialization)
                .Where(mr => mr.Appointment.DoctorId == doctorId);

            if (date.HasValue)
            {
                query = query.Where(mr => mr.Appointment.AppointmentDate == date.Value);
            }

            return await query
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetMedicalRecordCountByAppointment(int appointmentId)
        {
            return await _dbSet
                .CountAsync(mr => mr.AppointmentId == appointmentId);
        }
    }
} 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class AppointmentRepository : BaseRepository<Appointment>
    {
        public AppointmentRepository(HospitalManagementContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDate(DateOnly date)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate == date)
                .OrderBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(int doctorId, DateOnly? date = null)
        {
            var query = _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.MedicalRecords)
                .Where(a => a.DoctorId == doctorId);

            if (date.HasValue)
            {
                query = query.Where(a => a.AppointmentDate == date.Value);
            }

            return await query
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatient(int patientId)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.MedicalRecords)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByDoctorAndTime(int doctorId, DateOnly date, TimeOnly timeSlot)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => 
                    a.DoctorId == doctorId && 
                    a.AppointmentDate == date && 
                    a.TimeSlot == timeSlot);
        }

        public async Task<Appointment?> GetAppointmentWithDetails(int appointmentId)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.MedicalRecords)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatus(string status)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == status)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithDetails()
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.MedicalRecords)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }
    }
} 
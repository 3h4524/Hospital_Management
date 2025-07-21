using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Model;
using Repository;

namespace Service
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<bool> CreatePatient(string fullName, string phoneNumber, string email, string gender, string address)
        {
            // Kiểm tra email đã tồn tại chưa
            if (!string.IsNullOrEmpty(email))
            {
                var existingPatient = await _patientRepository.FindByEmail(email);
                if (existingPatient != null)
                    return false;
            }

            var patient = new Patient
            {
                FullName = fullName,
                PhoneNumber = phoneNumber,
                Email = email,
                Gender = gender,
                Address = address,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _patientRepository.Add(patient);
            return true;
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            return await _patientRepository.GetAllPatientsWithAppointments();
        }

        public async Task<Patient?> GetPatientById(int patientId)
        {
            return await _patientRepository.GetPatientWithDetails(patientId);
        }

        public async Task<Patient?> GetPatientByEmail(string email)
        {
            return await _patientRepository.FindByEmail(email);
        }

        public async Task<Patient?> GetPatientByPhone(string phoneNumber)
        {
            return await _patientRepository.FindByPhone(phoneNumber);
        }

        public async Task<bool> UpdatePatient(int patientId, string fullName, string phoneNumber, string email, string gender, string address)
        {
            var patient = await _patientRepository.FindByID(patientId);
            if (patient == null)
                return false;

            // Kiểm tra email mới có bị trùng không
            if (!string.IsNullOrEmpty(email) && email != patient.Email)
            {
                var existingPatient = await _patientRepository.FindByEmail(email);
                if (existingPatient != null)
                    return false;
            }

            patient.FullName = fullName;
            patient.PhoneNumber = phoneNumber;
            patient.Email = email;
            patient.Gender = gender;
            patient.Address = address;
            patient.UpdatedAt = DateTime.Now;

            await _patientRepository.Update(patient);
            return true;
        }

        public async Task<bool> DeletePatient(int patientId)
        {
            var patient = await _patientRepository.FindByID(patientId);
            if (patient == null)
                return false;

            await _patientRepository.Delete(patient);
            return true;
        }

        public async Task<IEnumerable<Patient>> SearchPatients(string searchTerm)
        {
            return await _patientRepository.SearchPatients(searchTerm);
        }
    }
} 
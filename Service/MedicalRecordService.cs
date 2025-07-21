using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Model;
using Repository;

namespace Service
{
    public class MedicalRecordService
    {
        private readonly MedicalRecordRepository _medicalRecordRepository;
        private readonly AppointmentRepository _appointmentRepository;

        public MedicalRecordService(MedicalRecordRepository medicalRecordRepository, AppointmentRepository appointmentRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> CreateMedicalRecord(MedicalRecordRequest request)
        {
            // Kiểm tra appointment có tồn tại không
            var appointment = await _appointmentRepository.FindByID(request.AppointmentId);
            if (appointment == null)
                return false;

            var medicalRecord = new MedicalRecord
            {
                AppointmentId = request.AppointmentId,
                SpecializationId = request.SpecId,
                CreatedAt = DateTime.Now
            };

            await _medicalRecordRepository.Add(medicalRecord);
            return true;
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByAppointment(int appointmentId)
        {
            return await _medicalRecordRepository.GetMedicalRecordsByAppointment(appointmentId);
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatient(int patientId)
        {
            return await _medicalRecordRepository.GetMedicalRecordsByPatient(patientId);
        }

        public async Task<MedicalRecord?> GetMedicalRecordById(int recordId)
        {
            return await _medicalRecordRepository.GetMedicalRecordWithDetails(recordId);
        }

        public async Task<bool> UpdateMedicalRecord(int recordId, string diagnosis, string notes)
        {
            var medicalRecord = await _medicalRecordRepository.FindByID(recordId);
            if (medicalRecord == null)
                return false;

            medicalRecord.Diagnosis = diagnosis;
            medicalRecord.Notes = notes;

            await _medicalRecordRepository.Update(medicalRecord);
            return true;
        }

        public async Task<bool> DeleteMedicalRecord(int recordId)
        {
            var medicalRecord = await _medicalRecordRepository.FindByID(recordId);
            if (medicalRecord == null)
                return false;

            await _medicalRecordRepository.Delete(medicalRecord);
            return true;
        }

        public async Task<bool> CreateSummaryRecord(MedicalRecord summaryRecord)
        {
            try
            {
                await _medicalRecordRepository.Add(summaryRecord);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateSummaryAndDeleteOld(List<MedicalRecord> summaryRecords, List<int> oldRecordIds)
        {
            try
            {
                // Tạo phiếu tổng hợp
                foreach (var record in summaryRecords)
                {
                    await _medicalRecordRepository.Add(record);
                }

                // Xóa phiếu cũ
                foreach (var recordId in oldRecordIds)
                {
                    var oldRecord = await _medicalRecordRepository.FindByID(recordId);
                    if (oldRecord != null)
                    {
                        await _medicalRecordRepository.Delete(oldRecord);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 
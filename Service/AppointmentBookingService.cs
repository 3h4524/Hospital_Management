using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DTO.Response;
using DTO.Request;
using Repository;
using Model;

namespace Service
{
    public class AppointmentBookingService
    {
        private DoctorRepository _doctorRepository;
        private AppointmentRepository _appointmentRepository;
        private PatientRepository _patientRepository;


        public AppointmentBookingService(HospitalManagementContext context)
        {
            _doctorRepository = new DoctorRepository(context);
            _appointmentRepository = new AppointmentRepository(context);
            _patientRepository = new PatientRepository(context);
        }

        public async Task<List<DoctorInfomationResponse>> fetchDoctorInformation(int pageNumber, int pageSize, DoctorInformationSearch search = null)
        {
            return await _doctorRepository.FindAllBySpecificationWithSubquery(pageNumber, pageSize, search);
        }

        public async Task CreateAppointment(PatientRequestDto patientRequest, BookingRequestDto bookingRequest)
        {
            Patient patient = (await _patientRepository.Find(p => p.PhoneNumber == patientRequest.PhoneNumber)).FirstOrDefault();
            if (patient == null)
            {
                patient = new Patient
                {
                    FullName = patientRequest.FullName,
                    PhoneNumber = patientRequest.PhoneNumber,
                    Gender = patientRequest.Gender,
                    CreatedAt = DateTime.Now
                };

                await _patientRepository.Add(patient);
            }
            else
            {
                if (patient.FullName != patientRequest.FullName)
                    throw new ArgumentException("Full name does not match");
            }

            Appointment appointment = new Appointment
            {
                PatientId = patient.PatientId,
                DoctorId = bookingRequest.DoctorId,
                AppointmentDate = bookingRequest.BookingDate,
                TimeSlot = TimeOnly.FromTimeSpan(bookingRequest.EndTime - bookingRequest.StartTime),
                Reason = bookingRequest.Reason,
                Status = "Pending",
            };

            await _appointmentRepository.Add(appointment);
        }
    }
}

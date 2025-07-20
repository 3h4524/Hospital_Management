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
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Diagnostics;

namespace Service
{
    public class AppointmentBookingService
    {
        private DoctorRepository _doctorRepository;
        private DoctorScheduleRepository _doctorScheduleRepository;
        private AppointmentRepository _appointmentRepository;

  

        public AppointmentBookingService(DoctorScheduleRepository doctorScheduleRepository, 
            DoctorRepository doctorRepository, AppointmentRepository appointmentRepository) 
        {
            _doctorRepository = doctorRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<DoctorInfomationResponse>> fetchDoctorInformation(int pageNumber, int pageSize, DoctorInformationSearch search = null)
        {
            return await _doctorRepository.FindAllBySpecificationWithSubquery(pageNumber, pageSize, search);   
        }

            public async Task<List<DoctorAppointmentResponse>> GetScheduleForDoctor(int doctorId, DateOnly date)
            {
                DoctorSchedule? doctorSchedule = await _doctorScheduleRepository.FindByDoctorIdAndDate(doctorId, date);
                if (doctorSchedule == null)
                    return new();

                Debug.WriteLine($"Doctor: {doctorSchedule.WorkDate}");
                List<Appointment> appointments = await _appointmentRepository.GetAllByDoctorIdAndWorkDate(doctorId, date);
                List<DoctorAppointmentResponse> result = new ();

                TimeOnly current = doctorSchedule.StartTime;

                foreach(var appointment in appointments)
                {
                    if(current < appointment.StartTime)
                    {
                        result.Add(new DoctorAppointmentResponse
                        {
                            StartTime = current,
                            EndTime = appointment.StartTime,
                            PatientName = "Trống",
                            DoctorId = appointment.DoctorId,
                            TimeSlot = appointment.StartTime - current,
                        });
                    }

                    result.Add(new DoctorAppointmentResponse
                    {
                        StartTime = appointment.StartTime,
                        EndTime = appointment.EndTime,
                        PatientName = appointment.Patient.FullName,
                        PatientId = appointment.PatientId,
                        DoctorId = appointment.DoctorId,
                        TimeSlot = appointment.EndTime - appointment.StartTime,
                        AppointmentId = appointment.AppointmentId
                    });

                    current = appointment.EndTime;
                }



                if (current < doctorSchedule.EndTime)
                {
                    result.Add(new DoctorAppointmentResponse
                    {
                        StartTime = current,
                        EndTime = doctorSchedule.EndTime,
                        PatientName = "Trống",
                        DoctorId = doctorSchedule.DoctorId,
                        TimeSlot = doctorSchedule.EndTime - current,
                    });
                }
                return result;
            }

        public async Task<bool> CreateOrUpdateAppointment(Appointment appointment)
        {
            var doctorSchedule = await _doctorScheduleRepository.FindByDoctorIdAndDate(appointment.DoctorId, appointment.AppointmentDate);
            if(appointment.StartTime < doctorSchedule.StartTime || appointment.EndTime > doctorSchedule.EndTime)
            {
                return false;
            }
            List<Appointment> appointments = await _appointmentRepository
                .GetAllByDoctorIdAndWorkDate(appointment.DoctorId, appointment.AppointmentDate);

            Debug.WriteLine($"Appointment: {appointment.ToString()}");

            foreach (var ap in appointments)
            {
                // Bỏ qua chính nó khi cập nhật
                if (ap.AppointmentId == appointment.AppointmentId)
                    continue;

                bool isOverlapping = (appointment.EndTime > ap.StartTime && appointment.StartTime < ap.EndTime);
                if (isOverlapping)
                {
                    return false; // Trùng thời gian với lịch khác
                }
            }

            if (appointment.AppointmentId == 0)
            {
                await _appointmentRepository.Add(appointment);
            }
            else
            {
                await _appointmentRepository.Update(appointment);
            }

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Model;
using Repository;

namespace Service
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;
        private readonly PatientRepository _patientRepository;
        private readonly SystemUserRepository _userRepository;
        private readonly DoctorScheduleRepository _doctorScheduleRepository;

        public AppointmentService(
            AppointmentRepository appointmentRepository,
            PatientRepository patientRepository,
            SystemUserRepository userRepository,
            DoctorScheduleRepository doctorScheduleRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
        }

        public async Task<bool> CreateAppointment(int patientId, int doctorId, DateOnly appointmentDate, TimeOnly timeSlot, string reason)
        {
            try
            {
                // Kiểm tra doctor có tồn tại và là doctor không
                var doctor = await _userRepository.FindByID(doctorId);
                if (doctor == null || doctor.Role != "Doctor")
                {
                    Console.WriteLine($"Doctor not found or not a doctor: {doctorId}");
                    return false;
                }

                // Kiểm tra patient có tồn tại không
                var patient = await _patientRepository.FindByID(patientId);
                if (patient == null)
                {
                    Console.WriteLine($"Patient not found: {patientId}");
                    return false;
                }

                // Kiểm tra doctor có lịch làm việc vào thời gian này không
                var doctorSchedule = await _doctorScheduleRepository.GetScheduleForDoctor(doctorId);
                Console.WriteLine($"Found {doctorSchedule.Count()} schedules for doctor {doctorId}");
                
                var hasSchedule = doctorSchedule.Any(s => 
                {
                    var isAvailable = s.WorkDate == appointmentDate && 
                                     s.StartTime <= timeSlot && 
                                     s.EndTime >= timeSlot && 
                                     s.IsAvailable;
                    
                    if (s.WorkDate == appointmentDate)
                    {
                        Console.WriteLine($"Schedule on {appointmentDate}: {s.StartTime} - {s.EndTime}, Available: {s.IsAvailable}, Matches: {isAvailable}");
                    }
                    
                    return isAvailable;
                });

                if (!hasSchedule)
                {
                    Console.WriteLine($"No available schedule for doctor {doctorId} on {appointmentDate} at {timeSlot}");
                    return false;
                }

                // Kiểm tra slot đã được đặt chưa
                var existingAppointment = await _appointmentRepository.GetAppointmentByDoctorAndTime(doctorId, appointmentDate, timeSlot);
                if (existingAppointment != null)
                {
                    Console.WriteLine($"Appointment already exists for doctor {doctorId} on {appointmentDate} at {timeSlot}");
                    return false;
                }

                var appointment = new Appointment
                {
                    PatientId = patientId,
                    DoctorId = doctorId,
                    AppointmentDate = appointmentDate,
                    TimeSlot = timeSlot,
                    Reason = reason,
                    Status = "Scheduled"
                };

                await _appointmentRepository.Add(appointment);
                Console.WriteLine($"Appointment created successfully: {appointment.AppointmentId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating appointment: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDate(DateOnly date)
        {
            return await _appointmentRepository.GetAppointmentsByDate(date);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(int doctorId, DateOnly? date = null)
        {
            return await _appointmentRepository.GetAppointmentsByDoctor(doctorId, date);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatient(int patientId)
        {
            return await _appointmentRepository.GetAppointmentsByPatient(patientId);
        }

        public async Task<bool> UpdateAppointmentStatus(int appointmentId, string status)
        {
            var appointment = await _appointmentRepository.FindByID(appointmentId);
            if (appointment == null)
                return false;

            appointment.Status = status;
            await _appointmentRepository.Update(appointment);
            return true;
        }

        public async Task<Appointment?> GetAppointmentById(int appointmentId)
        {
            return await _appointmentRepository.GetAppointmentWithDetails(appointmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            try
            {
                return await _appointmentRepository.GetAllAppointmentsWithDetails();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all appointments: {ex.Message}");
                return new List<Appointment>();
            }
        }
    }
} 
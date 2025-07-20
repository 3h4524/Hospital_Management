using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Model;
using Repository;
using BCrypt;
using System.Runtime.InteropServices;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Service
{
    public class DoctorScheduleService
    {
        private readonly SystemUserRepository _userRepository;
        private readonly DoctorScheduleRepository _doctorScheduleRepository;

        public DoctorScheduleService(HospitalManagementContext context)
        {
            _userRepository = new(context);
            _doctorScheduleRepository = new(context);
        }


        public async Task<IEnumerable<DoctorSchedule>> GetDoctorSchedulesByMonth(int DoctorId, int month, int year)
        {
            return await _doctorScheduleRepository.GetDoctorSchedulesByMonth(DoctorId, month, year);
        }

        public async Task<bool> RequestSessionOff(DoctorSchedule schedule)
        {

            schedule.Status = "Pending";
            await _doctorScheduleRepository.Update(schedule);
            return true;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetAllPendingDayOffRequests()
        {
            return await _doctorScheduleRepository.GetAllPendingDayOffRequests();
        }

        public async Task UpdateScheduleStatus(DoctorSchedule schedule)
        {
            await _doctorScheduleRepository.Update(schedule);
        }

    }


}

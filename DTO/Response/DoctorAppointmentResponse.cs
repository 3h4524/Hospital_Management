using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Response
{
    public class DoctorAppointmentResponse
    {
        public int DoctorId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public TimeSpan TimeSlot { get; set; }
        public int AppointmentId { get; set; }

        public override string ToString()
        {
            return $"DoctorId: {DoctorId}, StartTime: {StartTime}, EndTime: {EndTime}, PatientName: {PatientName}, PatientId: {PatientId}, AppointmentId: {AppointmentId}";
        }
    }
}

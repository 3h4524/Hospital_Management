using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Response
{
    public class DailyScheduleReportResponse()
    {
        public int DoctorId { get; set; }

        public DateOnly WorkDate { get; set; }

        public float WorkingHours { get; set; }

        public float LateHours { get; set; }

        public string Status { get; set; }

        public override string ToString()
        {
            return $"Doctor ID: {DoctorId}" +
                   $"Date: {WorkDate}" +
                   $"Working Hours: {WorkingHours:F2}, " +
                   $"Late Hours: {LateHours:F2}" +
                   $"Status: {Status}";
        }

    }
}

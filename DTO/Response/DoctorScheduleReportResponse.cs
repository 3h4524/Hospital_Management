using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Response
{
    public class DoctorScheduleReportResponse()
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public int TotalDaysWorked { get; set; }

        public Double TotalWorkingHours { get; set; }

        public Double TotalLateHours { get; set; }

        public override string ToString()
        {
            return $"Doctor ID: {DoctorId}, Name: {DoctorName}, " +
                   $"Total Days Worked: {TotalDaysWorked}, " +
                   $"Total Working Hours: {TotalWorkingHours:F2}, " +
                   $"Total Late Hours: {TotalLateHours:F2}";
        }

    }
}

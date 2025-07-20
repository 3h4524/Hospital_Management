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

        public float TotalWorkingHours { get; set; }

        public float TotalLateHours { get; set; }

        public int TotalDaysOff { get; set; }

        public decimal TotalRewardAmount { get; set; }

        public decimal TotalPenaltyAmount { get; set; }

        public override string ToString()
        {
            return $"Doctor ID: {DoctorId}, Name: {DoctorName}, " +
                   $"Total Days Worked: {TotalDaysWorked}, " +
                   $"Total Working Hours: {TotalWorkingHours:F2}, " +
                   $"Total Days Off: {TotalDaysOff}, " +
                   $"Total Late Hours: {TotalLateHours:F2}" +
                   $"Total Reward Amount: {TotalRewardAmount:F2}" +
                   $"Total Penalty Amount: {TotalPenaltyAmount:F2}";
        }

    }
}

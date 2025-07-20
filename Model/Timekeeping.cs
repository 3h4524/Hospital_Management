using System;
using System.Collections.Generic;

namespace Model;

public partial class Timekeeping
{
    public int TimeKeepingId { get; set; }

    public int UserId { get; set; }

    public int ScheduleId { get; set; }

    public DateOnly WorkDate { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual SystemUser User { get; set; } = null!;
    public virtual DoctorSchedule Schedule { get; set; } = null!;

    public override string ToString()
    {
        return $"TimekeepingID: {TimeKeepingId}, " +
               $"UserID: {UserId}, " +
               $"ScheduleID: {ScheduleId}, " +
               $"WorkDate: {WorkDate}, " +
               $"CheckIn: {(CheckInTime.HasValue ? CheckInTime.Value.ToString("HH:mm") : "null")}, " +
               $"CheckOut: {(CheckOutTime.HasValue ? CheckOutTime.Value.ToString("HH:mm") : "null")}, " +
               $"Status: {Status}, " +
               $"Note: {Note}";
    }

}

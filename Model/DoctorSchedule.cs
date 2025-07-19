using System;
using System.Collections.Generic;

namespace Model;

public partial class DoctorSchedule
{
    public int ScheduleId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly WorkDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public String Status { get; set; } = "Working";

    public virtual SystemUser Doctor { get; set; } = null!;
}

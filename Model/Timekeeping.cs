using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class Timekeeping
{
    public int TimeKeepingId { get; set; }

    public int? DoctorId { get; set; }

    public int? SystemUserId { get; set; }

    public DateOnly WorkDate { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual SystemUser? SystemUser { get; set; }
}

using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class Timekeeping
{
    public int TimeKeepingId { get; set; }

    public int UserId { get; set; }

    public DateOnly WorkDate { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual SystemUser User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class DoctorProfile
{
    public int UserId { get; set; }

    public string? Specialization { get; set; }

    public int? Dependencies { get; set; }

    public DateOnly? JoinDate { get; set; }

    public decimal? BaseSalary { get; set; }

    public virtual SystemUser User { get; set; } = null!;
}

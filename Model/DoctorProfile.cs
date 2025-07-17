using System;
using System.Collections.Generic;

namespace Model;

public partial class DoctorProfile
{
    public int UserId { get; set; }

    public string? Specialization { get; set; }

    public DateOnly? JoinDate { get; set; }

    public decimal? BaseSalary { get; set; }

    public string? Gender { get; set; }
    public string? Degree { get; set; }
    public string? Address { get; set; }

    public virtual SystemUser User { get; set; } = null!;
}

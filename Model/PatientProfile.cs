using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class PatientProfile
{
    public int UserId { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual SystemUser User { get; set; } = null!;
}

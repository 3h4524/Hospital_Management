using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class RewardPenalty
{
    public int RpId { get; set; }

    public int? DoctorId { get; set; }

    public int? SystemUserId { get; set; }

    public DateOnly Rpdate { get; set; }

    public string? Type { get; set; }

    public decimal Amount { get; set; }

    public string? Reason { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual SystemUser? SystemUser { get; set; }
}

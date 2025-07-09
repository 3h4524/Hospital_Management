using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class SystemUser
{
    public int SystemUserId { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string HashPassword { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? FullName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<RewardPenalty> RewardPenalties { get; set; } = new List<RewardPenalty>();

    public virtual ICollection<Timekeeping> Timekeepings { get; set; } = new List<Timekeeping>();
}

using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int UserId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal BaseSalary { get; set; }

    public int? WorkingDays { get; set; }

    public decimal? TotalReward { get; set; }

    public decimal? TotalPenalty { get; set; }

    public decimal? FinalSalary { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public virtual SystemUser User { get; set; } = null!;
}

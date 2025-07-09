using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string HashPassword { get; set; } = null!;

    public string? Specialization { get; set; }

    public DateOnly? JoinDate { get; set; }

    public decimal BaseSalary { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<RewardPenalty> RewardPenalties { get; set; } = new List<RewardPenalty>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<Timekeeping> Timekeepings { get; set; } = new List<Timekeeping>();
}

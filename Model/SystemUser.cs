using System;
using System.Collections.Generic;

namespace Model;

public partial class SystemUser
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string FullName { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Dependencies { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; } = new List<ChatMessage>();

    public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; } = new List<ChatMessage>();

    public virtual DoctorProfile? DoctorProfile { get; set; }

    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    public virtual ICollection<RewardPenalty> RewardPenalties { get; set; } = new List<RewardPenalty>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<Timekeeping> Timekeepings { get; set; } = new List<Timekeeping>();

    public virtual ICollection<EmailResetPassword> EmailResetPasswords { get; set; } = new List<EmailResetPassword>();
}

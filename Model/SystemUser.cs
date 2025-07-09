using System;
using System.Collections.Generic;

namespace Model;

public partial class SystemUser
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FullName { get; set; }

    public string HashPassword { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> AppointmentDoctors { get; set; } = new List<Appointment>();

    public virtual ICollection<Appointment> AppointmentPatients { get; set; } = new List<Appointment>();

    public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; } = new List<ChatMessage>();

    public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; } = new List<ChatMessage>();

    public virtual DoctorProfile? DoctorProfile { get; set; }

    public virtual PatientProfile? PatientProfile { get; set; }

    public virtual ICollection<RewardPenalty> RewardPenalties { get; set; } = new List<RewardPenalty>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<Timekeeping> Timekeepings { get; set; } = new List<Timekeeping>();
}

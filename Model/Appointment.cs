using System;
using System.Collections.Generic;

namespace Model;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly TimeSlot { get; set; }

    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public virtual SystemUser Doctor { get; set; } = null!;

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Patient Patient { get; set; } = null!;

    public override string ToString()
    {
        return $"Appointment ID: {AppointmentId}, " +
               $"Patient ID: {PatientId}, " +
               $"Doctor ID: {DoctorId}, " +
               $"Date: {AppointmentDate}, " +
               $"Start: {StartTime}, End: {EndTime}, " +
               $"TimeSlot: {TimeSlot}, " +
               $"Reason: {Reason}, " +
               $"Status: {Status}, " +
               $"Patient Name: {Patient?.FullName}, " +
               $"Doctor Name: {Doctor?.FullName}";
    }
}

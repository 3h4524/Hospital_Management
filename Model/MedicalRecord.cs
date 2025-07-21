using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("MedicalRecords")]
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }
        
        [Required]
        public int AppointmentId { get; set; }
        
        [Required]
        public int SpecializationId { get; set; }
        
        public string? Diagnosis { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }
        
        [ForeignKey("SpecializationId")]
        public virtual Specialization? Specialization { get; set; }
    }
}

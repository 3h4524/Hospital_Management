using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class MedicalRecordRequest
    {
        [Required]
        public int AppointmentId { get; set; }
        
        [Required]
        public int SpecId { get; set; }
    }
} 
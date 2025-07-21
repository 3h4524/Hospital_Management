using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Specialization")]
    public class Specialization
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
} 
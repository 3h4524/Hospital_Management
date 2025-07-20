using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Request
{
    public class DoctorBookingDto
    {
        public int DoctorId { get; set; }
        public DateOnly StartTime { get; set; }
        public DateOnly EndTime { get; set; }
    }
}

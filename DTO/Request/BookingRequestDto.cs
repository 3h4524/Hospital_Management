using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Request
{
    public class BookingRequestDto
    {
        public int DoctorId { get; set; }

        public DateOnly BookingDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string? Reason { get; set; }

    }
}

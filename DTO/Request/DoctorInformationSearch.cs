using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Request
{
    public class DoctorInformationSearch
    {
        public string Specification { get; set; }
        public string FullName { get; set; }
        public DateOnly Date {  get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}

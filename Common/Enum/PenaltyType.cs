using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Common.Enum
{
    public enum PenaltyType
    {
        [Display(Name = "Late Penalty Per Hour")]
        LatePerHour = 50000,

        [Display(Name = "Attendance Bonus")]
        AttendanceBonus = 500000
    }
}


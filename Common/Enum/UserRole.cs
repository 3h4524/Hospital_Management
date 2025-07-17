using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum
{
        public enum UserRole
        {
            [Display(Name = "Administrator")]
            Admin,
            [Display(Name = "Doctor")]
            Doctor,
            [Display(Name = "Receptionist")]
            Receptionist
        }
}

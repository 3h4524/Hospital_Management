using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class AppointmentRepository : BaseRepository<Appointment>
    {   
        public AppointmentRepository(HospitalManagementContext context) : base(context){}

        
    }
}

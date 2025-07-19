using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DTO.Response;
using DTO.Request;
using Repository;
using Model;

namespace Service
{
    public class AppointmentBookingService
    {
        private DoctorRepository _doctorRepository;
  

        public AppointmentBookingService(HospitalManagementContext context) 
        {
            _doctorRepository = new DoctorRepository(context);
        }

        public async Task<List<DoctorInfomationResponse>> fetchDoctorInformation(int pageNumber, int pageSize, DoctorInformationSearch search = null)
        {
            return await _doctorRepository.FindAllBySpecificationWithSubquery(pageNumber, pageSize, search);   
        }
    }
}

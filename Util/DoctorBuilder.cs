//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Model;

//namespace Util
//{
//    public class DoctorBuilder
//    {
//        {
//    private readonly GenericBuilder<DoctorProfile> _doctorBuilder;

//        public DoctorBuilder()
//        {
//            _doctorBuilder = new GenericBuilder<DoctorProfile>()
//                .WithProperty("UserId", (doctor, val) => doctor.UserId = Convert.ToInt32(val))
//                .WithProperty("Specialization", (doctor, val) => doctor.Specialization = val.ToString())
//        }

//        public DoctorBuilder Set(string propertyName, object value)
//        {
//            _doctorBuilder.Set(propertyName, value);
//            return this;
//        }

//        public DoctorProfile Build()
//        {
//            return _doctorBuilder.Build();
//        }
//    }
//}
//}

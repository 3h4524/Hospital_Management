using DataAccess;
using Model;
using Service;

namespace TestApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            await Haha();
        }

        private static async Task Haha()
        {
            HospitalManagementContext context = new();
            DoctorScheduleService a = new(context);
            List<DoctorSchedule> list =  (await a.GetDoctorSchedulesByMonth(1,7,2025)).ToList();
            list.ForEach(s => Console.WriteLine(s));
        }
    }

}

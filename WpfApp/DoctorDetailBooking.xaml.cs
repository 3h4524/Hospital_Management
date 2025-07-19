using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO.Response;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace View
{
    /// <summary>
    /// Interaction logic for DoctorDetailBooking.xaml
    /// </summary>
    public partial class DoctorDetailBooking : UserControl
    {
        private AppointmentBookingService _bookingService;
        private DoctorInfomationResponse _doctorInformation;
        public DoctorDetailBooking(DoctorInfomationResponse doctorInformation)
        {
            InitializeComponent();
            DataContext = this;
            _bookingService = App._serviceProvider.GetRequiredService<AppointmentBookingService>();
            _doctorInformation = doctorInformation;
        }

        private void Search_Doctor_Click(object sender, RoutedEventArgs e)
        {

        }

  
    }
}

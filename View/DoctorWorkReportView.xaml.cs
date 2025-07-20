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
using System.Collections.ObjectModel;
using Service;

namespace View
{
    /// <summary>
    /// Interaction logic for DoctorWorkReportView.xaml
    /// </summary>
    public partial class DoctorWorkReportView : UserControl
    {
        private AttendanceService _attendanceService;
        private ObservableCollection<DoctorScheduleReportResponse> doctorScheduleReports;
        public DoctorWorkReportView(AttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
            doctorScheduleReports = [];
            InitializeComponent();
        }
    }
}

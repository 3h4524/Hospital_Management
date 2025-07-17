using System.Windows.Controls;
using System.Windows;

namespace View
{
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
            MainContent.Content = new SalaryReportView();
        }
        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            // MainContent.Content = new DashboardView();
            // HeaderText.Text = "Dashboard";
        }
        private void BtnSalaryReport_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SalaryReportView();
            HeaderText.Text = "Báo cáo lương";
        }
        private void BtnDoctorProfile_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DoctorProfileView();
            HeaderText.Text = "Hồ sơ bác sĩ";
        }
        private void BtnPatientManagement_Click(object sender, RoutedEventArgs e)
        {
            //MainContent.Content = new PatientManagementView();
            HeaderText.Text = "Quản lý bệnh nhân";
        }
        private void BtnAppointment_Click(object sender, RoutedEventArgs e)
        {
            //MainContent.Content = new AppointmentView();
            HeaderText.Text = "Lịch hẹn";
        }
    }
} 
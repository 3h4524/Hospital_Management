using System;
using System.Windows;
using System.Windows.Controls;
using Service;
using Model;
using Repository;
using Microsoft.Extensions.DependencyInjection;

namespace View
{
    public partial class ReceptionistDashboardView : UserControl
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;
        private readonly SystemUser _currentUser;

        public ReceptionistDashboardView(AppointmentService appointmentService, PatientService patientService)
        {
            InitializeComponent();
            _appointmentService = appointmentService;
            _patientService = patientService;
            
            _currentUser = Application.Current.Properties["CurrentUser"] as SystemUser
                          ?? throw new InvalidOperationException("CurrentUser session is missing");
            
            HeaderText.Text = $"Welcome, {_currentUser.FullName}!";
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            // Load dashboard content
            var dashboardContent = new StackPanel { Margin = new Thickness(32) };
            
            // Welcome message
            var welcomeText = new TextBlock
            {
                Text = $"Chào mừng {_currentUser.FullName}!",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20)
            };
            dashboardContent.Children.Add(welcomeText);

            // Quick stats
            var statsGrid = new Grid();
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Today's appointments count
            var todayCard = CreateStatCard("Lịch hẹn hôm nay", "0", "#42A5F5");
            Grid.SetColumn(todayCard, 0);
            statsGrid.Children.Add(todayCard);

            // Total patients count
            var patientsCard = CreateStatCard("Tổng bệnh nhân", "0", "#66BB6A");
            Grid.SetColumn(patientsCard, 1);
            statsGrid.Children.Add(patientsCard);

            // Pending appointments count
            var pendingCard = CreateStatCard("Lịch hẹn chờ", "0", "#FFA726");
            Grid.SetColumn(pendingCard, 2);
            statsGrid.Children.Add(pendingCard);

            dashboardContent.Children.Add(statsGrid);
            MainContent.Content = dashboardContent;
        }

        private Border CreateStatCard(string title, string value, string color)
        {
            var card = new Border
            {
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(12),
                Margin = new Thickness(8),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 8,
                    ShadowDepth = 2,
                    Color = System.Windows.Media.Colors.Gray
                }
            };

            var content = new StackPanel { Margin = new Thickness(20) };
            
            var titleText = new TextBlock
            {
                Text = title,
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(0, 0, 0, 8)
            };
            content.Children.Add(titleText);

            var valueText = new TextBlock
            {
                Text = value,
                FontSize = 32,
                FontWeight = FontWeights.Bold,
                Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color))
            };
            content.Children.Add(valueText);

            card.Child = content;
            return card;
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Dashboard";
            LoadDashboard();
        }

        private void BtnPatientManagement_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Quản lý bệnh nhân";
            // TODO: Load patient management view
            MainContent.Content = new TextBlock
            {
                Text = "Patient Management functionality will be implemented",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void BtnCreateAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HeaderText.Text = "Tạo lịch hẹn";
                var systemUserRepo = App._serviceProvider.GetRequiredService<SystemUserRepository>();
                var doctorScheduleRepo = App._serviceProvider.GetRequiredService<DoctorScheduleRepository>();
                
                MainContent.Content = new CreateAppointmentView(
                    _appointmentService,
                    _patientService,
                    systemUserRepo,
                    doctorScheduleRepo,
                    this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Create Appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnTodayAppointments_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Lịch hẹn hôm nay";
            // TODO: Load today's appointments view
            MainContent.Content = new TextBlock
            {
                Text = "Today's Appointments functionality will be implemented",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void BtnAllAppointments_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Tất cả lịch hẹn";
            // TODO: Load all appointments view
            MainContent.Content = new TextBlock
            {
                Text = "All Appointments functionality will be implemented",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
} 
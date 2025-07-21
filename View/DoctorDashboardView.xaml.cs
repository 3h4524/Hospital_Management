using System;
using System.Windows;
using System.Windows.Controls;
using Service;
using Model;
using Repository;
using Microsoft.Extensions.DependencyInjection;

namespace View
{
    public partial class DoctorDashboardView : UserControl
    {
        private readonly AppointmentService _appointmentService;
        private readonly MedicalRecordService _medicalRecordService;
        private readonly SystemUser _currentUser;

        public DoctorDashboardView(AppointmentService appointmentService, MedicalRecordService medicalRecordService)
        {
            InitializeComponent();
            _appointmentService = appointmentService;
            _medicalRecordService = medicalRecordService;
            
            _currentUser = Application.Current.Properties["CurrentUser"] as SystemUser
                          ?? throw new InvalidOperationException("CurrentUser session is missing");
            
            HeaderText.Text = "Dashboard";
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            // Load dashboard content
            var dashboardContent = new StackPanel { Margin = new Thickness(32) };
            
            // Welcome message
            var welcomeText = new TextBlock
            {
                Text = $"Chào mừng Dr. {_currentUser.FullName}!",
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 32)
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

            // Completed appointments count
            var completedCard = CreateStatCard("Lịch hẹn đã hoàn thành", "0", "#66BB6A");
            Grid.SetColumn(completedCard, 1);
            statsGrid.Children.Add(completedCard);

            // Medical records count
            var recordsCard = CreateStatCard("Phiếu chỉ định", "0", "#FFA726");
            Grid.SetColumn(recordsCard, 2);
            statsGrid.Children.Add(recordsCard);

            dashboardContent.Children.Add(statsGrid);
            MainContent.Content = dashboardContent;
        }

        private Border CreateStatCard(string title, string value, string color)
        {
            var card = new Border
            {
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(16),
                Margin = new Thickness(8),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 12,
                    ShadowDepth = 4,
                    Color = System.Windows.Media.Colors.Gray
                }
            };

            var content = new StackPanel { Margin = new Thickness(24) };
            
            var titleText = new TextBlock
            {
                Text = title,
                FontSize = 16,
                Foreground = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(0, 0, 0, 12)
            };
            content.Children.Add(titleText);

            var valueText = new TextBlock
            {
                Text = value,
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color))
            };
            content.Children.Add(valueText);

            card.Child = content;
            return card;
        }

        public void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Dashboard";
            LoadDashboard();
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

        private void BtnCreateMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Tạo phiếu chỉ định";
            MainContent.Content = new CreateMedicalRecordView(
                _medicalRecordService,
                _appointmentService,
                App._serviceProvider.GetRequiredService<AppointmentRepository>(),
                App._serviceProvider.GetRequiredService<SpecializationRepository>(),
                this);
        }

        private void BtnMedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Phiếu chỉ định";
            MainContent.Content = new MedicalRecordsView(
                _medicalRecordService,
                _appointmentService,
                this);
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Lịch làm việc";
            // TODO: Load schedule view
            MainContent.Content = new TextBlock
            {
                Text = "Schedule functionality will be implemented",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Thông tin cá nhân";
            // TODO: Load profile view
            MainContent.Content = new TextBlock
            {
                Text = "Profile functionality will be implemented",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
} 
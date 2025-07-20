using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Service;
using Model;

namespace View
{
    public partial class PendingDayOffRequestsView : UserControl
    {
        private readonly DoctorScheduleService _doctorScheduleService;
        public ObservableCollection<DoctorSchedule> PendingRequests { get; set; } = new();
        public string Message { get; set; } = string.Empty;

        public PendingDayOffRequestsView(DoctorScheduleService doctorScheduleService)
        {
            InitializeComponent();
            _doctorScheduleService = doctorScheduleService;
            DataContext = this;
            Loaded += async (s, e) => await LoadPendingRequests();
        }

        private async Task LoadPendingRequests()
        {
            PendingRequests.Clear();
            Message = string.Empty;
            try
            {
                var requests = await _doctorScheduleService.GetAllPendingDayOffRequests();
                foreach (var req in requests)
                {
                    PendingRequests.Add(req);
                }
                if (PendingRequests.Count == 0)
                {
                    Message = "No pending requests.";
                }
            }
            catch (System.Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
            this.Dispatcher.Invoke(() =>
            {
                var msgProp = GetType().GetProperty("Message");
                msgProp?.SetValue(this, Message);
            });
        }

        private async void Approve_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DoctorSchedule schedule)
            {
                schedule.Status = "Off";
                await _doctorScheduleService.UpdateScheduleStatus(schedule);
                await LoadPendingRequests();
            }
        }

        private async void Reject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DoctorSchedule schedule)
            {
                schedule.Status = "Working";
                await _doctorScheduleService.UpdateScheduleStatus(schedule);
                await LoadPendingRequests();
            }
        }
    }
} 
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;
using Repository;

namespace ViewModel
{
    public class AttendanceTrackingViewModel : ViewModelBase
    {
        private readonly AttendanceRepository _attendanceRepository;

        public ObservableCollection<Timekeeping> AttendanceRecords { get; set; } = new();

        private Timekeeping? _selectedRecord;
        public Timekeeping? SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                _selectedRecord = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public AttendanceTrackingViewModel(AttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;

            AddCommand = new RelayCommand(async _ => await AddAttendance(), _ => SelectedRecord != null);
            UpdateCommand = new RelayCommand(async _ => await UpdateAttendance(), _ => SelectedRecord != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteAttendance(), _ => SelectedRecord != null);
            RefreshCommand = new RelayCommand(async _ => await LoadAttendance());

            Task.Run(async () => await LoadAttendance());
        }

        public async Task LoadAttendance()
        {
            var records = await _attendanceRepository.FindAll();
            AttendanceRecords.Clear();
            foreach (var record in records)
                AttendanceRecords.Add(record);
        }

        public async Task AddAttendance()
        {
            if (SelectedRecord == null) return;
            await _attendanceRepository.Add(SelectedRecord);
            await LoadAttendance();
        }

        public async Task UpdateAttendance()
        {
            if (SelectedRecord == null) return;
            await _attendanceRepository.Update(SelectedRecord);
            await LoadAttendance();
        }

        public async Task DeleteAttendance()
        {
            if (SelectedRecord == null) return;
            await _attendanceRepository.Delete(SelectedRecord);
            await LoadAttendance();
        }
    }
}
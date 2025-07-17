using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Hospital_Management;
using Model;
using System.Linq;

namespace View;

public class SalaryReportViewModel : INotifyPropertyChanged
{
    private readonly SalaryService _salaryService;
    private int _selectedMonth;
    private int _selectedYear;
    private bool _isLoading;
    private ObservableCollection<SalaryViewModelItem> _salaries = new();

    public SalaryReportViewModel(SalaryService salaryService)
    {
        _salaryService = salaryService;
        _selectedMonth = DateTime.Now.Month;
        _selectedYear = DateTime.Now.Year;
        LoadReportCommand = new RelayCommand(async _ => await LoadReportAsync(), _ => !IsLoading);
        ExportReportCommand = new RelayCommand(_ => ExportReport(), _ => Salaries.Count > 0);
    }

    public int SelectedMonth
    {
        get => _selectedMonth;
        set { _selectedMonth = value; OnPropertyChanged(); }
    }
    public int SelectedYear
    {
        get => _selectedYear;
        set { _selectedYear = value; OnPropertyChanged(); }
    }
    public ObservableCollection<SalaryViewModelItem> Salaries
    {
        get => _salaries;
        set { _salaries = value; OnPropertyChanged(); }
    }
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ObservableCollection<int> Months { get; } = new(Enumerable.Range(1, 12));
    public ObservableCollection<int> Years { get; } = new(Enumerable.Range(DateTime.Now.Year - 4, 5));

    public ICommand LoadReportCommand { get; }
    public ICommand ExportReportCommand { get; }

    public async Task LoadReportAsync()
    {
        try
        {
            IsLoading = true;
            var data = await _salaryService.GenerateSalaryReportAsync(SelectedMonth, SelectedYear);
            var mapped = data.Select(s => new SalaryViewModelItem
            {
                DoctorName = s.User?.FullName ?? "",
                BaseSalary = s.BaseSalary,
                WorkDays = s.WorkingDays ?? 0,
                Bonus = s.TotalReward ?? 0,
                Penalty = s.TotalPenalty ?? 0,
                TaxAmount = s.TaxAmount ?? 0,
                TotalSalary = s.FinalSalary ?? 0
            });
            Salaries = new ObservableCollection<SalaryViewModelItem>(mapped);
            if (Salaries.Count == 0)
            {
                System.Windows.MessageBox.Show($"Không có dữ liệu lương cho tháng {SelectedMonth}/{SelectedYear}.", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.ToString(), "Lỗi khi tải báo cáo");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ExportReport()
    {
        // TODO: Export to Excel/PDF (placeholder)
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class SalaryViewModelItem
{
    public string DoctorName { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public int WorkDays { get; set; }
    public decimal Bonus { get; set; }
    public decimal Penalty { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalSalary { get; set; }
}

// RelayCommand cho MVVM
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }
    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object? parameter) => _execute(parameter);
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
} 
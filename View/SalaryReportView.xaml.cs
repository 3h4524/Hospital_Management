using DataAccess;
using Repository;
using Hospital_Management;
using System.Windows.Controls;

namespace View;

public partial class SalaryReportView : UserControl
{
    public SalaryReportView()
    {
        InitializeComponent();
        var context = new HospitalManagementContext();
        var salaryRepo = new SalaryRepository(context);
        var salaryService = new SalaryService(salaryRepo, context);
        DataContext = new SalaryReportViewModel(salaryService);
    }
} 
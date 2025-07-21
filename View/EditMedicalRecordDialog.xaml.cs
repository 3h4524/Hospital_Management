using System.Windows;

namespace View
{
    public partial class EditMedicalRecordDialog : Window
    {
        private readonly MedicalRecordViewModel _record;

        public string Diagnosis => DiagnosisTextBox.Text;
        public string Notes => NotesTextBox.Text;

        public EditMedicalRecordDialog(MedicalRecordViewModel record)
        {
            InitializeComponent();
            _record = record;
            LoadData();
        }

        private void LoadData()
        {
            PatientInfoText.Text = $"Bệnh nhân: {_record.PatientName} (ID: {_record.PatientId})\n" +
                                  $"Lịch hẹn: {_record.AppointmentId}\n" +
                                  $"Chuyên khoa: {_record.SpecializationName}";
            
            DiagnosisTextBox.Text = _record.Diagnosis;
            NotesTextBox.Text = _record.Notes;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 
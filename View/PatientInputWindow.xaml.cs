using System;
using System.Windows;
using System.Windows.Controls;
using Model;

namespace View
{
    public partial class PatientInputWindow : Window
    {
        public Patient? Patient { get; private set; }

        public PatientInputWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ và tên", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    FullNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    PhoneNumberTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập email", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    EmailTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    AddressTextBox.Focus();
                    return;
                }

                // Create patient object
                Patient = new Patient
                {
                    FullName = FullNameTextBox.Text.Trim(),
                    PhoneNumber = PhoneNumberTextBox.Text.Trim(),
                    Email = EmailTextBox.Text.Trim(),
                    Gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Tag.ToString() ?? "MALE",
                    Address = AddressTextBox.Text.Trim(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 
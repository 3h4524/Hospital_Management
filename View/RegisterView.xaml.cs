using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Service;
using Common.Enum;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Printing;

namespace View
{
    public partial class RegisterView : UserControl, INotifyPropertyChanged
    {
        private readonly AuthenticationService _authenticationService;
        private string _fullName;
        private UserRole _role;
        private string _phoneNumber;
        private string _email;
        private string _password;
        private string _confirmPassword;

        public event PropertyChangedEventHandler? PropertyChanged;


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                if (SetPropertyChanged(ref _fullName, value))
                {
                    if (string.IsNullOrWhiteSpace(FullName))
                    {
                        FullNameError.Text = "Full name cannot be empty";
                    } else
                    {
                        FullNameError.Text = "";
                    }
                }
            }
        }

        public UserRole Role
        {
            get => _role;
            set
            {
                if (SetPropertyChanged(ref _role, value))
                {
                    if(!Enum.IsDefined(typeof(UserRole), value))
                    {
                        RoleError.Text = "Role must be selected";
                    } else
                    {
                        RoleError.Text = "";
                    }
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (SetPropertyChanged(ref _phoneNumber, value))
                {
                    PhoneNumberError.Text = ValidatePhone(value);
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetPropertyChanged(ref _email, value))
                {
                    EmailError.Text = ValidateEmail(value);
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetPropertyChanged(ref _password, value))
                {
                    PasswordError.Text = ValidatePassword(value);
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetPropertyChanged(ref _confirmPassword, value))
                {
                    if(!Equals(_confirmPassword, _password))
                    {
                        ConfirmPasswordError.Text = "Passwords do not match";
                    } else
                    {
                        ConfirmPasswordError.Text = "";
                    }
                }
            }
        }

    

        public RegisterView(AuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            DataContext = this;
            RoleBox.ItemsSource = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Where(role => role != UserRole.Admin);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                Password = passwordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                ConfirmPassword = passwordBox.Password;
            }
        }

        private void OnLoginClick(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).Content = new LoginView(_authenticationService);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageTextBox.Text = "";


            if (HasErrors())
            {
                MessageTextBox.Text = "Please fix the errors before registering";
                return;
            }

            try
            {
                string roleString = Role.ToString();
                bool success = await _authenticationService.Register(Email, Password, FullName, roleString, PhoneNumber);
                if (!success)
                {
                    MessageTextBox.Text = "Registration failed!";
                    MessageBox.Show("Registration failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageTextBox.Text = "Registration successful!";
                    MessageBox.Show("Registration successful!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window.GetWindow(this).Content = new LoginView(_authenticationService);
                }
            }
            catch (Exception ex)
            {
                MessageTextBox.Text = $"Registration failed: {ex.Message}";
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email cannot be empty";
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Email format is invalid";
            return "";
        }

        private string ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "Phone cannot be empty";
            if (!Regex.IsMatch(phone, @"^\d{9,11}$"))
                return "Phone must be 9–11 digits";
            return "";
        }

        private string ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return "Password cannot be empty";
            }
            else if (password.Length < 6)
            {
                return "Password must be at least 6 characters";
            }
            return "";
        }

        private bool HasErrors()
        {
            FullNameError.Text = string.IsNullOrWhiteSpace(FullName) ? "Full name cannot be empty" : "";
            EmailError.Text = ValidateEmail(Email);
            PhoneNumberError.Text = ValidatePhone(PhoneNumber);
            PasswordError.Text = ValidatePassword(Password);
            ConfirmPasswordError.Text = Password == ConfirmPassword ? "" : "Passwords do not match";
            RoleError.Text = !Enum.IsDefined(typeof(UserRole), Role) ? "Role must be selected" : "";

            return
                !string.IsNullOrEmpty(FullNameError.Text) ||
                !string.IsNullOrEmpty(EmailError.Text) ||
                !string.IsNullOrEmpty(PhoneNumberError.Text) ||
                !string.IsNullOrEmpty(PasswordError.Text) ||
                !string.IsNullOrEmpty(ConfirmPasswordError.Text) ||
                !string.IsNullOrEmpty(RoleError.Text);
        }

        private void RoleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
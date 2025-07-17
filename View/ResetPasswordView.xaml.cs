using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Service;

namespace View
{
    /// <summary>
    /// Interaction logic for ResetPassowrdView.xaml
    /// </summary>
    public partial class ResetPasswordView : UserControl, INotifyPropertyChanged
    {
        private AuthenticationService _authenticationService;
        private string _email;
        private string _resetCode;
        private string _password;
        private string _confirmPassword;
        private string _passwordError;
        private string _confirmPasswordError;
        private string _message;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private bool SetPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        public ResetPasswordView(AuthenticationService authenticationService, string email)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _email = email;
            DataContext = this;
        }

        public string ResetCode
        {
            get => _resetCode;
            set
            {
                SetPropertyChanged(ref _resetCode, value);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if(SetPropertyChanged(ref _password, value))
                {
                    PasswordError = ValidatePassword(value);
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
                    ConfirmPasswordError = ValidateConfirmPassword(value);
                }
            }
        }


        public string PasswordError
        {
            get => _passwordError;
            set => SetPropertyChanged(ref _passwordError, value);
        }

        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set => SetPropertyChanged(ref _confirmPasswordError, value);
        }

        public string Message
        {
            get => _message;
            set => SetPropertyChanged(ref _message, value);
        }

        private void PwdChanged(object sender, RoutedEventArgs e)
        {
           if (sender is PasswordBox pb)
            {
                Password = pb.Password;
            }
        }

        private void ConfPwdChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox cb)
            {
                ConfirmPassword = cb.Password;
            }
        }

        private void GoLogin(object sender, MouseButtonEventArgs e)
        {
            Window mainWindow = Window.GetWindow(this);
            mainWindow.Content = new LoginView(_authenticationService);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Email: {_email}");
            if (hasErrors())
            {
                Message = "Please fix errors before resetting your password";
                return;
            }

            bool success = await _authenticationService.ResetPassword(_email, _resetCode, _password);
            if(success)
            {
                Message = "Password has been reseted successfully";
            } else
            {
                Message = "Reset code is incorrect";
            }
        }

        private bool hasErrors()
        {
            PasswordError = ValidatePassword(_password);
            ConfirmPasswordError = ValidateConfirmPassword(_confirmPassword);

            return !string.IsNullOrWhiteSpace(PasswordError) || !string.IsNullOrEmpty(ConfirmPasswordError);
        }
        private string ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "Password cannot be empty";
            if (password.Length < 6)
                return "Password must be at least 6 characters";
            return "";
        }

        private string ValidateConfirmPassword(string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(confirmPassword))
                return "Confirm password cannot be empty";
            if (confirmPassword != _password)
                return "Passwords do not match";
            return "";
        }
    }
}

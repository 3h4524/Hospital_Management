using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Service;

namespace ViewModel
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly INavigateService _navigationService;
        private string _email;
        private string _password;
        private string _message;

        public RegisterViewModel(AuthenticationService authenticationService, INavigateService navigateService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigateService;
            RegisterCommand = new RelayCommand(Register, CanRegister);
            GoLoginCommand = new RelayCommand(GoToLogin);
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand GoLoginCommand { get; }

        private bool CanRegister(object parameter)
        {
            return !string.IsNullOrEmpty(Email) &&
                   Email.Contains("@") && Email.Contains(".") &&
                   !string.IsNullOrEmpty(Password);
        }

        private async void Register(object parameter)
        {
            try
            {
                bool success = await _authenticationService.Register(Email, Password);
                if (success)
                {
                    Message = "Registration successful! Please log in.";
                    _navigationService.NavigateTo<LoginViewModel>();
                }
                else
                {
                    Message = "Email already exists.";
                }

            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
        }

        private void GoToLogin(object paremeter)
        {
            _navigationService.NavigateTo<LoginViewModel>();
        }

    }
}

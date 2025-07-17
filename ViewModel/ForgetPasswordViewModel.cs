using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Service;

namespace ViewModel
{
    public class ForgetPasswordViewModel : ViewModelBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly INavigateService _navigationService;
        private string _email = "";
        private string _message = "";
        public ICommand SendResetCodeCommand;
        public ICommand GoLoginCommand;

        public ForgetPasswordViewModel(AuthenticationService authenticationService, INavigateService navigateService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigateService;
            SendResetCodeCommand = new RelayCommand(ForgetPassword, CanSendResetCode);
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

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public void GoToLogin(object? parameter)
        {
            _navigationService.NavigateTo<LoginViewModel>();
        }

        public async void ForgetPassword(object? parameter)
        {
            try
            {
                bool success = await _authenticationService.ForgetPassword(Email);
                if (success)
                {
                    Message = "Reset Code sent to your email.";
                    _navigationService.NavigateTo<ResetPasswordViewModel>(Email);
                }
                else
                {
                    Message = "Email not found";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
        }


        private bool CanSendResetCode(object? parameter)
        {
            return !string.IsNullOrEmpty(Email) &&
                    Email.Contains("@") && Email.Contains(".");
        }
    }
}

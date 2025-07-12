using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Service;

namespace ViewModel
{
    public class ResetPasswordViewModel : ViewModelBase, INavigateWithParameter
    {
        private readonly AuthenticationService _authenticationService;
        private readonly INavigateService _navigationService;
        private string _email;
        private string _resetCode;
        private string _newPassword;
        private string _message;
        public ICommand ResetPasswordCommand;
        public ICommand GoLoginCommand;

        public ResetPasswordViewModel(AuthenticationService authenticationService, INavigateService navigateService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigateService;
            ResetPasswordCommand = new RelayCommand(ResetPasssword);
            GoLoginCommand = new RelayCommand(GoToLogin);
        }

        public String Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string ResetCode
        {
            get { return _resetCode; }
            set
            {
                _resetCode = value; OnPropertyChanged();
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value; OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value; OnPropertyChanged();
            }
        }


        public async void ResetPasssword(object parameter)
        {
            try
            {
                bool success = await _authenticationService.ResetPassword(Email, ResetCode, NewPassword);
                if (success)
                {
                    Message = "Reset Password successfull! Please Login.";
                    _navigationService.NavigateTo<LoginViewModel>();
                }
                else
                {
                    Message = "Invalid email or reset code";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
        }

        public void GoToLogin(object parameter)
        {
            _navigationService.NavigateTo<LoginViewModel>();
        }



        public void OnNavigateTo(object parameter)
        {
            if (parameter is string email)
            {
                Email = email;
            }
        }
    }
}

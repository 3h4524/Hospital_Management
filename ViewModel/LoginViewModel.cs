using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Service;

namespace ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthenticationService _authService;
        private readonly INavigateService _navigateService;
        private string _email = "";
        private string _password = "";
        private string _message = "";
        private bool _isLoggingIn;

        public LoginViewModel(AuthenticationService authenticationService, INavigateService navigateService)
        {
            _authService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _navigateService = navigateService ?? throw new ArgumentNullException(nameof(navigateService));
            LoginCommand = new RelayCommand(async parameter => await Login(parameter), CanLogin);
            GoRegisterCommand = new RelayCommand(GoToRegister, _ => !_isLoggingIn);
            GoForgotCommand = new RelayCommand(GoToForgetPassword, _ => !_isLoggingIn);
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public bool IsLoggingIn
        {
            get => _isLoggingIn;
            private set
            {
                if (SetProperty(ref _isLoggingIn, value))
                {
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)GoRegisterCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)GoForgotCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand GoRegisterCommand { get; }
        public ICommand GoForgotCommand { get; }

        private async Task Login(object parameter)
        {
            if (IsLoggingIn) return; 
            IsLoggingIn = true;
            Message = string.Empty;

            try
            {
                var user = await _authService.Login(Email, Password);
                if (user != null)
                {
                    Message = "Login successful!";
                    // Optionally navigate to a dashboard view
                    // _navigateService.NavigateTo<DashboardViewModel>();
                }
                else
                {
                    Message = "Invalid email or password.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoggingIn = false;
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsLoggingIn &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   Email.Contains("@") && Email.Contains(".") &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        private void GoToRegister(object parameter)
        {
            _navigateService.NavigateTo<RegisterViewModel>();
        }

        private void GoToForgetPassword(object parameter)
        {
            _navigateService.NavigateTo<ForgetPasswordViewModel>();
        }
    }
}
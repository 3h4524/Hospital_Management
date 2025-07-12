using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Service;

namespace ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthenticationService _authService;
        private readonly MainViewModel _main;

        private readonly INavigateService _navigateService;  

        
        public ICommand LoginCommand { get; }
        public ICommand GoRegisterCommand { get; }
        public ICommand GoForgotCommand { get; }

        private string _email = "";
        private string _password = "";
        private string _message = "";

        public LoginViewModel(AuthenticationService authenticationService, INavigateService navigateService)
        {
            _authService = authenticationService;
            _navigateService = navigateService;
            LoginCommand = new RelayCommand(Login, CanLogin);
            GoRegisterCommand = new RelayCommand(GoToRegister);
            GoForgotCommand = new RelayCommand(GoToForgetPassword);

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


        private async void Login(object parameter)
        {
            try
            {
                var user = await _authService.Login(Email, Password);
                if(user != null)
                {
                    Message = "Login successful!";
                } else
                {
                    Message = "Invalid email or password";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
        }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Email) &&
                    Email.Contains("@") && Email.Contains(".") &&
                    !string.IsNullOrEmpty(Password);
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

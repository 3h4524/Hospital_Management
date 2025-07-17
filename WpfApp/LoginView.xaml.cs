using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModel;

namespace View
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PwdChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void OnForgotPasswordClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.GoForgotCommand.Execute(null);
            }
        }

        private void OnRegisterClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.GoRegisterCommand.Execute(null);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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
using Service;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for ResetPassowrdView.xaml
    /// </summary>
    public partial class ResetPasswordView : UserControl
    {
        private AuthenticationService _authenticationService;
        public ResetPasswordView(AuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;

        }

        private void PwdChanged(object sender, RoutedEventArgs e)
        {
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;        
        private readonly IServiceProvider _serviceProvider;

        public MainViewModel(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            NavigateTo<LoginViewModel>();
        }

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { 
                _currentViewModel = value; 
                OnPropertyChanged();
            }
        }

        public void NavigateTo<TViewModel>(object parameter = null) where TViewModel : ViewModelBase
        {
            var viewModelType = typeof(TViewModel);
            var viewModel = (TViewModel)_serviceProvider.GetService(viewModelType); // Ép kiểu về TViewModel

            if (viewModel is INavigateWithParameter vmWithParam)
            {
                vmWithParam.OnNavigateTo(parameter);
            }

            CurrentViewModel = viewModel;
        }
    }
}

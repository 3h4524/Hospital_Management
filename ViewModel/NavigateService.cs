//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ViewModel
//{
//    public class NavigateService : INavigateService
//    {
//        private MainViewModel _mainViewModel { get; set; }

//        public void SetNavigateService(MainViewModel mainViewModel)
//        {
//            _mainViewModel = mainViewModel;
//        }

//        public void NavigateTo<TViewModel>(object parameter = null) where TViewModel : ViewModelBase 
//        {
//            _mainViewModel.NavigateTo<TViewModel>(parameter);
//        }

//    }
//}

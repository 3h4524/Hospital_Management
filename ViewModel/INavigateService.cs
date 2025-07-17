using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public interface INavigateService
    {
        void NavigateTo<TViewModel>(object parameter = null) where TViewModel : ViewModelBase;
    }
}

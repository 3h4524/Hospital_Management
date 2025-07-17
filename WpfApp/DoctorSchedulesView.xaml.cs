using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Interaction logic for DoctorSchedulesView.xaml
    /// </summary>
    public partial class DoctorSchedulesView : UserControl
    {
        public DoctorSchedulesView()
        {
            InitializeComponent();
        }
    }

    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int month)
            {
                return month - 1; // Convert to 0-based index for ComboBox
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return index + 1; // Convert back to 1-based month number
            }
            return 1;
        }
    }

    public class MonthNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int month && month >= 1 && month <= 12)
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

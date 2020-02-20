using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;


namespace BlockBusters
{
    class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }


        //it gets called when data flows from UI to ViewModel
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //do nothing 
            return Binding.DoNothing;

        }
    }
}

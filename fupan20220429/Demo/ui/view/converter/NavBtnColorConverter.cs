using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Demo.ui.view.converter
{
    public class NavBtnColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse((string)parameter) == (int)value)
            {
                
                return new SolidColorBrush(Color.FromArgb(255, 0x0, 0xAE, 0xF0));
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, 0xB5, 0xB3, 0xB0));
            }

        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

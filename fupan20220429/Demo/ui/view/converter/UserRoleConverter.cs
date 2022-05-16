using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Demo.ui.model;
using System.Collections.Generic;

namespace Demo.ui.view.converter
{
    public class UserRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1) {

                return "Administrator";
            }
            else if ((int)value == 2)
            {

                return "Config Engineer";
            }
            else if ((int)value == 3)
            {

                return "Recipe Engineer";
            }
            else if ((int)value == 4)
            {

                return "Maintanence Engineer";
            }
            return "";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

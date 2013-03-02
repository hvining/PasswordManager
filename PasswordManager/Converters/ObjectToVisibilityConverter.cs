using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PasswordManager.Converters
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public Boolean IsNullVisible { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                if (IsNullVisible)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
            {
                if (IsNullVisible)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

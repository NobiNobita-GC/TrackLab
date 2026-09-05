using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI.Converts
{
    public class StringToIconConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? iconName = value?.ToString()?.Replace(" ", "");
            if (string.IsNullOrEmpty(iconName))
            {
                return null;
            }
            return Application.Current.Resources[$"{iconName}Icon"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

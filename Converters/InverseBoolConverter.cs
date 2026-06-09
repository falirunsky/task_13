using System.Globalization;
using System.Windows.Data;

namespace task_12.Converters
{
    /// <summary>
    /// Инвертирует bool: True -> False, False -> True.
    /// Используется для IsEnabled="{Binding IsBusy, Converter={StaticResource InverseBoolConverter}}".
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b && !b;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b && !b;
    }
}

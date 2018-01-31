using System;
using System.Globalization;
using System.Windows.Data;

namespace Digimezzo.WPFControls.Converters
{
    public class IntRatioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = (double) value * System.Convert.ToInt32(parameter);
            return ((int) num).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = System.Convert.ToDouble(value);
            var para = System.Convert.ToInt32(parameter);
            return num / para;
        }
    }
}
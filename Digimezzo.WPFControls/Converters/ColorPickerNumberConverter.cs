using System;
using System.Globalization;
using System.Windows.Data;

namespace Digimezzo.WPFControls.Converters
{
    public class ColorPickerNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = (double) value * System.Convert.ToInt32(parameter);
            return ((int)num).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double.TryParse((string)value, out var num);
            var para = System.Convert.ToInt32(parameter);
            if (0 <= num && num <= para)
                return num / para;
            else
            {
                return 0;
            }
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Digimezzo.WPFControls.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (!(value is Color))
            {
                throw new InvalidOperationException("Unsupported type [" + value.GetType().Name + "], ColorToSolidColorBrushConverter.Convert()");
            }

            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
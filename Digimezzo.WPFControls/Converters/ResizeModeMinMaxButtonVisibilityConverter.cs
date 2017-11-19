using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Digimezzo.WPFControls.Converters
{
    /// <summary>
    /// ResizeModeMinMaxButtonVisibilityConverter is based on code from MahApps.Metro: https://github.com/MahApps/MahApps.Metro
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    public class ResizeModeMinMaxButtonVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2 && parameter is string)
            {
                bool windowPropValue = System.Convert.ToBoolean(values[0]);
                ResizeMode windowResizeMode = (ResizeMode)values[1];
                string whichButton = System.Convert.ToString(parameter);

                switch (windowResizeMode)
                {
                    case ResizeMode.NoResize:
                        return Visibility.Collapsed;
                    case ResizeMode.CanMinimize:
                        if (whichButton == "MIN")
                        {
                            return (windowPropValue ? Visibility.Visible : Visibility.Collapsed);
                        }
                        return Visibility.Collapsed;
                    case ResizeMode.CanResize:
                        return (windowPropValue ? Visibility.Visible : Visibility.Collapsed);
                    case ResizeMode.CanResizeWithGrip:
                        return (windowPropValue ? Visibility.Visible : Visibility.Collapsed);
                    default:
                        return (windowPropValue ? Visibility.Visible : Visibility.Collapsed);
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
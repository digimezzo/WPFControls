using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Digimezzo.WPFControls.Helpers
{
    public static class VisualTree
    {
        public static T FindAncestor<T>(DependencyObject d) where T : class
        {
            if (d is Visual || d is Visual3D)
            {
                DependencyObject item = VisualTreeHelper.GetParent(d);

                while (item != null)
                {
                    T itemAsT = item as T;
                    if (itemAsT != null)
                    {
                        return itemAsT;
                    }
                    item = VisualTreeHelper.GetParent(item);
                }
            }

            return null;
        }
    }
}

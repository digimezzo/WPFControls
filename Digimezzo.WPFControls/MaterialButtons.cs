using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class MaterialRaisedButton : Button
    {
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialRaisedButton), new PropertyMetadata(Brushes.Red));

        static MaterialRaisedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialRaisedButton), new FrameworkPropertyMetadata(typeof(MaterialRaisedButton)));
        }
    }
}

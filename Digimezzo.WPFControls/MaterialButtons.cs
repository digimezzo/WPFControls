using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class MaterialButtonBase : Button
    {
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialButtonBase), new PropertyMetadata(Brushes.Red));

        static MaterialButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialButtonBase), new FrameworkPropertyMetadata(typeof(MaterialButtonBase)));
        }
    }

    public class MaterialRaisedButton : MaterialButtonBase
    {
        static MaterialRaisedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialRaisedButton), new FrameworkPropertyMetadata(typeof(MaterialRaisedButton)));
        }
    }

    public class MaterialFlatButton : MaterialButtonBase
    {
        static MaterialFlatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialFlatButton), new FrameworkPropertyMetadata(typeof(MaterialFlatButton)));
        }
    }

    public class MaterialFloatingActionButton : MaterialButtonBase
    {
        static MaterialFloatingActionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialFloatingActionButton), new FrameworkPropertyMetadata(typeof(MaterialFloatingActionButton)));
        }
    }

    public class MaterialMiniFloatingActionButton : MaterialButtonBase
    {
        static MaterialMiniFloatingActionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialMiniFloatingActionButton), new FrameworkPropertyMetadata(typeof(MaterialMiniFloatingActionButton)));
        }
    }
}

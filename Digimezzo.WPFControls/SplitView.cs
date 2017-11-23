using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public class SplitView : Control
    {
        private ContentPresenter pane;
        private ContentPresenter content;

        public object Pane
        {
            get { return (object)GetValue(PaneProperty); }
            set { SetValue(PaneProperty, value); }
        }

        public static readonly DependencyProperty PaneProperty =
           DependencyProperty.Register(nameof(Pane), typeof(object), typeof(SplitView), new PropertyMetadata(null));

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
          DependencyProperty.Register(nameof(Content), typeof(object), typeof(SplitView), new PropertyMetadata(null));

        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register(nameof(IsPaneOpen), typeof(bool), typeof(SplitView), new PropertyMetadata(false, OnIsPaneOpenChanged));
       

        public double OpenPaneLength
        {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }

        public static readonly DependencyProperty OpenPaneLengthProperty =
          DependencyProperty.Register(nameof(OpenPaneLength), typeof(double), typeof(SplitView), new PropertyMetadata(200.0));

        public event EventHandler PaneClosed = delegate { };
   
        private static void OnIsPaneOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SplitView splitView = o as SplitView;
            bool isPaneOpen = (bool)e.NewValue;

            if (isPaneOpen)
            {
                splitView.OpenPane();
            }
            else
            {
                splitView.ClosePane();
            }
        }
    
        static SplitView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitView), new FrameworkPropertyMetadata(typeof(SplitView)));
        }
      
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.pane = (ContentPresenter)GetTemplateChild("PART_Pane");
            this.content = (ContentPresenter)GetTemplateChild("PART_Content");

            if (this.pane != null)
            {
                this.pane.Margin = new Thickness(-this.OpenPaneLength - 1, 0, 0, 0);
            }

            if (this.content != null)
            {
                this.content.MouseUp += Content_MouseUp;
            }
        }

        private void Content_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.IsPaneOpen = false;
        }

        private void OpenPane()
        {
            if (this.pane != null)
            {
                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(-this.OpenPaneLength - 1, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = TimeSpan.FromMilliseconds(150)
                };

                this.pane.BeginAnimation(ContentPresenter.MarginProperty, marginAnimation);
            }
        }
        private void ClosePane()
        {
            if (this.pane != null)
            {
                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(0, 0, 0, 0),
                    To = new Thickness(-this.OpenPaneLength - 1, 0, 0, 0),
                    Duration = TimeSpan.FromMilliseconds(150)
                };

                this.pane.BeginAnimation(ContentPresenter.MarginProperty, marginAnimation);
            }

            this.PaneClosed(this, new EventArgs());
        }
    }
}

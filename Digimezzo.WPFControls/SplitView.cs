using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public class SplitView : Control
    {
        #region Variables
        private ContentPresenter pane;
        private ContentPresenter content;
        #endregion

        #region Properties
        public object Pane
        {
            get { return (object)GetValue(PaneProperty); }
            set { SetValue(PaneProperty, value); }
        }

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        public double OpenPaneLength
        {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(SplitView), new PropertyMetadata(false, OnIsPaneOpenChanged));
        public static readonly DependencyProperty PaneProperty = DependencyProperty.Register("Pane", typeof(object), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata(200.0));
        #endregion

        #region Event handlers
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
        #endregion

        #region Construction
        static SplitView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitView), new FrameworkPropertyMetadata(typeof(SplitView)));
        }
        #endregion

        #region Overrides
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
        #endregion

        #region Private
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
        }
        #endregion
    }
}

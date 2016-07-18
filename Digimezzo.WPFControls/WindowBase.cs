using Digimezzo.WPFControls.Native;
using RaphaelGodart.Controls.Native;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public abstract class WindowBase : Window
    {
        #region Variables
        protected Button minimizeButton;
        protected Button maximizeButton;
        protected Button closeButton;
        protected Border windowBorder;
        protected ContentPresenter windowCommands;
        protected WindowState previousWindowState; // Holds the previous WindowState
        #endregion

        #region Properties
        public double TitleBarHeight
        {
            get { return Convert.ToDouble(GetValue(TitleBarHeightProperty)); }
            set { SetValue(TitleBarHeightProperty, value); }
        }

        public bool IsMovable
        {
            get { return Convert.ToBoolean(GetValue(IsMovableProperty)); }
            set { SetValue(IsMovableProperty, value); }
        }

        public bool IsOverlayVisible
        {
            get { return Convert.ToBoolean(GetValue(IsOverlayVisibleProperty)); }
            set { SetValue(IsOverlayVisibleProperty, value); }
        }

        public Brush OverlayBackground
        {
            get { return (Brush)GetValue(OverlayBackgroundProperty); }
            set { SetValue(OverlayBackgroundProperty, value); }
        }

        public object WindowCommands
        {
            get { return (object)GetValue(WindowCommandsProperty); }
            set { SetValue(WindowCommandsProperty, value); }
        }

        public bool ShowMinButton
        {
            get { return Convert.ToBoolean(GetValue(ShowMinButtonProperty)); }
            set { SetValue(ShowMinButtonProperty, value); }
        }

        public bool ShowMaxRestoreButton
        {
            get { return Convert.ToBoolean(GetValue(ShowMaxRestoreButtonProperty)); }
            set { SetValue(ShowMaxRestoreButtonProperty, value); }
        }

        public bool ShowCloseButton
        {
            get { return Convert.ToBoolean(GetValue(ShowCloseButtonProperty)); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        public string MinimizeToolTip
        {
            get { return Convert.ToString(GetValue(MinimizeToolTipProperty)); }
            set { SetValue(MinimizeToolTipProperty, value); }
        }

        public string MaximizeToolTip
        {
            get { return Convert.ToString(GetValue(MaximizeToolTipProperty)); }
            set { SetValue(MaximizeToolTipProperty, value); }
        }

        public string RestoreToolTip
        {
            get { return Convert.ToString(GetValue(RestoreToolTipProperty)); }
            set { SetValue(RestoreToolTipProperty, value); }
        }

        public string CloseToolTip
        {
            get { return Convert.ToString(GetValue(CloseToolTipProperty)); }
            set { SetValue(CloseToolTipProperty, value); }
        }

        public bool ShowWindowControls
        {
            get { return Convert.ToBoolean(GetValue(ShowWindowControlsProperty)); }
            set { SetValue(ShowWindowControlsProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(WindowBase), new PropertyMetadata(24.0));
        public static readonly DependencyProperty IsMovableProperty = DependencyProperty.Register("IsMovable", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty IsOverlayVisibleProperty = DependencyProperty.Register("IsOverlayVisible", typeof(bool), typeof(WindowBase), new PropertyMetadata(false));
        public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register("OverlayBackground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(null));
        public static readonly DependencyProperty ShowMinButtonProperty = DependencyProperty.Register("ShowMinButton", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowMaxRestoreButtonProperty = DependencyProperty.Register("ShowMaxRestoreButton", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty MinimizeToolTipProperty = DependencyProperty.Register("MinimizeToolTip", typeof(string), typeof(WindowBase), new PropertyMetadata("Minimize", MinimizeToolTipChangedCallback));
        public static readonly DependencyProperty MaximizeToolTipProperty = DependencyProperty.Register("MaximizeToolTip", typeof(string), typeof(WindowBase), new PropertyMetadata("Maximize", MaximizeToolTipChangedCallback));
        public static readonly DependencyProperty RestoreToolTipProperty = DependencyProperty.Register("RestoreToolTip", typeof(string), typeof(WindowBase), new PropertyMetadata("Restore", RestoreToolTipChangedCallback));
        public static readonly DependencyProperty CloseToolTipProperty = DependencyProperty.Register("CloseToolTip", typeof(string), typeof(WindowBase), new PropertyMetadata("Close", CloseToolTipChangedCallback));
        public static readonly DependencyProperty WindowCommandsProperty = DependencyProperty.Register("WindowCommands", typeof(object), typeof(WindowBase), new PropertyMetadata(null));
        public static readonly DependencyProperty ShowWindowControlsProperty = DependencyProperty.Register("ShowWindowControls", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.minimizeButton = (Button)GetTemplateChild("PART_Min");
            this.maximizeButton = (Button)GetTemplateChild("PART_Max");
            this.closeButton = (Button)GetTemplateChild("PART_Close");
            this.windowBorder = (Border)GetTemplateChild("PART_Border");
            this.windowCommands = (ContentPresenter)GetTemplateChild("PART_WindowCommands");

            if (this.minimizeButton != null)
            {
                this.minimizeButton.Click -= MinimizeButton_Click;
                this.minimizeButton.Click += MinimizeButton_Click;
            }

            if (this.maximizeButton != null)
            {
                this.maximizeButton.Click -= MaximizeButton_Click;
                this.maximizeButton.Click += MaximizeButton_Click;
            }

            if (this.closeButton != null)
            {
                this.closeButton.Click -= CloseButton_Click;
                this.closeButton.Click += CloseButton_Click;
            }

            this.MouseLeftButtonDown -= WindowBase_MouseLeftButtonDown;
            this.MouseLeftButtonDown += WindowBase_MouseLeftButtonDown;

            this.SizeChanged -= WindowBase_SizeChanged;
            this.SizeChanged += WindowBase_SizeChanged;
        }
        #endregion

        #region Private
        private void WindowBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = Mouse.GetPosition(this);

            // Only do stuff if the mouse cursor is within the title bar area
            if (this.IsMovable & this.TitleBarHeight > 0 && point.Y < this.TitleBarHeight)
            {
                // Max/Restore on TitleBar double-click
                if (e.ClickCount == 2 && (this.ResizeMode == ResizeMode.CanResizeWithGrip || this.ResizeMode == ResizeMode.CanResize))
                {
                    this.MaximizeButton_Click(sender, e);
                }
                else
                {
                    // This handles DragMove and restoring from snap in Windows
                    System.Windows.Point wpfPoint = this.PointToScreen(point);
                    int x = Convert.ToInt16(wpfPoint.X);
                    int y = Convert.ToInt16(wpfPoint.Y);
                    int lParam = Convert.ToInt32(Convert.ToInt32(x)) | (y << 16);

                    IntPtr windowHandle = new WindowInteropHelper(this).Handle;
                    NativeMethods.SendMessage(windowHandle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, lParam);
                }
            }
        }

        private static void MinimizeToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowBase win = (WindowBase)d;
            win.MinimizeToolTipChanged(win, new EventArgs());
        }

        private static void MaximizeToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowBase win = (WindowBase)d;
            win.MaximizeToolTipChanged(win, new EventArgs());
        }

        private static void RestoreToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowBase win = (WindowBase)d;
            win.RestoreToolTipChanged(win, new EventArgs());
        }

        private static void CloseToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowBase win = (WindowBase)d;
            win.CloseToolTipChanged(win, new EventArgs());
        }

        #endregion

        #region Event Handlers
        protected void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        protected void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResizeMode == ResizeMode.CanResizeWithGrip || this.ResizeMode == ResizeMode.CanResize)
            {
                if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    SystemCommands.RestoreWindow(this);
                }
                else
                {
                    SystemCommands.MaximizeWindow(this);
                }
            }
        }

        protected void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }


        protected virtual void WindowBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal && this.previousWindowState == System.Windows.WindowState.Maximized)
            {
                this.OnRestored();
            }

            this.previousWindowState = this.WindowState;
        }
        #endregion

        #region Events
        public event EventHandler MinimizeToolTipChanged = delegate { };
        public event EventHandler MaximizeToolTipChanged = delegate { };
        public event EventHandler RestoreToolTipChanged = delegate { };
        public event EventHandler CloseToolTipChanged = delegate { };
        public event EventHandler Restored = delegate { };

        public void OnRestored()
        {
                this.Restored(this, new EventArgs());
        }
        #endregion
    }
}
using Digimezzo.WPFControls.Native;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace Digimezzo.WPFControls.Base
{
    public abstract class BorderlessWindowBase : Window
    {
        #region Variables
        protected Button minimizeButton;
        protected Button maximizeButton;
        protected Button closeButton;
        protected Border windowBorder;
        protected ContentPresenter windowCommands;
        protected WindowState previousWindowState; // Holds the previous WindowState
        protected Thickness previousBorderThickness; // Holds the previous BorderThickness
        private WindowChrome windowChrome;
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
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(BorderlessWindowBase), new PropertyMetadata(24.0));
        public static readonly DependencyProperty IsMovableProperty = DependencyProperty.Register("IsMovable", typeof(bool), typeof(BorderlessWindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty IsOverlayVisibleProperty = DependencyProperty.Register("IsOverlayVisible", typeof(bool), typeof(BorderlessWindowBase), new PropertyMetadata(false));
        public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register("OverlayBackground", typeof(Brush), typeof(BorderlessWindowBase), new PropertyMetadata(null));
        public static readonly DependencyProperty ShowMinButtonProperty = DependencyProperty.Register("ShowMinButton", typeof(bool), typeof(BorderlessWindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowMaxRestoreButtonProperty = DependencyProperty.Register("ShowMaxRestoreButton", typeof(bool), typeof(BorderlessWindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(BorderlessWindowBase), new PropertyMetadata(true));
        public static readonly DependencyProperty MinimizeToolTipProperty = DependencyProperty.Register("MinimizeToolTip", typeof(string), typeof(BorderlessWindowBase), new PropertyMetadata("Minimize", MinimizeToolTipChangedCallback));
        public static readonly DependencyProperty MaximizeToolTipProperty = DependencyProperty.Register("MaximizeToolTip", typeof(string), typeof(BorderlessWindowBase), new PropertyMetadata("Maximize", MaximizeToolTipChangedCallback));
        public static readonly DependencyProperty RestoreToolTipProperty = DependencyProperty.Register("RestoreToolTip", typeof(string), typeof(BorderlessWindowBase), new PropertyMetadata("Restore", RestoreToolTipChangedCallback));
        public static readonly DependencyProperty CloseToolTipProperty = DependencyProperty.Register("CloseToolTip", typeof(string), typeof(BorderlessWindowBase), new PropertyMetadata("Close", CloseToolTipChangedCallback));
        public static readonly DependencyProperty WindowCommandsProperty = DependencyProperty.Register("WindowCommands", typeof(object), typeof(BorderlessWindowBase), new PropertyMetadata(null));
        public static readonly DependencyProperty ShowWindowControlsProperty = DependencyProperty.Register("ShowWindowControls", typeof(bool), typeof(BorderlessWindowBase), new PropertyMetadata(true));
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

            this.MouseLeftButtonDown -= BorderlessWindowBase_MouseLeftButtonDown;
            this.MouseLeftButtonDown += BorderlessWindowBase_MouseLeftButtonDown;

            this.SizeChanged -= BorderlessWindowBase_SizeChanged;
            this.SizeChanged += BorderlessWindowBase_SizeChanged;

            this.InitializeWindow();
        }
        #endregion

        #region Public
        protected void InitializeWindow()
        {
            // Create the WindowChrome
            this.windowChrome = new WindowChrome
            {
                CaptionHeight = 0,
                CornerRadius = new CornerRadius(0)
            };

            // Assign the WindowChrome
            WindowChrome.SetWindowChrome(this, this.windowChrome);

            // ToolTips
            this.minimizeButton.ToolTip = this.MinimizeToolTip;
            this.closeButton.ToolTip = this.CloseToolTip;

            // IsHitTestVisibleInChromeProperty (In comment because this blocks 
            // resizing of the window when the mouse Is over the close button)
            //this.minimizeButton.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
            //this.maximizeButton.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
            //this.closeButton.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
            //this.windowCommands.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);

            // Handlers
            this.SizeChanged -= this.BorderlessWindowBase_SizeChanged;
            this.SizeChanged += this.BorderlessWindowBase_SizeChanged;

            // Store the previous BorderThickness
            this.previousBorderThickness = this.windowBorder.BorderThickness;

            // Update the Window for the first time
            this.UpdateWindow();
        }

        protected void UpdateWindow()
        {
            if (this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip)
            {
                this.windowChrome.ResizeBorderThickness = new Thickness(6);
            }
            else
            {
                this.windowChrome.ResizeBorderThickness = new Thickness(0);
            }

            if (this.WindowState == WindowState.Maximized)
            {
                var mHwnd = new WindowInteropHelper(this).Handle;
                var monitor = NativeMethods.MonitorFromWindow(mHwnd, Constants.MONITOR_DEFAULTTONEAREST);

                var pData = new APPBARDATA();
                pData.cbSize = Marshal.SizeOf(pData);
                pData.hWnd = mHwnd;

                if (Convert.ToBoolean(NativeMethods.SHAppBarMessage((int)ABMsg.ABM_GETSTATE, ref pData)))
                {
                  if (monitor != IntPtr.Zero)
                    {
                        var monitorInfo = new MONITORINFO();
                        NativeMethods.GetMonitorInfo(monitor, monitorInfo);
                        int x = monitorInfo.rcWork.left;
                        int y = monitorInfo.rcWork.top;
                        int cx = monitorInfo.rcWork.right - x;
                        int cy = monitorInfo.rcWork.bottom - y;


                        NativeMethods.SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref pData);
                        var uEdge = GetEdge(pData.rc);

                        switch (uEdge)
                        {
                                case ABEdge.ABE_TOP: y++;
                                    break;
                                case ABEdge.ABE_BOTTOM: cy--;
                                    break;
                                case ABEdge.ABE_LEFT: x++;
                                break;
                                case ABEdge.ABE_RIGHT: cx--;
                                break;
                        }

                        NativeMethods.SetWindowPos(mHwnd, new IntPtr(Constants.HWND_NOTOPMOST), x, y, cx, cy,
                            Constants.SWP_SHOWWINDOW);
                    }
                }
                else
                {
                    this.windowBorder.BorderThickness = new Thickness(6);
                }

                this.windowChrome.GlassFrameThickness = new Thickness(0);
                this.windowChrome.ResizeBorderThickness = new Thickness(0);
                this.maximizeButton.ToolTip = RestoreToolTip;
            }
            else
            {
                this.windowChrome.GlassFrameThickness = new Thickness(1, 0, 0, 0);
                this.windowBorder.BorderThickness = this.previousBorderThickness;
                this.maximizeButton.ToolTip = MaximizeToolTip;
            }
        }

        #endregion

        #region Private
        private static ABEdge GetEdge(RECT rc)
        {
            ABEdge uEdge;
            if (rc.top == rc.left && rc.bottom > rc.right)
                uEdge = ABEdge.ABE_LEFT;
            else if (rc.top == rc.left && rc.bottom < rc.right)
                uEdge = ABEdge.ABE_TOP;
            else if (rc.top > rc.left)
                uEdge = ABEdge.ABE_BOTTOM;
            else
                uEdge = ABEdge.ABE_RIGHT;
            return uEdge;
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

        protected virtual void BorderlessWindowBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal && this.previousWindowState == System.Windows.WindowState.Maximized)
            {
                this.OnRestored();
            }

            this.previousWindowState = this.WindowState;

            this.UpdateWindow();
        }

        private void BorderlessWindowBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        #endregion

        #region Callbacks
        private static void MinimizeToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BorderlessWindowBase win = (BorderlessWindowBase)d;
            win.MinimizeToolTipChanged(win, new EventArgs());
        }

        private static void MaximizeToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BorderlessWindowBase win = (BorderlessWindowBase)d;
            win.MaximizeToolTipChanged(win, new EventArgs());
        }

        private static void RestoreToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BorderlessWindowBase win = (BorderlessWindowBase)d;
            win.RestoreToolTipChanged(win, new EventArgs());
        }

        private static void CloseToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BorderlessWindowBase win = (BorderlessWindowBase)d;
            win.CloseToolTipChanged(win, new EventArgs());
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
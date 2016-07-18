using System.Windows;
using System.Windows.Shell;

namespace Digimezzo.WPFControls
{
    public class BorderlessWindow : WindowBase
    {
        #region Variables
        private WindowChrome windowChrome;
        #endregion

        #region Construction
        static BorderlessWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderlessWindow), new FrameworkPropertyMetadata(typeof(BorderlessWindow)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.InitializeWindow();
        }

        protected override void WindowBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.WindowBase_SizeChanged(sender, e);
            this.UpdateWindow();
        }
        #endregion

        #region Private
        private void InitializeWindow()
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
            this.SizeChanged -= this.WindowBase_SizeChanged;
            this.SizeChanged += this.WindowBase_SizeChanged;

            // Update the Window for the first time
            this.UpdateWindow();
        }
        private void UpdateWindow()
        {
            if (this.ResizeMode == System.Windows.ResizeMode.CanResize || this.ResizeMode == System.Windows.ResizeMode.CanResizeWithGrip)
            {
                this.windowChrome.ResizeBorderThickness = new Thickness(6);
            }
            else
            {
                this.windowChrome.ResizeBorderThickness = new Thickness(0);
            }

            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.windowChrome.GlassFrameThickness = new Thickness(0);
                this.windowBorder.BorderThickness = new Thickness(6);
                this.maximizeButton.ToolTip = RestoreToolTip;
            }
            else
            {
                this.windowChrome.GlassFrameThickness = new Thickness(1, 0, 0, 0);
                this.windowBorder.BorderThickness = new Thickness(0);
                this.maximizeButton.ToolTip = MaximizeToolTip;
            }
        }
        #endregion
    }
}
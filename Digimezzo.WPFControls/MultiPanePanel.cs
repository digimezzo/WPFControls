using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Digimezzo.WPFControls
{
    public class MultiPanePanel : Control
    {
        private Border leftPane;
        private Border middlePane;
        private Border rightPane;
        private ContentPresenter leftPaneContent;
        private ContentPresenter middlePaneContent;
        private ContentPresenter rightPaneContent;

        private Border separatorLeft;
        private bool isSeparatorLeftMouseButtonDown;

        private Border separatorRight;
        private bool isSeparatorRightMouseButtonDown;

        private Timer resizeContentTimer = new Timer();

        public bool IsRightPaneCollapsed
        {
            get { return Convert.ToBoolean(GetValue(IsRightPaneCollapsedProperty)); }
            set { SetValue(IsRightPaneCollapsedProperty, value); }
        }

        public static readonly DependencyProperty IsRightPaneCollapsedProperty =
            DependencyProperty.Register(nameof(IsRightPaneCollapsed), typeof(bool), typeof(MultiPanePanel), new PropertyMetadata(false));

        public bool CanResize
        {
            get { return Convert.ToBoolean(GetValue(CanResizeProperty)); }
            set { SetValue(CanResizeProperty, value); }
        }

        public static readonly DependencyProperty CanResizeProperty =
            DependencyProperty.Register(nameof(CanResize), typeof(bool), typeof(MultiPanePanel), new PropertyMetadata(true));

        public int SeparatorMarginTop
        {
            get { return Convert.ToInt32(GetValue(SeparatorMarginTopProperty)); }
            set { SetValue(SeparatorMarginTopProperty, value); }
        }

        public static readonly DependencyProperty SeparatorMarginTopProperty =
           DependencyProperty.Register(nameof(SeparatorMarginTop), typeof(int), typeof(MultiPanePanel), new PropertyMetadata(0));

        public int SeparatorMarginBottom
        {
            get { return Convert.ToInt32(GetValue(SeparatorMarginBottomProperty)); }
            set { SetValue(SeparatorMarginBottomProperty, value); }
        }

        public static readonly DependencyProperty SeparatorMarginBottomProperty =
            DependencyProperty.Register(nameof(SeparatorMarginBottom), typeof(int), typeof(MultiPanePanel), new PropertyMetadata(0));

        public int ContentResizeDelay
        {
            get { return Convert.ToInt32(GetValue(ContentResizeDelayProperty)); }
            set { SetValue(ContentResizeDelayProperty, value); }
        }

        public static readonly DependencyProperty ContentResizeDelayProperty =
           DependencyProperty.Register(nameof(ContentResizeDelay), typeof(int), typeof(MultiPanePanel), new PropertyMetadata(0));

        public double ResizeGripWidth
        {
            get { return Convert.ToDouble(GetValue(ResizeGripWidthProperty)); }
            set { SetValue(ResizeGripWidthProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripWidthProperty =
            DependencyProperty.Register(nameof(ResizeGripWidth), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(5.0));

        public double LeftPaneWidthPercent
        {
            get { return Convert.ToDouble(GetValue(LeftPaneWidthPercentProperty)); }
            set { SetValue(LeftPaneWidthPercentProperty, value); }
        }

        public static readonly DependencyProperty LeftPaneWidthPercentProperty =
            DependencyProperty.Register(nameof(LeftPaneWidthPercent), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(33.0));

        public double LeftPaneMinimumWidth
        {
            get { return Convert.ToDouble(GetValue(LeftPaneMinimumWidthProperty)); }
            set { SetValue(LeftPaneMinimumWidthProperty, value); }
        }

        public static readonly DependencyProperty LeftPaneMinimumWidthProperty =
            DependencyProperty.Register(nameof(LeftPaneMinimumWidth), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(0.0));

        public double RightPaneWidthPercent
        {
            get { return Convert.ToDouble(GetValue(RightPaneWidthPercentProperty)); }
            set { SetValue(RightPaneWidthPercentProperty, value); }
        }

        public static readonly DependencyProperty RightPaneWidthPercentProperty =
           DependencyProperty.Register(nameof(RightPaneWidthPercent), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(33.0));

        public double RightPaneMinimumWidth
        {
            get { return Convert.ToDouble(GetValue(RightPaneMinimumWidthProperty)); }
            set { SetValue(RightPaneMinimumWidthProperty, value); }
        }

        public static readonly DependencyProperty RightPaneMinimumWidthProperty =
           DependencyProperty.Register(nameof(RightPaneMinimumWidth), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(0.0));

        public double MiddlePaneMinimumWidth
        {
            get { return Convert.ToDouble(GetValue(MiddlePaneMinimumWidthProperty)); }
            set { SetValue(MiddlePaneMinimumWidthProperty, value); }
        }

        public static readonly DependencyProperty MiddlePaneMinimumWidthProperty =
        DependencyProperty.Register(nameof(MiddlePaneMinimumWidth), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(0.0));

        public object LeftPaneContent
        {
            get { return (object)GetValue(LeftPaneContentProperty); }
            set { SetValue(LeftPaneContentProperty, value); }
        }

        public static readonly DependencyProperty LeftPaneContentProperty =
           DependencyProperty.Register(nameof(LeftPaneContent), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(null));

        public object MiddlePaneContent
        {
            get { return (object)GetValue(MiddlePaneContentProperty); }
            set { SetValue(MiddlePaneContentProperty, value); }
        }

        public static readonly DependencyProperty MiddlePaneContentProperty =
           DependencyProperty.Register(nameof(MiddlePaneContent), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(null));

        public object RightPaneContent
        {
            get { return (object)GetValue(RightPaneContentProperty); }
            set { SetValue(RightPaneContentProperty, value); }
        }

        public static readonly DependencyProperty RightPaneContentProperty =
           DependencyProperty.Register(nameof(RightPaneContent), typeof(object), typeof(MultiPanePanel), new PropertyMetadata(null));

        static MultiPanePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiPanePanel), new FrameworkPropertyMetadata(typeof(MultiPanePanel)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.leftPane = (Border)GetTemplateChild("PART_LeftPane");
            this.middlePane = (Border)GetTemplateChild("PART_MiddlePane");
            this.rightPane = (Border)GetTemplateChild("PART_RightPane");

            this.leftPaneContent = (ContentPresenter)GetTemplateChild("PART_LeftPaneContent");
            this.middlePaneContent = (ContentPresenter)GetTemplateChild("PART_MiddlePaneContent");
            this.rightPaneContent = (ContentPresenter)GetTemplateChild("PART_RightPaneContent");

            this.separatorLeft = (Border)GetTemplateChild("PART_SeparatorLeft");
            this.separatorRight = (Border)GetTemplateChild("PART_SeparatorRight");

            this.separatorLeft.Margin = new Thickness(0, this.SeparatorMarginTop, 0, this.SeparatorMarginBottom);
            this.separatorRight.Margin = new Thickness(0, this.SeparatorMarginTop, 0, this.SeparatorMarginBottom);

            base.SizeChanged += this.MultiPanePanel_SizeChanged;
            base.MouseLeftButtonUp += this.MultiPanePanel_MouseLeftButtonUp;
            base.MouseMove += this.MultiPanePanel_PreviewMouseMove;

            if (this.separatorLeft != null)
            {
                this.separatorLeft.PreviewMouseLeftButtonDown += SeparatorLeft_PreviewMouseLeftButtonDown;
                this.separatorLeft.PreviewMouseLeftButtonUp += SeparatorLeft_PreviewMouseLeftButtonUp;
                this.separatorLeft.MouseEnter += PART_SeparatorLeft_MouseEnter;
            }

            if (this.separatorRight != null)
            {
                this.separatorRight.PreviewMouseLeftButtonDown += SeparatorRight_PreviewMouseLeftButtonDown;
                this.separatorRight.PreviewMouseLeftButtonUp += SeparatorRight_PreviewMouseLeftButtonUp;
                this.separatorRight.MouseEnter += PART_SeparatorRight_MouseEnter;
            }

            if (this.ContentResizeDelay > 0)
            {
                this.resizeContentTimer.Interval = this.ContentResizeDelay;
            }
            else
            {
                this.resizeContentTimer.Interval = 1;
            }
            this.resizeContentTimer.Elapsed += this.ResizeContentTimer_Elapsed;
        }

        private double GetTotalWidth()
        {
            return !this.IsRightPaneCollapsed ? this.ActualWidth - this.ResizeGripWidth * 2 : this.ActualWidth - this.ResizeGripWidth;
        }

        private void ResizeContentTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.resizeContentTimer.Stop();

            Application.Current.Dispatcher.Invoke(() => this.ResizeContent());
        }

        private void ApplyPercentages()
        {
            if (this.ActualWidth > 0 && this.LeftPaneWidthPercent > 0 && this.RightPaneWidthPercent > 0)
            {
                double totalWidth = this.GetTotalWidth();

                double newLeftPaneWidth = Convert.ToDouble(totalWidth * this.LeftPaneWidthPercent / 100);
                this.leftPane.Width = newLeftPaneWidth > this.LeftPaneMinimumWidth ? newLeftPaneWidth : this.LeftPaneMinimumWidth;

                if (!this.IsRightPaneCollapsed)
                {
                    totalWidth = this.ActualWidth - this.ResizeGripWidth * 2;
                    double newRightPaneWidth = Convert.ToDouble(totalWidth * this.RightPaneWidthPercent / 100);
                    this.rightPane.Width = newRightPaneWidth > this.RightPaneMinimumWidth ? newRightPaneWidth : this.RightPaneMinimumWidth;
                    double proposedWidth = totalWidth - this.leftPane.Width - this.rightPane.Width;
                    this.middlePane.Width = proposedWidth >= 0 ? proposedWidth : 0.0;
                }
                else
                {
                    double newMiddlePaneWidth = totalWidth - this.leftPane.Width;
                    this.middlePane.Width = newMiddlePaneWidth > this.MiddlePaneMinimumWidth ? newMiddlePaneWidth : this.MiddlePaneMinimumWidth;
                }

                this.resizeContentTimer.Stop();
                this.resizeContentTimer.Start();
            }
        }

        private void RecalculatePercentages()
        {
            double totalWidth = this.GetTotalWidth();

            this.LeftPaneWidthPercent = Math.Round(this.leftPane.Width * 100 / totalWidth);
            if (!this.IsRightPaneCollapsed) this.RightPaneWidthPercent = Math.Round(this.rightPane.Width * 100 / totalWidth);

            this.ApplyPercentages();
        }

        private void ResizeContent()
        {
            this.leftPaneContent.Width = this.leftPane.ActualWidth;
            this.middlePaneContent.Width = this.middlePane.ActualWidth;
            if (!this.IsRightPaneCollapsed) this.rightPaneContent.Width = this.rightPane.ActualWidth;
        }

        private void MultiPanePanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ApplyPercentages();
        }

        private void MultiPanePanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.CanResize) { return; }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.isSeparatorLeftMouseButtonDown)
                {
                    Point p = e.GetPosition(this.separatorLeft);

                    double newLeftWidth = this.leftPane.Width + p.X;
                    double newMiddleWidth = this.middlePane.Width - p.X;

                    if (newLeftWidth > this.LeftPaneMinimumWidth && newMiddleWidth > this.MiddlePaneMinimumWidth)
                    {
                        this.leftPane.Width = newLeftWidth;
                        this.middlePane.Width = newMiddleWidth;
                        this.RecalculatePercentages();
                    }
                }

                if (this.isSeparatorRightMouseButtonDown && !this.IsRightPaneCollapsed)
                {
                    Point p = e.GetPosition(this.separatorRight);

                    double newRightWidth = this.rightPane.Width - p.X;
                    double newMiddleWidth = this.middlePane.Width + p.X;

                    if (newRightWidth > this.RightPaneMinimumWidth && newMiddleWidth > this.MiddlePaneMinimumWidth)
                    {
                        this.rightPane.Width = newRightWidth;
                        this.middlePane.Width = newMiddleWidth;
                        this.RecalculatePercentages();
                    }
                }
            }
            else
            {
                this.isSeparatorLeftMouseButtonDown = false;
                this.isSeparatorRightMouseButtonDown = false;
            }
        }

        private void MultiPanePanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isSeparatorLeftMouseButtonDown = false;
            this.isSeparatorRightMouseButtonDown = false;
        }

        private void PART_SeparatorLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.CanResize) { this.separatorLeft.Cursor = Cursors.SizeWE; }
        }

        private void SeparatorLeft_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isSeparatorLeftMouseButtonDown = false;
        }

        private void SeparatorLeft_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.isSeparatorLeftMouseButtonDown = true;
        }

        private void PART_SeparatorRight_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.CanResize && !this.IsRightPaneCollapsed) { this.separatorRight.Cursor = Cursors.SizeWE; }
        }

        private void SeparatorRight_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsRightPaneCollapsed) this.isSeparatorRightMouseButtonDown = false;
        }

        private void SeparatorRight_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsRightPaneCollapsed) this.isSeparatorRightMouseButtonDown = true;
        }
    }
}
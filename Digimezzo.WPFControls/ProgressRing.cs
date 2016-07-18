using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.WPFControls
{
    /// <summary>
    /// ProgressRing is based on code from MahApps.Metro: https://github.com/MahApps/MahApps.Metro
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    [TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public class ProgressRing : Control
    {
        #region Variables
        private List<Action> deferredActions = new List<Action>();
        #endregion

        #region Properties
        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            private set { SetValue(MaxSideLengthProperty, value); }
        }

        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            private set { SetValue(EllipseDiameterProperty, value); }
        }

        public double EllipseDiameterScale
        {
            get { return (double)GetValue(EllipseDiameterScaleProperty); }
            set { SetValue(EllipseDiameterScaleProperty, value); }
        }

        public Thickness EllipseOffset
        {
            get { return (Thickness)GetValue(EllipseOffsetProperty); }
            private set { SetValue(EllipseOffsetProperty, value); }
        }

        public double BindableWidth
        {
            get { return (double)GetValue(BindableWidthProperty); }
            private set { SetValue(BindableWidthProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public bool IsLarge
        {
            get { return (bool)GetValue(IsLargeProperty); }
            set { SetValue(IsLargeProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double), BindableWidthCallback));
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, IsActiveChanged));
        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, IsLargeChangedCallback));
        public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(ProgressRing), new PropertyMetadata(default(Thickness)));
        public static readonly DependencyProperty EllipseDiameterScaleProperty = DependencyProperty.Register("EllipseDiameterScale", typeof(double), typeof(ProgressRing), new PropertyMetadata(1D));
        #endregion

        #region Construction
        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
            VisibilityProperty.OverrideMetadata(typeof(ProgressRing),
                                                new FrameworkPropertyMetadata(
                                                    new PropertyChangedCallback(
                                                        (ringObject, e) => {
                                                            if (e.NewValue != e.OldValue)
                                                            {
                                                                var ring = (ProgressRing)ringObject;
                                                                // Auto set IsActive to false if we're hiding it.
                                                                if ((Visibility)e.NewValue != Visibility.Visible)
                                                                {
                                                                    // Sets the value without overriding it's binding (if any).
                                                                    ring.SetCurrentValue(ProgressRing.IsActiveProperty, false);
                                                                }
                                                                else
                                                                {
                                                                    // Don't forget to re-activate
                                                                    ring.IsActive = true;
                                                                }
                                                            }
                                                        })));
        }

        public ProgressRing()
        {
            this.SizeChanged += OnSizeChanged;
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            // Make sure the states get updated
            this.UpdateLargeState();
            this.UpdateActiveState();
            base.OnApplyTemplate();

            if (this.deferredActions != null)
            {
                foreach (var action in this.deferredActions)
                {
                    action();
                }
            }

            this.deferredActions = null;
        }
        #endregion

        #region Callbacks
        private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as ProgressRing;
            if (ring == null)
                return;

            var action = new Action(() =>
            {

                ring.SetEllipseDiameter(
                    (double)dependencyPropertyChangedEventArgs.NewValue);
                ring.SetEllipseOffset(
                    (double)dependencyPropertyChangedEventArgs.NewValue);
                ring.SetMaxSideLength(
                    (double)dependencyPropertyChangedEventArgs.NewValue);
            });

            if (ring.deferredActions != null)
                ring.deferredActions.Add(action);
            else
                action();
        }

        private static void IsLargeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as ProgressRing;
            if (ring == null)
                return;

            ring.UpdateLargeState();
        }
        #endregion

        #region Private
        private void SetMaxSideLength(double width)
        {
            MaxSideLength = width <= 20 ? 20 : width;
        }

        private void SetEllipseDiameter(double width)
        {
            EllipseDiameter = (width / 8) * EllipseDiameterScale;
        }

        private void SetEllipseOffset(double width)
        {
            EllipseOffset = new Thickness(0, width / 2, 0, 0);
        }

        private void UpdateLargeState()
        {
            Action action;

            if (IsLarge)
            {
                action = () => VisualStateManager.GoToState(this, "Large", true);
            }
            else
            {
                action = () => VisualStateManager.GoToState(this, "Small", true);
            }

            if (this.deferredActions != null)
            {
                this.deferredActions.Add(action);
            }
            else
            {
                action();
            }    
        }

        private void UpdateActiveState()
        {
            Action action;

            if (IsActive)
            {
                action = () => VisualStateManager.GoToState(this, "Active", true);
            }
            else
            {
                action = () => VisualStateManager.GoToState(this, "Inactive", true);
            }

            if (this.deferredActions != null)
            {
                this.deferredActions.Add(action);
            }
            else
            {
                action();
            }
        }
        #endregion

        #region Event Handlers
        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            BindableWidth = ActualWidth;
        }

        private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as ProgressRing;
            if (ring == null) return;

            ring.UpdateActiveState();
        }
        #endregion 
    }
}
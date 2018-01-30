using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Digimezzo.WPFControls
{
    /// <summary>
    /// ColorPicker is based on code from 
    /// Gu.Wpf.Geometry owned by JohanLarsson: https://github.com/JohanLarsson/Gu.Wpf.Geometry
    /// PropertyTools owned by objorke: https://github.com/objorke/PropertyTools
    /// Their license is included in the "Licenses" folder.
    /// </summary>
    public sealed class ColorPicker : Control
    {
        #region Variables

        private static Color DefaultColor = Colors.White;
        private const double Radius = 150;
        private const double Radius2 = Radius * Radius;

        private bool isMouseDown = false;
        private Canvas pCanvas;
        private Ellipse pEllipse;
        private Thumb pThumb;
        private Slider vSlider;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Control Properties
        public static readonly DependencyProperty ButtonInnerBackgroundProperty =
            DependencyProperty.Register(nameof(ButtonInnerBackground), typeof(Brush), typeof(HorizontalUWPSlider),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(DefaultColor));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ColorPicker),
                new PropertyMetadata(1d, ComponentChangedCallback));

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register(nameof(Hue), typeof(double), typeof(ColorPicker),
                new PropertyMetadata(0d, ComponentChangedCallback));

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(ColorPicker),
                new PropertyMetadata(1d, ComponentChangedCallback));

        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register(nameof(Red), typeof(int), typeof(ColorPicker),
                new PropertyMetadata(0, RgbComponentChangedCallback));

        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register(nameof(Green), typeof(int), typeof(ColorPicker),
                new PropertyMetadata(0, RgbComponentChangedCallback));

        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register(nameof(Blue), typeof(int), typeof(ColorPicker),
                new PropertyMetadata(0, RgbComponentChangedCallback));

        public Brush ButtonInnerBackground
        {
            get => (Brush)GetValue(ButtonInnerBackgroundProperty);
            set => SetValue(ButtonInnerBackgroundProperty, value);
        }

        public Color SelectedColor
        {
            get => (Color) GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public double Saturation
        {
            get => (double) GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public double Hue
        {
            get => (double) GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public int Red
        {
            get => (int) GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public int Green
        {
            get => (int) GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public int Blue
        {
            get => (int) GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker),
                new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            pCanvas = (Canvas) GetTemplateChild("PART_PickerCanvas");
            pEllipse = (Ellipse) GetTemplateChild("PART_PickerEllipse");
            pThumb = (Thumb) GetTemplateChild("PART_Thumb");
            vSlider = (Slider) GetTemplateChild("PART_ValueSlider");

            pThumb.DragDelta += PickerThumb_DragDelta;
            pThumb.DragCompleted += PickerThumb_DragCompleted;
            pCanvas.MouseDown += PickerCanvas_MouseDown;
            vSlider.ValueChanged += ValueSlider_ValueChanged;

            InitPalette();
        }

        private void ValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateColor();
        }

        #endregion

        #region Event Handlers

        private static void ComponentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnComponentChanged(e);
        }

        private static void RgbComponentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnRgbComponentChanged(e);
        }
        
        private void PickerThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isMouseDown = false;
        }

        private void PickerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double nX = Canvas.GetLeft(pThumb) + e.HorizontalChange, nY = Canvas.GetTop(pThumb) + e.VerticalChange;
            GetColorFromCurrentPositionAndMoveThumb(nX, nY);
        }

        private void PickerCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                return;
            isMouseDown = true;
            var mousePos = Mouse.GetPosition(pEllipse);
            GetColorFromCurrentPositionAndMoveThumb(mousePos.X, mousePos.Y);
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => pThumb.RaiseEvent(e)));
        }

        #endregion

        private void InitPalette()
        {
            if (SelectedColor != DefaultColor)
            {
                var hsv = ColorToHsv(SelectedColor);
                SetThumbPosition(hsv);
                Red = SelectedColor.R;
                Green = SelectedColor.G;
                Blue = SelectedColor.B;
                Hue = hsv.hue;
                Saturation = hsv.sat;
                Value = hsv.val;
            }
        }

        private void OnComponentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!isMouseDown&&e.Property != ValueProperty)
            {
                {
                    SetThumbPosition(SelectedColor);
                }
            }
        }

        private void OnRgbComponentChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        private void GetColorFromCurrentPositionAndMoveThumb(double nX, double nY)
        {
            double diffX = nX - Radius, diffY = nY - Radius;
            var radian = Math.Atan(diffY / diffX);
            var hue = RadianToAngle(radian);
            if ((nX >= Radius && nY < Radius) || (nX >= Radius && nY >= Radius))
                hue = hue + 180;
            if (nX < Radius && nY >= Radius)
                hue = hue + 360;
            Hue = hue / 360d;

            var euclid = diffX * diffX + diffY * diffY;
            if (Radius2 - euclid >= 0)
            {
                Canvas.SetLeft(pThumb, nX);
                Canvas.SetTop(pThumb, nY);
                Saturation = Math.Sqrt(euclid) / 150d;
            }
            else
            {
                Saturation = 1d;
                nX = Math.Cos(radian) * Radius;
                nY = Math.Sin(radian) * Radius;
                if (diffX < 0)
                {
                    Canvas.SetLeft(pThumb, Radius - nX);
                    Canvas.SetTop(pThumb, Radius - nY);
                }
                else
                {
                    Canvas.SetLeft(pThumb, Radius + nX);
                    Canvas.SetTop(pThumb, Radius + nY);
                }
            }

            UpdateColor();
        }

        private void SetThumbPosition(Color color)
        {
            if (pThumb == null)
                return;

            var hsv = ColorToHsv(color);
            SetThumbPosition(hsv);
        }

        private void SetThumbPosition((double hue, double sat, double val) hsv)
        {
            var radian = AngleToRadian(hsv.hue * 360d);
            double nX = Math.Cos(radian) * hsv.sat * Radius, nY = Math.Sin(radian) * hsv.sat * Radius;
            Canvas.SetLeft(pThumb, 150 - nX);
            Canvas.SetTop(pThumb, 150 - nY);
        }

        private static double AngleToRadian(double radian)
        {
            return radian * Math.PI / 180d;
        }

        private static double RadianToAngle(double angle)
        {
            return angle / Math.PI * 180d;
        }

        private static Color HsvToColor(double hue, double sat, double val, double alpha = 1.0)
        {
            double r = 0;
            double g = 0;
            double b = 0;

            if (sat == 0)
            {
                // Gray scale
                r = g = b = val;
            }
            else
            {
                if (hue == 1.0)
                {
                    hue = 0;
                }

                hue *= 6.0;
                var i = (int) Math.Floor(hue);
                double f = hue - i;
                double aa = val * (1 - sat);
                double bb = val * (1 - (sat * f));
                double cc = val * (1 - (sat * (1 - f)));
                switch (i)
                {
                    case 0:
                        r = val;
                        g = cc;
                        b = aa;
                        break;
                    case 1:
                        r = bb;
                        g = val;
                        b = aa;
                        break;
                    case 2:
                        r = aa;
                        g = val;
                        b = cc;
                        break;
                    case 3:
                        r = aa;
                        g = bb;
                        b = val;
                        break;
                    case 4:
                        r = cc;
                        g = aa;
                        b = val;
                        break;
                    case 5:
                        r = val;
                        g = aa;
                        b = bb;
                        break;
                }
            }

            return Color.FromArgb((byte) (alpha * 255), (byte) (r * 255), (byte) (g * 255), (byte) (b * 255));
        }

        private static (double hue, double sat, double val) ColorToHsv(Color color)
        {
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;

            double h = 0;
            double s;

            double min = Math.Min(Math.Min(r, g), b);
            double v = Math.Max(Math.Max(r, g), b);
            double delta = v - min;

            if (v == 0.0)
            {
                s = 0;
            }
            else
            {
                s = delta / v;
            }

            if (s == 0)
            {
                h = 0.0;
            }
            else
            {
                if (r == v)
                {
                    h = (g - b) / delta;
                }
                else if (g == v)
                {
                    h = 2 + (b - r) / delta;
                }
                else if (b == v)
                {
                    h = 4 + (r - g) / delta;
                }

                h *= 60;
                if (h < 0.0)
                {
                    h = h + 360;
                }
            }

            return (h / 360d, s, v / 255d);
        }

        private void UpdateColor()
        {
            SelectedColor = HsvToColor(Hue, Saturation, Value);
            Red = SelectedColor.R;
            Green = SelectedColor.G;
            Blue = SelectedColor.B;
        }
    }
}
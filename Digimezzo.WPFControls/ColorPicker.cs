using System;
using System.Globalization;
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

        private const double Radius = 150;
        private const double Radius2 = Radius * Radius;

        private bool withinColorChange = false;
        private Canvas pCanvas;
        private Ellipse pEllipse;
        private Thumb pThumb;
        private Slider vSlider;

        #endregion

        #region Control Properties
        private static readonly DependencyProperty SelectedFullValueColorProperty =
            DependencyProperty.Register(nameof(SelectedFullValueColor), typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(Colors.White));

        private Color SelectedFullValueColor
        {
            get => (Color) GetValue(SelectedFullValueColorProperty);
            set => SetValue(SelectedFullValueColorProperty, value);
        }

        public static readonly DependencyProperty ButtonInnerBackgroundProperty =
            DependencyProperty.Register(nameof(ButtonInnerBackground), typeof(Brush), typeof(ColorPicker),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush ButtonInnerBackground
        {
            get => (Brush)GetValue(ButtonInnerBackgroundProperty);
            set => SetValue(ButtonInnerBackgroundProperty, value);
        }

        public static readonly DependencyProperty ButtonBackgroundProperty =
            DependencyProperty.Register(nameof(ButtonBackground), typeof(Brush), typeof(ColorPicker),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public Brush ButtonBackground
        {
            get => (Brush)GetValue(ButtonBackgroundProperty);
            set => SetValue(ButtonBackgroundProperty, value);
        }

        public static readonly DependencyProperty TextBoxBackgroundProperty =
            DependencyProperty.Register(nameof(TextBoxBackground), typeof(Brush), typeof(ColorPicker),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(223,223,223))));

        public Brush TextBoxBackground
        {
            get => (Brush)GetValue(TextBoxBackgroundProperty);
            set => SetValue(TextBoxBackgroundProperty, value);
        }

        public static readonly DependencyProperty TextBoxForegroundProperty =
            DependencyProperty.Register(nameof(TextBoxForeground), typeof(Brush), typeof(ColorPicker),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public Brush TextBoxForeground
        {
            get => (Brush)GetValue(TextBoxForegroundProperty);
            set => SetValue(TextBoxForegroundProperty, value);
        }

        public static readonly DependencyProperty TextBoxSelectionBrushProperty =
            DependencyProperty.Register(nameof(TextBoxSelectionBrush), typeof(Brush), typeof(ColorPicker),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0,120,215))));

        public Brush TextBoxSelectionBrush
        {
            get => (Brush)GetValue(TextBoxSelectionBrushProperty);
            set => SetValue(TextBoxSelectionBrushProperty, value);
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(Colors.White));

        public Color SelectedColor
        {
            get => (Color) GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly DependencyProperty HexProperty = 
            DependencyProperty.Register(nameof(Hex),typeof(string),typeof(ColorPicker),
                new PropertyMetadata("#FF000000", ComponentChangedCallback));

        public string Hex
        {
            get => (string) GetValue(HexProperty);
            set => SetValue(HexProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ColorPicker),
                new PropertyMetadata(1d, ComponentChangedCallback));

        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register(nameof(Hue), typeof(double), typeof(ColorPicker),
                new PropertyMetadata(0d, ComponentChangedCallback));
        
        public double Hue
        {
            get => (double) GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(ColorPicker),
                new PropertyMetadata(1d, ComponentChangedCallback));

        public double Saturation
        {
            get => (double) GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register(nameof(Red), typeof(int), typeof(ColorPicker),
                new PropertyMetadata(0, ComponentChangedCallback));

        public int Red
        {
            get => (int) GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register(nameof(Green), typeof(int), typeof(ColorPicker),
                new PropertyMetadata(0, ComponentChangedCallback));

        public int Green
        {
            get => (int) GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register(nameof(Blue), typeof(int), typeof(ColorPicker),
                new PropertyMetadata(0, ComponentChangedCallback));

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

            // Initalize Palette
            if (SelectedColor != Colors.White)
            {
                withinColorChange = true;

                UpdateAllComponents();
                ResetPickerThumbPosition();

                withinColorChange = false;
            }
        }

        private void ValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            withinColorChange = true;
            UpdateRgb(new Hsv(Hue, Saturation, Value));
            withinColorChange = false;
        }

        #endregion

        #region Event Handlers

        private static void ComponentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnComponentChanged(e);
        }
        
        private void PickerThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            withinColorChange = false;
        }

        private void PickerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            withinColorChange = true;
            double nX = Canvas.GetLeft(pThumb) + e.HorizontalChange, nY = Canvas.GetTop(pThumb) + e.VerticalChange;
            GetColorFromCurrentPositionAndMoveThumb(nX, nY);
        }

        private void PickerCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                return;
            withinColorChange = true;
            var mousePos = Mouse.GetPosition(pEllipse);
            GetColorFromCurrentPositionAndMoveThumb(mousePos.X, mousePos.Y);
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => pThumb.RaiseEvent(e)));
        }

        #endregion

        private void OnComponentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (withinColorChange)
            {
                return;
            }

            withinColorChange = true;

            if (e.Property == HexProperty)
            {
                var rgb = HexToRgb((string)e.NewValue);
                UpdateHsv(rgb);
            }
            else if (e.Property == RedProperty)
            {
                var rgb = Color.FromArgb(255, Convert.ToByte(e.NewValue), SelectedColor.G, SelectedColor.B);
                UpdateHsv(rgb);
            }
            else if (e.Property == GreenProperty)
            {
                var rgb = Color.FromArgb(255, SelectedColor.R, Convert.ToByte(e.NewValue), SelectedColor.B);
                UpdateHsv(rgb);
            }
            else if (e.Property == BlueProperty)
            {
                var rgb = Color.FromArgb(255, SelectedColor.R, SelectedColor.G, Convert.ToByte(e.NewValue));
                UpdateHsv(rgb);
            }
            else if (e.Property == SelectedColorProperty)
            {
                UpdateAllComponents();
            }
            else
            {
                var hsv = new Hsv(Hue, Saturation, Value);
                UpdateRgb(hsv);
                if (e.Property != ValueProperty)
                {
                    UpdateValueSlider(hsv);
                }
            }

            ResetPickerThumbPosition();

            withinColorChange = false;
        }

        private void UpdateAllComponents()
        {
            var hsv = RgbToHsv(SelectedColor);
            Hue = hsv.Hue;
            Saturation = hsv.Saturation;
            Value = hsv.Value;
            Red = SelectedColor.R;
            Green = SelectedColor.G;
            Blue = SelectedColor.B;
            Hex = RgbToHex(SelectedColor);

            UpdateValueSlider(hsv);
        }

        private void UpdateHsv(Color rgb)
        {
            var hsv = RgbToHsv(rgb);
            Hue = hsv.Hue;
            Saturation = hsv.Saturation;
            Value = hsv.Value;
            SelectedColor = rgb;
            Hex = RgbToHex(rgb);
            
            UpdateValueSlider(hsv);
        }

        private void UpdateRgb(Hsv hsv)
        {
            var rgb = HsvToRgb(hsv);
            Red = rgb.R;
            Green = rgb.G;
            Blue = rgb.B;
            SelectedColor = rgb;
            Hex = RgbToHex(rgb);   
        }

        private void UpdateValueSlider(Hsv hsv)
        {
            hsv.Value = 1.0;
            var rgb = HsvToRgb(hsv);
            SelectedFullValueColor = rgb;
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

            var hsv = new Hsv(Hue, Saturation, Value);
            UpdateRgb(hsv);
            UpdateValueSlider(hsv);
        }

        private void ResetPickerThumbPosition()
        {
            var radian = AngleToRadian(Hue * 360d);
            double nX = Math.Cos(radian) * Saturation * Radius, nY = Math.Sin(radian) * Saturation * Radius;
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

        private static Color HsvToRgb(Hsv hsv, double alpha = 1.0)
        {
            double hue = hsv.Hue, sat = hsv.Saturation, val = hsv.Value;
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

        private static Hsv RgbToHsv(Color rgb)
        {
            byte r = rgb.R;
            byte g = rgb.G;
            byte b = rgb.B;

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

            
            return new Hsv(h / 360d, s, v / 255d);
        }

        private static Color HexToRgb(string hex)
        {
            return Color.FromRgb(
                byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber),
                byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber),
                byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber));
        }

        private static string RgbToHex(Color rgb)
        {
            return $"#{rgb.R:X02}{rgb.G:X02}{rgb.B:X02}";
        }
    }

    struct Hsv
    {
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Value { get; set; }

        public Hsv(double hue, double saturation, double value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }
    }
}
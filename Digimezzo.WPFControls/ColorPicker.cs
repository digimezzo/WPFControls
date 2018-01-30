using System;
using System.ComponentModel;
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
        private static readonly Color DefaultColor = Colors.White; 
        private const double Radius = 150;
        private const double Radius2 = Radius * Radius;

        private Canvas pCanvas;
        private Ellipse pEllipse;
        private Thumb pThumb;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Control Properties

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(DefaultColor));

        public Color SelectedColor
        {
            get => (Color) GetValue(SelectedColorProperty);
            set
            {
                SetValue(SelectedColorProperty, value);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedColor)));
            }
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

            pThumb.DragDelta += PThumbDragDelta;
            pCanvas.MouseDown += PickerCanvas_MouseDown;

            if (SelectedColor != DefaultColor)
            {
                SetThumbPosition(SelectedColor);
            }
        }

        #endregion

        #region Event Handlers

        private void PThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            double nX = Canvas.GetLeft(pThumb) + e.HorizontalChange, nY = Canvas.GetTop(pThumb) + e.VerticalChange;
            double diffX = nX - Radius, diffY = nY - Radius;
            var radian = Math.Atan(diffY / diffX);
            var hue = RadianToAngle(radian);
            if ((nX >= Radius && nY < Radius) || (nX >= Radius && nY >= Radius))
                hue = hue + 180;
            if (nX < Radius && nY >= Radius)
                hue = hue + 360;

            var euclid = diffX * diffX + diffY * diffY;
            if (Radius2 - euclid >= 0)
            {
                Canvas.SetLeft(pThumb, nX);
                Canvas.SetTop(pThumb, nY);
                UpdateColor(hue, Math.Sqrt(euclid) / 1.5);
            }
            else
            {
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

                UpdateColor(hue, 100);
            }
        }

        private void PickerCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                return;
            var mousePos = Mouse.GetPosition(pEllipse);
            Canvas.SetLeft(pThumb, mousePos.X);
            Canvas.SetTop(pThumb, mousePos.Y);
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => pThumb.RaiseEvent(e)));
        }

        #endregion

        private void SetThumbPosition(Color color)
        {
            var hsv = ColorToHsv(color);
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

        private static Color HsvToColor(double hue, double sat, double val = 1.0, double alpha = 1.0)
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

        private void UpdateColor(double hue, double sat)
        {
            SelectedColor = HsvToColor(hue / 360.0, sat / 100.0);
        }
    }
}
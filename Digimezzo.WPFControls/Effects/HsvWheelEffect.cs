using Digimezzo.WPFControls.Utils;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Digimezzo.WPFControls.Effects
{
    /// ColorPicker is based on code from Gu.Wpf.Geometry owned by JohanLarsson: https://github.com/JohanLarsson/Gu.Wpf.Geometry
    /// Their license is included in the "Licenses" folder.

    /// <summary>
    /// An effect that renders a HSV colour wheel.
    /// </summary>
    public class HsvWheelEffect : ShaderEffect
    {
        #region Variables
        private static readonly PixelShader Shader = new PixelShader()
        {
            UriSource = UriUtils.MakePackUri<HsvWheelEffect>("Effects/HsvWheelEffect.ps")
        };
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(
            nameof(Input),
            typeof(HsvWheelEffect),
            0);

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            nameof(InnerRadius),
            typeof(double),
            typeof(HsvWheelEffect),
            new UIPropertyMetadata(0d, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty InnerSaturationProperty = DependencyProperty.Register(
            nameof(InnerSaturation),
            typeof(double),
            typeof(HsvWheelEffect),
            new UIPropertyMetadata(0d, PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(HsvWheelEffect),
            new UIPropertyMetadata(1d, PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
            nameof(StartAngle),
            typeof(double),
            typeof(HsvWheelEffect),
            new UIPropertyMetadata(
                270d,
                PixelShaderConstantCallback(3)));
        #endregion

        #region Properties
        /// <summary>
        /// There has to be a property of type Brush called "Input". This property contains the input image and it is usually not set directly - it is set automatically when our effect is applied to a control.
        /// </summary>
        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public double InnerRadius
        {
            get => (double)GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        public double InnerSaturation
        {
            get => (double)GetValue(InnerSaturationProperty);
            set => SetValue(InnerSaturationProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }
        #endregion
        
        #region Constructor
        public HsvWheelEffect()
        {
            PixelShader = Shader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(InnerRadiusProperty);
            UpdateShaderValue(InnerSaturationProperty);
            UpdateShaderValue(ValueProperty);
            UpdateShaderValue(StartAngleProperty);
        }
        #endregion
    }
}

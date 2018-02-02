/// ColorPicker is based on code from Gu.Wpf.Geometry owned by JohanLarsson: https://github.com/JohanLarsson/Gu.Wpf.Geometry
/// Their license is included in the "Licenses" folder.

static float PI = 3.14159274f;
static float PI2 = 6.28318548f;
static float2 cp = float2(0.5, 0.5);

/// <summary>The inner radius.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float InnerRadius : register(C0);

/// <summary>The inner saturation.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float InnerSaturation : register(C1);

/// <summary>The value.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float Value : register(C2);

/// <summary>The starting angle of the gradient, clockwise from X-axis</summary>
/// <minValue>-360</minValue>
/// <maxValue>360</maxValue>
/// <defaultValue>90</defaultValue>
float StartAngle : register(C3);

float4 lerp_rgba(float4 x, float4 y, float s)
{
    float a = lerp(x.a, y.a, s);
    float3 rgb = lerp(x.rgb, y.rgb, s) * a;
    return float4(rgb.r, rgb.g, rgb.b, a);
}

float interpolate(float min, float max, float value)
{
    if (min == max)
    {
        return 0.5;
    }

    if (min < max)
    {
        return clamp((value - min) / (max - min), 0, 1);
    }

    return clamp((value - max) / (min - max), 0, 1);
}

float clamp_angle_positive(float a)
{
    if (a < 0)
    {
        return a + PI2;
    }

    return a;
}

float clamp_angle_negative(float a)
{
    if (a > 0)
    {
        return a - PI2;
    }

    return a;
}

float angle_from_start(float2 uv, float2 center_point, float start_angle, float central_angle)
{
    float2 v = uv - center_point;
    return central_angle > 0
        ? clamp_angle_positive(clamp_angle_positive(atan2(v.x, -v.y)) - clamp_angle_positive(start_angle))
        : abs(clamp_angle_negative(clamp_angle_negative(atan2(v.x, -v.y)) - clamp_angle_negative(start_angle)));
}

float3 HUEtoRGB(in float H)
{
    float R = abs(H * 6 - 3) - 1;
    float G = 2 - abs(H * 6 - 2);
    float B = 2 - abs(H * 6 - 4);
    return saturate(float3(R, G, B));
}

// http://www.chilliant.com/rgb2hsv.html
float3 HSVtoRGB(in float3 HSV)
{
    float3 RGB = HUEtoRGB(HSV.x);
    return ((RGB - 1) * HSV.y + 1) * HSV.z;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float2 rv = uv - cp;
    float r = length(rv);
    float ir = InnerRadius / 2;
    if (r >= ir && r <= 0.5)
    {
        float sa = radians(StartAngle);
        float h = interpolate(
            0,
            PI2,
            angle_from_start(uv, cp, sa, PI2));
        float s = lerp(InnerSaturation, 1, interpolate(ir, 0.5, r));
        float v = Value;
		return float4(HSVtoRGB(float3(h, s, v)), 1);
    }

    return float4(0, 0, 0, 0);
}
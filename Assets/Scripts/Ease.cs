using UnityEngine;

public static class Ease
{
    // --- BÉZIER CURVES ---

    // Quadratic Bézier: 3 control points (Arc)
    public static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        //u is the inverse of t, representing the remaining portion of the interpolation
        //t is clamped between 0 and 1 to ensure it doesn't exceed the bounds of the interpolation
        t = Mathf.Clamp01(t); 
        float u = 1f - t; 
        return (u * u * p0) + (2f * u * t * p1) + (t * t * p2);
    }

    // Cubic Bézier: 4 control points (S-Curve)
    public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * u * p0) + (3f * uu * t * p1) + (3f * u * tt * p2) + (tt * t * p3);
    }

    // --- EASING FUNCTIONS ---

    // Linear
    public static float Linear(float t) => Mathf.Clamp01(t);

    // Quadratic
    public static float EaseInQuad(float t)
    {
        t = Mathf.Clamp01(t);
        return t * t;
    }

    public static float EaseOutQuad(float t)
    {
        t = Mathf.Clamp01(t);
        return 1f - (1f - t) * (1f - t);
    }

    public static float EaseInOutQuad(float t)
    {
        t = Mathf.Clamp01(t);
        return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2) / 2f;
    }

    // Cubic
    public static float EaseInCubic(float t)
    {
        t = Mathf.Clamp01(t);
        return t * t * t;
    }

    public static float EaseOutCubic(float t)
    {
        t = Mathf.Clamp01(t);
        return 1f - Mathf.Pow(1f - t, 3);
    }

    public static float EaseInOutCubic(float t)
    {
        t = Mathf.Clamp01(t);
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3) / 2f;
    }

    // Sine
    public static float EaseInSine(float t)
    {
        t = Mathf.Clamp01(t);
        return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
    }

    public static float EaseOutSine(float t)
    {
        t = Mathf.Clamp01(t);
        return Mathf.Sin(t * Mathf.PI * 0.5f);
    }

    public static float EaseInOutSine(float t)
    {
        t = Mathf.Clamp01(t);
        return -(Mathf.Cos(Mathf.PI * t) - 1f) * 0.5f;
    }

    // Exponential
    public static float EaseInExpo(float t)
    {
        t = Mathf.Clamp01(t);
        return t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
    }

    public static float EaseOutExpo(float t)
    {
        t = Mathf.Clamp01(t);
        return t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
    }

    // Elastic (Great for UI pop/punch effects)
    public static float EaseOutElastic(float t)
    {
        t = Mathf.Clamp01(t);
        const float c4 = (2f * Mathf.PI) / 3f;
        return t == 0f ? 0f : t == 1f ? 1f : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
    }
}
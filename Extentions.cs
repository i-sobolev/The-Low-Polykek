using UnityEngine;

public static class Extentions 
{
    // float extentions
    public static float EaseInOutQuad(this float x) => x < 0.5 ? 2 * x * x : 1 - (-2 * x + 2) * (-2 * x + 2) / 2;
    public static float EaseOutQuint(this float x) => 1f - Mathf.Pow(1 - x, 5);
    public static float EaseInQuint(this float x) => Mathf.Pow(x, 4);

    public static Vector3 WithoutY(this Vector3 v) => new Vector3(v.x, 0, v.z);
    public static Vector3 SplitY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
}

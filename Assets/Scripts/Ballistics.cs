using UnityEngine;

public static class Ballistics
{
    public static float CalculateBombReleaseDistance(float speed,float height)
    {
        float g = 9.81f;
        float t = Mathf.Sqrt(2f * height / g);

        return speed * t;
    }
}

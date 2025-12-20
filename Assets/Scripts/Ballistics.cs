using UnityEngine;

public static class Ballistics
{
    public static float CalculateBombReleaseDistance(float speed,float height)
    {
        float g = 9.81f;
        float t = Mathf.Sqrt(2f * height / g);

        return speed * t;
    }

    public static float CalculateBombReleaseDistance(Vector3 velocity, float height)
    {
        float g = 9.81f;
        float t = Mathf.Sqrt(2f * height / g);

        // Берём только горизонтальную скорость
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        float horizontalSpeed = horizontalVelocity.magnitude;

        return horizontalSpeed * t;
    }
}

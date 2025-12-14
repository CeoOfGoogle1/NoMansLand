using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Vector3 velocity;

    public void Initialize(Vector3 initialVelocity)
    {
        velocity = initialVelocity;

        Debug.Log($"Initial Velocity is {initialVelocity}");
    }

    void Update()
    {
        ApplyGravity();

        transform.position += velocity * Time.deltaTime;
    }

    private void ApplyGravity()
    {
        velocity.y += -9.81f * Time.deltaTime;
    }
}

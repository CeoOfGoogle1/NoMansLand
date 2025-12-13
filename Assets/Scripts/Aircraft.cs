using System;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Splines;

public class Aircraft : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform rollVisual;
    [SerializeField] private float rollSpeed = 1f;
    [SerializeField] private float zRotMultiplier = 1f;

    private AircraftBehaviour aircraftBehaviour;
    private AircraftPath aircraftPath;
    private Vector3 spawnPoint;
    private Vector3 endPoint;
    private Vector3 targetPoint;

    private SplineAnimate splineAnimate;
    private Vector3 prevPosition;
    private Vector3 prevVelocity;
    private float currentRoll;

    private bool initialized = false;
    

    public void Initialize(AircraftBehaviour ab, Vector3 newSpawnPoint, Vector3 newEndPoint, Vector3 newTargetPoint, float newSpeed)
    {
        splineAnimate = GetComponent<SplineAnimate>();


        aircraftBehaviour = ab;

        spawnPoint = newSpawnPoint;
        transform.position = spawnPoint;

        endPoint = newEndPoint;

        targetPoint = newTargetPoint;

        aircraftPath = Instantiate(AircraftManager.Instance.GetAircraftPathPrefab()).GetComponent<AircraftPath>();
        aircraftPath.Initialize(ab, newSpawnPoint, newEndPoint, newTargetPoint);

        splineAnimate.Container = aircraftPath.GetSplineContainer();

        initialized = true;

        splineAnimate.Play();
    }

    void Update()
    {
        if (!initialized) return;

        ApplyRoll();
    }

    private void ApplyRoll()
    {
        float dt = Time.deltaTime;
        if (dt <= 0f) return;

        // === 1. Скорость ===
        Vector3 velocity = (transform.position - prevPosition) / dt;

        if (velocity.sqrMagnitude < 0.0001f)
        {
            prevPosition = transform.position;
            return;
        }

        // === 2. Ускорение ===
        Vector3 acceleration = (velocity - prevVelocity) / dt;

        // === 3. Forward ===
        Vector3 forward = velocity.normalized;

        // === 4. Боковое ускорение (убираем forward и гравитацию) ===
        Vector3 aLat =
            acceleration
            - Vector3.Project(acceleration, forward)
            - Physics.gravity;

        float targetRoll = currentRoll;

        if (aLat.sqrMagnitude > 0.0001f)
        {
            Vector3 upAircraft = aLat.normalized;
            Vector3 upWorld = Vector3.up;

            float rollRad = Mathf.Atan2(
                Vector3.Dot(Vector3.Cross(upWorld, upAircraft), forward),
                Vector3.Dot(upWorld, upAircraft)
            );

            targetRoll = rollRad * Mathf.Rad2Deg;
        }

        // === 5. Сглаживание ===
        currentRoll = Mathf.Lerp(
            currentRoll,
            targetRoll * zRotMultiplier,
            rollSpeed * dt
        );

        // currentRoll *= zRotMultiplier;

        // === 6. Применяем только к визуалу ===
        rollVisual.localRotation = Quaternion.Euler(
            rollVisual.localEulerAngles.x,
            rollVisual.localEulerAngles.y,
            currentRoll
        );

        // === 7. Сохраняем состояние ===
        prevVelocity = velocity;
        prevPosition = transform.position;
    }
}

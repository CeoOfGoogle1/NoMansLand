using System;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Splines;
using UnityEngine.VFX;

public class Aircraft : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject bombPrefab;

    //public VisualEffect fx;

    [Header("References")] 
    [SerializeField] private Transform rollVisual;
    [SerializeField] private float rollSpeed = 1f;
    [SerializeField] private float zRotMultiplier = 1f;

    [Header("Values")]
    [SerializeField] private float aircraftSpeed;
    [SerializeField] float dropHeight = 120;

    private AircraftBehaviour aircraftBehaviour;
    private AircraftPath aircraftPath;
    private Vector3 spawnPoint;
    private Vector3 endPoint;
    private Vector3 targetPoint;

    private SplineAnimate splineAnimate;
    private Vector3 prevPosition;
    private Vector3 prevVelocity;
    private Vector3 currentVelocity;
    private float currentRoll;
    private bool hasBomb = true;
    private bool initialized = false;
    

    public void Initialize(AircraftBehaviour ab, Vector3 newSpawnPoint, Vector3 newEndPoint, Vector3 newTargetPoint)
    {
        splineAnimate = GetComponent<SplineAnimate>();

        aircraftBehaviour = ab;

        spawnPoint = newSpawnPoint;
        transform.position = spawnPoint;

        endPoint = newEndPoint;

        targetPoint = newTargetPoint;

        aircraftPath = Instantiate(AircraftManager.Instance.GetAircraftPathPrefab()).GetComponent<AircraftPath>();
        aircraftPath.Initialize(ab, newSpawnPoint, newEndPoint, newTargetPoint, aircraftSpeed, dropHeight);

        splineAnimate.Container = aircraftPath.GetSplineContainer();

        initialized = true;

        splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;

        splineAnimate.MaxSpeed = aircraftSpeed;

        splineAnimate.Play();

        AudioManager.instance.Play("jet", transform.position);
    }

    void Update()
    {
        if (!initialized) return;

        ApplyRoll();

        if(aircraftBehaviour == AircraftBehaviour.BombDropping)
        {
            TryDropBomb();
        }

        //fx.SendEvent("OnBomb");
    }

    private void TryDropBomb()
    {
        Vector3 targetPointAir = targetPoint;
        targetPointAir.y = 0;

        Vector3 currentPosAir = transform.position;
        currentPosAir.y = 0;

        if (Mathf.Abs(Ballistics.CalculateBombReleaseDistance(aircraftSpeed, dropHeight) - currentPosAir.magnitude) <= 1 && hasBomb)
        {
            hasBomb = false;

            Instantiate(bombPrefab, transform.position, Quaternion.identity).GetComponent<Bomb>().Initialize(currentVelocity);
        }

        // Debug.Log($"Desired XZ Distance from target: {Ballistics.CalculateBombReleaseDistance(aircraftSpeed, dropHeight)}, distance from target: {Mathf.Abs(targetPoint.magnitude - currentPosAir.magnitude)}");
    }

    private void ApplyRoll()
    {
        float dt = Time.deltaTime;
        if (dt <= 0f) return;

        // === 1. Скорость ===
        currentVelocity = (transform.position - prevPosition) / dt;
        Vector3 velocity = currentVelocity;

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

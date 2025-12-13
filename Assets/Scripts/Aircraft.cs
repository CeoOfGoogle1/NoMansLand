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

    private AircraftBehaviour aircraftBehaviour;
    private AircraftPath aircraftPath;
    private Vector3 spawnPoint;
    private Vector3 endPoint;
    private Vector3 targetPoint;

    private SplineAnimate splineAnimate;
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

    }
}

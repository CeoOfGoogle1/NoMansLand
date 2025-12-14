using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class AircraftPath : MonoBehaviour
{
    private AircraftBehaviour aircraftBehaviour;
    private Vector3 spawnPoint;
    private Vector3 endPoint;
    private Vector3 targetPoint;

    private SplineContainer splineContainer;
    private Spline spline;

    private float speed;

    private float dropHeight;

    public void Initialize(AircraftBehaviour ab, Vector3 newSpawnPoint, Vector3 newEndPoint, Vector3 newTargetPoint, float aircraftSpeed, float newDropHeight)
    {
        aircraftBehaviour = ab;

        spawnPoint = newSpawnPoint;

        endPoint = newEndPoint;

        targetPoint = newTargetPoint;

        speed = aircraftSpeed;

        dropHeight = newDropHeight;

        SetupSpline();
    }

    private void SetupSpline()
    {
        splineContainer = GetComponent<SplineContainer>();

        spline = splineContainer.AddSpline();

        if (aircraftBehaviour == AircraftBehaviour.Flying)
        {
            BezierKnot[] knots = new BezierKnot[3];
            knots[0] = new BezierKnot(spawnPoint);
            knots[1] = new BezierKnot(targetPoint + new Vector3(0, 50, 0));
            knots[2] = new BezierKnot(endPoint);

            spline.Knots = knots;

            spline.SetTangentMode(TangentMode.AutoSmooth);
        }

        if (aircraftBehaviour == AircraftBehaviour.BombDropping)
        {
            // gather height
            float heightGatheringDistance = 1500f;

            BezierKnot[] knots = new BezierKnot[5];

            knots[0] = new BezierKnot(spawnPoint);

            float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 heightGatheredPos = new Vector3(spawnPoint.x + Mathf.Cos(angle) * heightGatheringDistance, dropHeight, spawnPoint.z + Mathf.Sin(angle) * heightGatheringDistance);
            
            Vector3 midHeightGatherPos = Vector3.Lerp(spawnPoint, heightGatheredPos, 0.5f);
            midHeightGatherPos.y /= 1.5f;

            knots[1] = new BezierKnot(midHeightGatherPos);

            knots[2] = new BezierKnot(heightGatheredPos); 

            // go to drop bomb

            knots[3] = new BezierKnot(targetPoint + new Vector3(0, dropHeight, 0));

            // go down

            knots[4] = new BezierKnot(endPoint);

            //finish
            spline.Knots = knots;

            spline.SetTangentMode(TangentMode.AutoSmooth);
        }
        

        
    }

    public SplineContainer GetSplineContainer()
    {
        return splineContainer;
    }
}

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
            BezierKnot[] knots = new BezierKnot[3];

            knots[0] = new BezierKnot(spawnPoint + new Vector3(0, dropHeight, 0));

            // go to drop bomb

            knots[1] = new BezierKnot(targetPoint + new Vector3(0, dropHeight, 0));

            // go down

            knots[2] = new BezierKnot(endPoint);

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

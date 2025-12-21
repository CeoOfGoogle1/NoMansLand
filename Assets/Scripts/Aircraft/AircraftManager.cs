using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class AircraftManager : MonoBehaviour
{
    public static AircraftManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] GameObject aircraftPath;
    [SerializeField] GameObject f15Prefab;
    [Header("Parameters")]
    [SerializeField] float jetSpawnRadius = 4000;
    [SerializeField] float landingAngleOffset = 40f;
    [Header("Positions (TEMPORARY)")]
    [SerializeField] Transform spawnPosTransform;
    [SerializeField] Transform endPosTransform;
    [SerializeField] Transform targetPosTransform;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // включи, если менеджер должен жить между сценами
    }

    public void LaunchAircraft()
    {
        var target = FindAnyObjectByType<MovementController>().transform.position;

        // случайный угол старта
        float startAngle = Random.Range(0f, 360f);

        // противоположная сторона + рандомное отклонение
        float endAngle = startAngle + 180f + Random.Range(-landingAngleOffset, landingAngleOffset);

        Vector3 startPos = GetPointOnCircle(target, jetSpawnRadius, startAngle);
        Vector3 endPos   = GetPointOnCircle(target, jetSpawnRadius, endAngle);

        Instantiate(f15Prefab)
            .GetComponent<Aircraft>()
            .Initialize(
                AircraftBehaviour.BombDropping,
                startPos,
                endPos,
                target
            );
    }

    Vector3 GetPointOnCircle(Vector3 center, float radius, float angleDeg)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;

        return center + new Vector3(
            Mathf.Cos(angleRad) * radius,
            0f, // если нужно лететь на другой высоте — меняй тут
            Mathf.Sin(angleRad) * radius
        );
    }

    public GameObject GetAircraftPathPrefab()
    {
        return aircraftPath;
    }

}

public enum AircraftBehaviour
{
    Flying,
    BombDropping,
    BombDiving
}

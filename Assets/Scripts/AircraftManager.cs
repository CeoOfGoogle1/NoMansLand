using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class AircraftManager : MonoBehaviour
{
    public static AircraftManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] GameObject aircraftPath;
    [SerializeField] GameObject f15Prefab;
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

    void Start()
    {
        //TEMP
        Instantiate(f15Prefab)
            .GetComponent<Aircraft>()
            .Initialize(AircraftBehaviour.BombDropping, spawnPosTransform.position, endPosTransform.position, targetPosTransform.position);
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

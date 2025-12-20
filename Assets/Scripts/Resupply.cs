using UnityEngine;

public class Resupply : MonoBehaviour
{
    public float resupplyInterval;
    private float currentIntervalTime;

    void Update()
    {
        currentIntervalTime += Time.deltaTime;

        if (currentIntervalTime >= resupplyInterval)
        {
            ResupplyChanceRoll();
            currentIntervalTime = 0f;
        }
    }

    void ResupplyChanceRoll()
    {
        // do super cool advanced chance stuff later

        SpawnResupply();
    }

    void SpawnResupply()
    {
        
    }
}

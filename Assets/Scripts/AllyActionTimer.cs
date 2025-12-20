using UnityEngine;
using System;

public class AllyActionTimer : MonoBehaviour
{
    public enum AllyAction { None, Recon, Movements }
    public AllyAction allyAction = AllyAction.None;
    public float allyActionInterval;
    private float currentIntervalTime = 0f;
    
    void Update()
    {
        currentIntervalTime += Time.deltaTime;

        if (currentIntervalTime >= allyActionInterval)
        {
            AllyActionChanceRoll();
            currentIntervalTime = 0f;
        }
    }

    void AllyActionChanceRoll()
    {
        // do super cool advanced chance stuff later

        allyAction = (AllyAction)Enum.GetValues(typeof(AllyAction)).GetValue(
        UnityEngine.Random.Range(0, Enum.GetValues(typeof(AllyAction)).Length));

        CommitAllyAction();
    }

    void CommitAllyAction()
    {
        if (allyAction == AllyAction.None) {} // 8===> ({})
        if (allyAction == AllyAction.Recon) {}
        if (allyAction == AllyAction.Movements) {}
    }
}

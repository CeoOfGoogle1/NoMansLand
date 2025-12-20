using UnityEngine;
using System;

public class MissionTimer : MonoBehaviour
{
    public enum MissionType { None, Recon, Drive, Place, Loot, Dispose, Shoot, Sabotage, Assassinate, Defend, Attack }
    public MissionType missionType = MissionType.None;
    public float missionInterval;
    private float currentIntervalTime = 0f;
    
    void Update()
    {
        currentIntervalTime += Time.deltaTime;

        if (currentIntervalTime >= missionInterval)
        {
            MissionChanceRoll();
            currentIntervalTime = 0f;
        }
    }

    void MissionChanceRoll()
    {
        // do super cool advanced chance stuff later

        missionType = (MissionType)Enum.GetValues(typeof(MissionType)).GetValue(
        UnityEngine.Random.Range(0, Enum.GetValues(typeof(MissionType)).Length));

        SelectMission();
    }

    void SelectMission()
    {
        if (missionType == MissionType.None) {}
        if (missionType == MissionType.Recon) {}
        if (missionType == MissionType.Drive) {}
        if (missionType == MissionType.Place) {}
        if (missionType == MissionType.Loot) {}
        if (missionType == MissionType.Dispose) {}
        if (missionType == MissionType.Shoot) {}
        if (missionType == MissionType.Sabotage) {}
        if (missionType == MissionType.Assassinate) {}
        if (missionType == MissionType.Defend) {}
        if (missionType == MissionType.Attack) {}
    }
}

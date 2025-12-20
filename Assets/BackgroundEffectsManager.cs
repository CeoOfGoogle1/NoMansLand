using System;
using UnityEngine;

public class BackgroundEffectsManager : MonoBehaviour
{
    [Header("Battles")]
    [SerializeField] float battleTimerTarget;
    [Header("Jets")]
    [SerializeField] float jetTimerTarget;
    [SerializeField] float jetLaunchPercent;
    [Header("Fire")]
    [SerializeField] float fireTimerTarget;
    [Header("Nuke")]
    [SerializeField] float nukeTimerTarget;

    float battleTimer = 0f;
    float jetTimer = 0f;
    float fireTimer = 0f;
    float nukeTimer = 0f;

    void Update()
    {
        UpdateBattleTimer();
        UpdateJetTimer();
        UpdateFireTimer();
        UpdateNukeTimer();
    }

    private void UpdateBattleTimer()
    {
        battleTimer += Time.deltaTime;
        if (battleTimer >= battleTimerTarget)
        {
            battleTimer = 0;

            
        }
        
    }

    private void UpdateJetTimer()
    {
        jetTimer += Time.deltaTime;
        if(jetTimer >= jetTimerTarget)
        {
            jetTimer = 0;

            if (UnityEngine.Random.Range(0f, 1f) < jetLaunchPercent / 100)
            {
                AircraftManager.Instance.LaunchAircraft();
            }
        }

    }

    private void UpdateFireTimer()
    {
        fireTimer += Time.deltaTime;
        if(fireTimer >= fireTimerTarget)
        {
            fireTimer = 0;
            
        }

    }

    private void UpdateNukeTimer()
    {
        nukeTimer += Time.deltaTime;

        if(nukeTimer >= nukeTimerTarget)
        {
            nukeTimer = 0;
        }

    }
}

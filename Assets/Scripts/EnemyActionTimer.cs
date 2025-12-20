using UnityEngine;
using System;

public class EnemyActionTimer : MonoBehaviour
{
    public enum EnemyAction { None, IndirectFire, DroneStrike, Airstrike, Recon, Assault }
    public EnemyAction enemyAction = EnemyAction.None;
    public float enemyActionInterval;
    private float currentIntervalTime = 0f;
    
    void Update()
    {
        currentIntervalTime += Time.deltaTime;

        if (currentIntervalTime >= enemyActionInterval)
        {
            EnemyActionChanceRoll();
            currentIntervalTime = 0f;
        }
    }

    void EnemyActionChanceRoll()
    {
        // do super cool advanced chance stuff later

        enemyAction = (EnemyAction)Enum.GetValues(typeof(EnemyAction)).GetValue(
        UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyAction)).Length));

        CommitEnemyAction();
    }

    void CommitEnemyAction()
    {
        if (enemyAction == EnemyAction.None) {} // 8===> ({})
        if (enemyAction == EnemyAction.IndirectFire) {}
        if (enemyAction == EnemyAction.DroneStrike) {}
        if (enemyAction == EnemyAction.Airstrike) {}
        if (enemyAction == EnemyAction.Recon) {}
        if (enemyAction == EnemyAction.Assault) {}
    }
}

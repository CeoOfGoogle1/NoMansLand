using System.Collections.Generic;
using UnityEngine;

public class SoldierSurroundingCheck : MonoBehaviour
{
    //private GameObject[] coverPoints;
    private List<GameObject> enemies;

    void Start()
    {
        enemies = new List<GameObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered by: " + other.name);
        if (other.CompareTag("Soldier"))
        {   
            Debug.Log("Soldier detected: " + other.name);
            other.TryGetComponent<SoldierAI>(out SoldierAI soldierAI);
            if (soldierAI != null)
            {
                if (soldierAI.GetSoldierSide() != GetComponentInParent<SoldierAI>().GetSoldierSide())
                {
                    Debug.Log("Enemy soldier detected: " + other.name);
                    enemies.Add(soldierAI.gameObject);
                }
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (GetComponentInParent<SoldierAI>().GetSoldierSide() == SoldierSide.BadGuys)
            {
                enemies.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Soldier"))
        {
            other.TryGetComponent<SoldierAI>(out SoldierAI soldierAI);
            if (soldierAI != null)
            {
                if (soldierAI.GetSoldierSide() != GetComponentInParent<SoldierAI>().GetSoldierSide())
                {
                    enemies.Remove(soldierAI.gameObject);
                }
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (GetComponentInParent<SoldierAI>().GetSoldierSide() == SoldierSide.BadGuys)
            {
                enemies.Remove(other.gameObject);
            }
        }
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    public bool HasEnemies()
    {
        return enemies.Count > 0;
    }

    public GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, currentPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}

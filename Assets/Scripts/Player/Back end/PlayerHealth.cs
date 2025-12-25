using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float legHealthSpeedFactor = 1f;
    public float legHealthStaminaFactor = 1f;
    public bool isDead = false;
    public bool passiveHeal = true;
    public bool isThirsty = false;
    public bool isHungry = false;
    public bool isTired = false;
    private float passiveHealCurrentTime;
    private float lightBleedCurrentDeathTime;
    private float heavyBleedCurrentDeathTime;
    private float passiveHealTime = 300f; // 5 minutes
    private float lightBleedDeathTime = 600f; // 10 minutes
    private float heavyBleedDeathTime = 60f; // 1 minute
    private List<Bodypart> bodyparts = new List<Bodypart>();
    private Dictionary<Bodypart.BodypartType, Bodypart> bodypartsByType = new Dictionary<Bodypart.BodypartType, Bodypart>();


    void Awake()
    {
        bodyparts.AddRange(GetComponentsInChildren<Bodypart>());

        bodypartsByType.Clear();

        foreach (var part in bodyparts)
        {
            bodypartsByType[part.bodypartType] = part;
        }
    }

    void Update()
    {
        if (isDead) return;

        UpdatePassiveHeal();
        UpdateBleeding();
        GetLowestLegHealth();
        CheckDeath();
    }

    public void AddHealthToAll(float amount)
    {
        foreach (var part in bodyparts)
        {
            part.Heal(amount);
        }
    }

    public void AddHealthToLowest(float amount)
    {
        Bodypart lowest = null;

        foreach (var part in bodyparts)
        {
            if (lowest == null || part.currentHealth < lowest.currentHealth)
                lowest = part;
        }

        if (lowest != null)
            lowest.Heal(amount);
    }

    void UpdatePassiveHeal()
    {
        if (!passiveHeal || isThirsty || isHungry || isTired) return;

        foreach (var part in bodyparts)
        {
            if (part.currentHealth <= 8) return;
        }

        passiveHealCurrentTime += Time.deltaTime;

        if (passiveHealCurrentTime >= passiveHealTime)
        {
            passiveHealCurrentTime = 0f;
            AddHealthToAll(1f);
        }
    }

    void UpdateBleeding()
    {
        bool heavy = false;
        bool light = false;

        foreach (var part in bodyparts)
        {
            if (part.currentHealth < 3) heavy = true;
            else if (part.currentHealth < 6) light = true;
        }

        if (heavy)
        {
            heavyBleedCurrentDeathTime += Time.deltaTime;
            lightBleedCurrentDeathTime = 0f;
        }
        else if (light)
        {
            lightBleedCurrentDeathTime += Time.deltaTime;
            heavyBleedCurrentDeathTime = 0f;
        }
        else
        {
            heavyBleedCurrentDeathTime = 0f;
            lightBleedCurrentDeathTime = 0f;
        }

        if (heavyBleedCurrentDeathTime >= heavyBleedDeathTime ||
            lightBleedCurrentDeathTime >= lightBleedDeathTime)
        {
            Die();
        }
    }

    public void GetLowestLegHealth()
    {
        Bodypart leftLeg  = GetBodypart(Bodypart.BodypartType.LeftLeg);
        Bodypart rightLeg = GetBodypart(Bodypart.BodypartType.RightLeg);
        float weakestLegCurrentHealth = Mathf.Min(leftLeg.currentHealth, rightLeg.currentHealth);

        legHealthSpeedFactor = 1 * (weakestLegCurrentHealth / 10);
    }

    void CheckDeath()
    {
        if (GetBodypart(Bodypart.BodypartType.Head).currentHealth <= 0 ||
        GetBodypart(Bodypart.BodypartType.Torso).currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Player has died.");
    }

    private Bodypart GetBodypart(Bodypart.BodypartType type)
    {
        if (bodypartsByType.TryGetValue(type, out Bodypart part))
            return part;

        Debug.LogError($"Bodypart {type} not found!");
        return null;
    }
}

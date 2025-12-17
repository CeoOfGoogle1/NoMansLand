using System.Runtime.Serialization;
using UnityEngine;

public class Bodypart : MonoBehaviour
{
    public enum BodypartType
    {
        Head,
        Torso,
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }
    public enum BleedState
    {
        None = 0,
        Light = 1,
        Heavy = 2
    }
    public float maxHealth = 10;
    public float currentHealth = 10;
    public BodypartType bodypartType;
    public BleedState bleedState = BleedState.None;
    public Collider bodypartCollider { get; private set; }

    void Awake()
    {
        bodypartCollider = GetComponent<Collider>();
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateBleedState();
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void UpdateBleedState()
    {
        if (currentHealth < 4) bleedState = BleedState.Heavy;
        else if (currentHealth < 8) bleedState = BleedState.Light;
        else bleedState = BleedState.None;
    }
}

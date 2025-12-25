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

    public float maxHealth = 10;
    public float currentHealth = 10;
    public BodypartType bodypartType;
    public Collider bodypartCollider { get; private set; }

    void Awake()
    {
        bodypartCollider = GetComponent<Collider>();
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}

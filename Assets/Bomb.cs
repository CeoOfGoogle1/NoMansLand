using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject explosionPrefab;
    public float effectDuration = 10f;

    public void Initialize(Vector3 initialVelocity)
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = initialVelocity;

        Debug.Log($"Initial Velocity is {initialVelocity}");

        StartCoroutine(DelayedSound(3f));
    }

    IEnumerator DelayedSound(float delay) {
    yield return new WaitForSeconds(delay);
    AudioManager.instance.Play("whistle", transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        AudioManager.instance.Play("bomb", transform);

        Explode();
    }

    public void Explode()
    {
        // Spawn VFX at bomb position, parented to null (world space)
        GameObject vfxInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);
        
        // Destroy bomb immediately
        Destroy(gameObject);
        
        // Clean up VFX after it finishes
        Destroy(vfxInstance, effectDuration);
    }
}

using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    Vector3 velocity;
    public GameObject explosionPrefab;
    public float effectDuration = 10f;

    public void Initialize(Vector3 initialVelocity)
    {
        velocity = initialVelocity;

        Debug.Log($"Initial Velocity is {initialVelocity}");

        StartCoroutine(DelayedSound(3f));
    }

    void Update()
    {
        ApplyGravity();

        ApplyAirRes();

        transform.position += velocity * Time.deltaTime;
    }

    private void ApplyAirRes()
    {
        //Todo include air res in ballistics

        float factor = Mathf.Pow(0.98f, Time.deltaTime);
        velocity.x *= factor;
        velocity.z *= factor;
    }

    private void ApplyGravity()
    {
        velocity.y += -9.81f * Time.deltaTime;
    }

    IEnumerator DelayedSound(float delay) {
    yield return new WaitForSeconds(delay);
    AudioManager.instance.Play("whistle", transform);
    }


    void OnTriggerEnter(Collider other)
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

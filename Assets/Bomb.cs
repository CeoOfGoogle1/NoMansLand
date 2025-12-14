using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb;
    public VisualEffect fx;

    public void Initialize(Vector3 initialVelocity)
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = initialVelocity;

        Debug.Log($"Initial Velocity is {initialVelocity}");

        StartCoroutine(DelayedSound(3f));
    }

    IEnumerator DelayedSound(float delay) {
    yield return new WaitForSeconds(delay);
    AudioManager.instance.Play("whistle", transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        AudioManager.instance.Play("bomb", transform.position);

        Debug.Log("Bomb collided with " + collision.gameObject.name);

        Destroy(gameObject);

        fx.SendEvent("OnBomb");
        fx.Play();
    }
}

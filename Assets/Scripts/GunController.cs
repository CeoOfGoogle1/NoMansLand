using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun")]
    public Transform gun;
    public Vector3 gunPosition = new Vector3(0.25f, -0.35f, 0.5f);
    public Vector3 aimedGunPosition = new Vector3(0f, -0.15f, 0.5f);
    public float aimSpeed = 5f;

    private bool isAiming;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gun.localPosition = gunPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAimInput();
    }

    void HandleAimInput()
    {
        if (Input.GetMouseButtonDown(1))
            isAiming = !isAiming;

        gun.localPosition = Vector3.Lerp(
            gun.localPosition,
            isAiming ? aimedGunPosition : gunPosition,
            Time.deltaTime * aimSpeed
        );
    }
}

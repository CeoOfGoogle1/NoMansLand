using UnityEngine;

public class GunController : MonoBehaviour
{
    Gun gun;

    public void SetGun(Gun newGun)
    {
        gun = newGun;
    }

    public void Clear()
    {
        gun = null;
    }

    void Update()
    {
        if (gun == null) return;

        HandleAim();

        if (Input.GetMouseButton(0) && gun != null)
            Fire();

        if (Input.GetMouseButton(3) && gun != null)
            CockGun();

        if (Input.GetKeyDown(KeyCode.R) && gun != null)
            Reload();
    }

    public void HandleAim()
    {
        gun?.HandleAim();
    }

    public void Fire()
    {
        gun?.Fire();
    }

    public void CockGun()
    {
        gun?.CockGun();
    }

    public void Reload()
    {
        gun?.Reload();
    }
}

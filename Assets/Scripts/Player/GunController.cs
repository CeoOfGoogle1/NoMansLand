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
}

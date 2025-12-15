using UnityEngine;

public class GunData : ScriptableObject
{
    public string gunName;
    public float damage;
    public float initialVelocity;
    public float fireRate;
    public int magazineSize;
    public float reloadTime;
    public float aimTime;
    public float recoilPower;
    public float weight;
    public GameObject bulletPrefab;
}

using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Positions")]
    public Vector3 hipPosition;
    public Vector3 aimedPosition;
    public Transform barrel;

    [Header("Gun Stats")]
    public float damage;
    public float initialVelocity;
    public float fireRate;
    public float reloadTime;
    public float aimTime;
    public float recoilPower;
    public float jamChance;
    public AmmoType ammoType;
    public GameObject bulletPrefab;

    [Header("Magazines")]
    public List<Magazine> mags = new();
    public Magazine loadedMag = null;
    public int magCapacity;
    public float magWeight;

    bool isAiming;
    bool chambered;

    Vector3 currentTargetPos;

    void Awake()
    {
        transform.localPosition = hipPosition;
        currentTargetPos = hipPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            currentTargetPos,
            Time.deltaTime * aimTime
        );
    }

    // ---------- Aiming ----------

    public void HandleAim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
            currentTargetPos = isAiming ? aimedPosition : hipPosition;
        }
    }

    // ---------- Firing ----------

    public void Fire()
    {
        if (!chambered) return;

        SpawnBullet();
        chambered = false;

        TryChamberFromMag();
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            barrel.position,
            barrel.rotation
        );

        if (bullet.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = barrel.forward * initialVelocity;
    }

    // ---------- Reloading ----------

    public void Reload()
    {
        Magazine best = GetBestMagazine();
        if (best == null) return;

        if (loadedMag != null)
            mags.Add(loadedMag);

        loadedMag = best;
        mags.Remove(best);
    }

    Magazine GetBestMagazine()
    {
        Magazine best = null;
        int mostAmmo = -1;

        foreach (var mag in mags)
        {
            if (mag.ammoType != ammoType) continue;

            if (mag.currentAmmo > mostAmmo)
            {
                best = mag;
                mostAmmo = mag.currentAmmo;
            }
        }

        return best;
    }

    // ---------- Bolt / Cock ----------

    public void CockGun()
    {
        if (loadedMag.currentAmmo > 0)
        {
            loadedMag.currentAmmo--;
            chambered = true;
        }
        else if (chambered)
        {
            chambered = false;
        }
        else
        {
            // empty ahh
        }
    }

    void TryChamberFromMag()
    {
        if (loadedMag == null) return;
        if (chambered) return;
        if (loadedMag.currentAmmo <= 0) return;

        loadedMag.currentAmmo--;
        chambered = true;
    }

    // ---------- Weight ----------

    public float GetTotalWeight()
    {
        float total = 0f;

        if (loadedMag != null)
            total += loadedMag.weight;

        foreach (var mag in mags)
            total += mag.weight;

        return total;
    }

    // Magazines

    public void RefillAllMags()
    {
        foreach (var mag in mags)
        {
            mag.currentAmmo = mag.capacity;
        }
    }

    public void AddMag(int ammoAmount)
    {
        Magazine mag = new Magazine();

        mag.ammoType = ammoType;
        mag.capacity = magCapacity;
        mag.currentAmmo = ammoAmount;
        mag.weight = magWeight;

        mags.Add(mag);
    }

    public void RemoveWorstMag()
    {
        if (mags.Count == 0) return;

        Magazine worst = mags[0];

        foreach (var mag in mags)
        {
            if (mag.currentAmmo < worst.currentAmmo)
            worst = mag;
        }

        mags.Remove(worst);
    }
}

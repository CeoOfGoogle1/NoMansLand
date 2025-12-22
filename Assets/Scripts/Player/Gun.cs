using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Positions")]
    public Vector3 hipPosition;
    public Vector3 aimedPosition;
    public Vector3 barrelPosition;

    [Header("Gun Stats")]
    public float damage;
    public float initialVelocity;
    public float firerate;
    public float reloadTime;
    public float aimTime;
    public float recoilPower;
    public float jamChance;
    public AmmoType ammoType;
    public GameObject bulletPrefab;
    public bool chambered;
    public bool semiAuto;

    [Header("Magazines")]
    public List<Magazine> mags = new();
    public Magazine loadedMag = null;
    public int magCapacity;
    public float magWeight;

    bool isAiming;
    float nextFireTime = 0f;

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
            Time.deltaTime * 10 / aimTime
        );

        if (Input.GetMouseButtonDown(1))
        {
            Aim();
        }

        if (Input.GetKeyDown(KeyCode.R)) Reload();

        if (Input.GetMouseButtonDown(2)) CockGun();
        
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && semiAuto)
        {
            Fire();
            nextFireTime = Time.time + (60f / firerate);
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && !semiAuto)
        {
            Fire();
            nextFireTime = Time.time + (60f / firerate);
        }
    }

    // ---------- Aiming ----------

    public void Aim()
    {
        isAiming = !isAiming;
        currentTargetPos = isAiming ? aimedPosition : hipPosition;
    }

    // ---------- Firing ----------

    public void Fire()
    {
        Debug.Log("trying to fire");

        if (!chambered) return;

        SpawnBullet();

        if (loadedMag != null && loadedMag.currentAmmo > 0)
        {
            loadedMag.currentAmmo--;
        }
        else
        {
            chambered = false;
        }

        Debug.Log("fired");
    }

    void SpawnBullet()
    {
        Vector3 spawnPosition = transform.TransformPoint(barrelPosition);

        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            transform.rotation
        );

        if (bullet.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = transform.forward * initialVelocity;
        Debug.Log("spawned bullet");
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
        Debug.Log("reloaded");
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
        Debug.Log("trying to cock");
        if (loadedMag != null && loadedMag.currentAmmo > 0)
        {
            loadedMag.currentAmmo--;
            chambered = true;
        }
        else
        {
            chambered = false;
        }
        Debug.Log("cocked");
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
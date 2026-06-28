using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public AimDownSight aimDownSight;

    [Header("Ammo")]
    public int magazineSize = 30;
    public int currentAmmo = 30;
    public int reserveAmmo = 120;

    public float reloadTime = 2f;

    private bool isReloading;

    // For AmmoGlitch.cs
    public bool IsReloading
    {
        get { return isReloading; }
    }

    [Header("Shooting")]
    public float bulletSpeed = 80f;
    public float fireRate = 0.15f;

    private float nextFireTime;

    [Header("UI")]
    public TMP_Text ammoText;

    private void Start()
    {
        currentAmmo = magazineSize;
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetMouseButton(0) &&
            Time.time >= nextFireTime)
        {
            Shoot();

            nextFireTime =
                Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            Reload();
            return;
        }

        currentAmmo--;

        UpdateAmmoUI();

        if (aimDownSight != null)
        {
            aimDownSight.SnapToAim();
        }

        Ray ray =
            playerCamera.ViewportPointToRay(
                new Vector3(0.5f, 0.5f, 0));

        Vector3 targetPoint;

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            1000f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint =
                ray.GetPoint(1000f);
        }

        Vector3 direction =
            (targetPoint -
             firePoint.position)
            .normalized;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.LookRotation(direction));

        Rigidbody rb =
            bullet.GetComponent<Rigidbody>();

        rb.linearVelocity =
            direction * bulletSpeed;
    }

    private void Reload()
    {
        if (isReloading)
            return;

        if (currentAmmo == magazineSize)
            return;

        if (reserveAmmo <= 0)
            return;

        StartCoroutine(
            ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        if (ammoText != null)
        {
            ammoText.text = "RELOADING...";
        }

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(
            reloadTime);

        int ammoNeeded =
            magazineSize - currentAmmo;

        int ammoToLoad =
            Mathf.Min(
                ammoNeeded,
                reserveAmmo);

        currentAmmo += ammoToLoad;

        reserveAmmo -= ammoToLoad;

        isReloading = false;

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text =
                currentAmmo +
                " / " +
                reserveAmmo;
        }
    }
}
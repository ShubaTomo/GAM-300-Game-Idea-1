using UnityEngine;
using TMPro;
using System.Collections;

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
        if (GameManagerScript.isDead)
            return; 

        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R) &&
            currentAmmo < magazineSize &&
            reserveAmmo > 0)
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
            if (ammoText != null)
            {
                if (reserveAmmo > 0)
                {
                    ammoText.text = "EMPTY";
                }
                else
                {
                    ammoText.text = "NO AMMO";
                }
            }

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
                Quaternion.LookRotation(
                    direction));

        Bullet bulletScript =
            bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.owner =
                transform.root.gameObject;
        }

        Collider bulletCollider =
            bullet.GetComponent<Collider>();

        Collider playerCollider =
            GetComponentInParent<Collider>();

        if (bulletCollider != null &&
            playerCollider != null)
        {
            Physics.IgnoreCollision(
                bulletCollider,
                playerCollider);
        }

        Rigidbody rb =
            bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                direction * bulletSpeed;
        }
    }

    private void Reload()
    {
        if (isReloading)
            return;

        if (currentAmmo ==
            magazineSize)
            return;

        if (reserveAmmo <= 0)
            return;

        StartCoroutine(
            ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        float timer = 0f;

        while (timer < reloadTime)
        {
            timer += Time.deltaTime;

            if (ammoText != null)
            {
                int dots =
                    (int)(Time.time * 3f)
                    % 4;

                ammoText.text =
                    "RELOAD" +
                    new string('.', dots);
            }

            yield return null;
        }

        int ammoNeeded =
            magazineSize -
            currentAmmo;

        int ammoToLoad =
            Mathf.Min(
                ammoNeeded,
                reserveAmmo);

        currentAmmo +=
            ammoToLoad;

        reserveAmmo -=
            ammoToLoad;

        isReloading = false;

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText == null)
            return;

        if (currentAmmo <= 0 &&
            reserveAmmo <= 0)
        {
            ammoText.text =
                "NO AMMO";
        }
        else if (currentAmmo <= 0)
        {
            ammoText.text =
                "EMPTY";
        }
        else
        {
            ammoText.text =
                currentAmmo +
                " | " +
                reserveAmmo;
        }
    }
}
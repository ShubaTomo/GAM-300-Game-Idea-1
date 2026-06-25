using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera;

    public Transform firePoint;

    public GameObject bulletPrefab;

    public AimDownSight aimDownSight;

    public float bulletSpeed = 80f;
    public float fireRate = 0.15f;

    private float nextFireTime;

    private void Update()
    {
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
        // Instantly move gun to ADS position
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
}
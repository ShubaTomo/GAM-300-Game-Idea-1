using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [HideInInspector]
    public Transform player;

    [Header("References")]
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Weapon Settings")]
    public float projectileSpeed = 40f;
    public float attackCooldown = 1f;

    private bool canShoot = true;

    public void TryShoot()
    {
        if (!canShoot || player == null)
            return;

        Shoot();

        canShoot = false;
        Invoke(nameof(ResetCooldown), attackCooldown);
    }

    private void Shoot()
    {
        Vector3 targetPosition = player.position + Vector3.up;
        Vector3 direction = (targetPosition - firePoint.position).normalized;

        GameObject bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = direction * projectileSpeed;
        }

        Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider enemyCollider = GetComponent<Collider>();

        if (bulletCollider != null && enemyCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, enemyCollider);
        }
    }

    private void ResetCooldown()
    {
        canShoot = true;
    }
}
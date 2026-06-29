using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 3f;

    // Who fired this bullet
    [HideInInspector]
    public GameObject owner;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore the shooter
        if (collision.transform.root.gameObject == owner)
            return;

        Debug.Log("Hit: " + collision.gameObject.name);

        //--------------------------------------------------
        // Damage Player
        //--------------------------------------------------

        PlayerHealth playerHealth =
            collision.transform
                     .GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        //--------------------------------------------------
        // Damage Enemy
        //--------------------------------------------------

        Enemy enemy =
            collision.transform
                     .GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(
                Mathf.RoundToInt(damage));

            Destroy(gameObject);
            return;
        }

        //--------------------------------------------------
        // Hit wall, floor, etc.
        //--------------------------------------------------

        Destroy(gameObject);
    }
}
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private Animator animator;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Vector2 shootDirection = Vector2.left;
    [SerializeField] private float bulletSpeed = 5f;

    private float fireTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= 1f / fireRate)
        {
           
            fireTimer = 0f;
            animator.SetBool("Shooting", true);
        }
        else
        {
            animator.SetBool("Shooting", false);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Evitar colisión con el enemigo
        Collider2D enemyCollider = GetComponent<Collider2D>();
        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        if (enemyCollider != null && bulletCollider != null)
        {
            Physics2D.IgnoreCollision(enemyCollider, bulletCollider);
        }

        // Configurar la bala si tiene el script EnemyProjectile
        EnemyProjectile enemyBullet = bullet.GetComponent<EnemyProjectile>();
        if (enemyBullet != null)
        {
            enemyBullet.SetDirection(shootDirection);
            enemyBullet.speed = bulletSpeed;
        }
        else
        {
            // Si no tiene EnemyProjectile, usar Rigidbody2D por fallback
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDirection.normalized * bulletSpeed;
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int maxProjectiles = 3;
    [SerializeField] private float chargeTime = 1.5f;

    private float lastShotTime;
    private int currentProjectiles;
    private float fireButtonHeldTime;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            fireButtonHeldTime += Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire1") && Time.time >= lastShotTime + fireRate && currentProjectiles < maxProjectiles)
        {
            bool isCharged = fireButtonHeldTime >= chargeTime;
            Shoot(isCharged);
            fireButtonHeldTime = 0f;
        }
    }

    private void Shoot(bool isCharged)
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 shootDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Projectile projScript = projectile.GetComponent<Projectile>();

        if (rb != null && projScript != null)
        {
            float speed = 10f;

            if (isCharged)
            {
                projScript.damage *= 2;
                projScript.lifetime *= 2;
                speed *= 2;
            }

            rb.velocity = shootDir * speed;

            Destroy(projectile, projScript.lifetime);
            StartCoroutine(DecreaseProjectileCountAfter(projScript.lifetime));
        }

        lastShotTime = Time.time;
        currentProjectiles++;
    }

    private IEnumerator DecreaseProjectileCountAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentProjectiles--;
    }
}

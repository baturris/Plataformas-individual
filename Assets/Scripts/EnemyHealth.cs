using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;
    public GameObject healthPickupPrefab; // Asigna en el inspector

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DropHealthPickup(); // nuevo
            Destroy(gameObject);
        }
    }

    private void DropHealthPickup()
    {
        if (healthPickupPrefab != null && Random.value < 0.3f) // 30% de probabilidad
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }
    }
}
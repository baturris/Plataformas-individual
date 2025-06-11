using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 3f;

    public enum ProjectileOwner { Player, Enemy }
    public ProjectileOwner owner;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignorar si colisiona con el dueño
        if (owner == ProjectileOwner.Player && collision.CompareTag("Player")) return;
        if (owner == ProjectileOwner.Enemy && collision.CompareTag("Enemy")) return;

        // Dañar enemigo
        if (owner == ProjectileOwner.Player)
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        // Dañar jugador
        if (owner == ProjectileOwner.Enemy)
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        // Si no es cámara, destruir
        if (!collision.CompareTag("Camera"))
        {
            Destroy(gameObject);
        }
    }
}

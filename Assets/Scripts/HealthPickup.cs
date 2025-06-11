using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log("Health pickup collected. Healed " + healAmount);
            Destroy(gameObject);
        }
    }
}

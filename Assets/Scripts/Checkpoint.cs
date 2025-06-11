using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int healAmount = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.SetCheckpoint(transform.position);
            player.Heal(healAmount);
            Debug.Log("Checkpoint reached. Position saved and player healed.");
        }
    }
}

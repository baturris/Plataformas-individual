using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Invulnerability")]
    public float invulnerabilityTime = 1f;
    private bool isInvulnerable = false;

    private Vector3 respawnPosition;

    private void Start()
    {
        currentHealth = maxHealth;
        respawnPosition = transform.position; // Posición inicial por defecto
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;
        Debug.Log("Player took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityFrames());
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed. Current health: " + currentHealth);
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
        Debug.Log("Checkpoint saved at: " + respawnPosition);
    }

    private void Die()
    {
        Debug.Log("Player Died! Respawning...");
        Respawn();
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
        currentHealth = maxHealth;
        Debug.Log("Player respawned with full health.");
    }

    private IEnumerator InvulnerabilityFrames()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }
}

using UnityEngine;

public class KeyDoorUnlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Buscar la llave entre los hijos del jugador
            KeyFollowPlayer key = collision.GetComponentInChildren<KeyFollowPlayer>();

            if (key != null)
            {
                Destroy(key.gameObject);  // Elimina la llave
                Destroy(gameObject);      // Elimina la puerta (se "abre")
            }
            else
            {
                Debug.Log("Necesitas una llave para abrir esta puerta.");
            }
        }
    }
}

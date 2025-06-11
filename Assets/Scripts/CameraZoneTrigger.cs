using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private float zoomSize = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 center = GetComponent<Collider2D>().bounds.center;
            Debug.Log("Entró al trigger, centrando cámara en: " + center);
            cameraFollow.SetFixedPosition(center, zoomSize);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Saliendo del trigger, volviendo a seguir al jugador");
            cameraFollow.ResumeFollowing();
        }
    }
}
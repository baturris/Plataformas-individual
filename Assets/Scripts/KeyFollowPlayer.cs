using UnityEngine;

public class KeyFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 1f, 0f);
    [SerializeField] private float followSpeed = 5f;

    [Header("Floating Animation")]
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatFrequency = 2f;

    private Vector3 initialOffset;
    private float floatTimer;

    private void Start()
    {
        initialOffset = offset;
    }

    private void Update()
    {
        if (player == null) return;

        floatTimer += Time.deltaTime;

        float floatOffsetY = Mathf.Sin(floatTimer * floatFrequency) * floatAmplitude;
        Vector3 animatedOffset = new Vector3(initialOffset.x, initialOffset.y + floatOffsetY, initialOffset.z);

        Vector3 targetPosition = player.position + animatedOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null && collision.CompareTag("Player"))
        {
            SetPlayer(collision.transform);
            transform.SetParent(collision.transform); // Opcional: sigue al jugador como hijo
            GetComponent<Collider2D>().enabled = false; // Desactiva el trigger para no recogerla de nuevo
        }
    }
}

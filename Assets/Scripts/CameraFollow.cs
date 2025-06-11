using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;         // El jugador
    [SerializeField] private float normalSize = 5f;
    [SerializeField] private float zoomedOutSize = 10f;
    [SerializeField] private float zoomSmoothTime = 0.5f;

    private float currentZoomVelocity;
    private float targetSize;

    private bool followTarget = true;                  // Nuevo flag
    private Vector3 fixedPosition;                     // Nueva posición fija opcional

    private void Start()
    {
        targetSize = normalSize;
        Camera.main.orthographicSize = normalSize;
    }

    private void Update()
    {
        if (followTarget && target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, fixedPosition + offset, ref velocity, smoothTime);
        }

        Camera.main.orthographicSize = Mathf.SmoothDamp(
            Camera.main.orthographicSize,
            targetSize,
            ref currentZoomVelocity,
            zoomSmoothTime
        );
    }

    // Métodos públicos para control externo

    public void SetFixedPosition(Vector3 position, float zoomSize)
    {
        followTarget = false;
        fixedPosition = position;
        targetSize = zoomSize;
    }

    public void ResumeFollowing()
    {
        followTarget = true;
        targetSize = normalSize;
    }
}

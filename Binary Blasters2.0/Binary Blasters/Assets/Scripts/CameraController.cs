using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Objeto a ser seguido pela câmera
    public float smoothSpeed = 0.125f; // Velocidade suave de movimento da câmera
    public Vector3 offset; // Distância entre a câmera e o objeto seguido

    [SerializeField] Vector3 desiredPosition;

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}

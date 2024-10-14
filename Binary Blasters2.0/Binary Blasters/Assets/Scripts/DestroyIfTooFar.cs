using UnityEngine;

public class DestroyIfTooFar : MonoBehaviour
{
    public float maxDistanceFromCamera = 30f;

    private Transform mainCameraTransform;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, mainCameraTransform.position);
        if (distance > maxDistanceFromCamera)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistanceFromCamera);
    }
}

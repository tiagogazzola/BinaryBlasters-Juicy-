using UnityEngine;

public class DistanceToSpaceStation : MonoBehaviour
{
    public Transform spaceStation;
    public float distance;

    void Update()
    {
        distance = Vector2.Distance(transform.position, spaceStation.position);
    }
}
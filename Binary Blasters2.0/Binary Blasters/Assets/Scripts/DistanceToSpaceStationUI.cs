using UnityEngine;
using TMPro;

public class DistanceToSpaceStationUI : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public DistanceToSpaceStation DistanceToSpaceStation;

    private void Update()
    {
        float distance = DistanceToSpaceStation.distance;
        distanceText.text = "Distance:\n" + Mathf.Floor(distance).ToString();
    }
}

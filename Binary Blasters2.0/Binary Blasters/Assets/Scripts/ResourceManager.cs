using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;
    public ResourceMagnet resourceMagnet;

    private void Update()
    {
        float totalResources = resourceMagnet.totalResourcesInStorage;
        float maxResources = resourceMagnet.maxResourcesInStorage;
        int totalResourcesInt = Mathf.FloorToInt(totalResources); // Arredonda para baixo e converte para inteiro
        resourceText.text = "Resources\n" + totalResourcesInt.ToString() + " | " + maxResources;
    }
}

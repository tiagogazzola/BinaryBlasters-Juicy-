using UnityEngine;

public class ResourceValue : MonoBehaviour
{
    public float resourceValue;
    
    public float ResourceValuePerUnit
    {
        get { return resourceValue; }
        set { resourceValue = value; }
    }
}

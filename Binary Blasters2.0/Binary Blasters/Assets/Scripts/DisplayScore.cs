using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    //public ResourceMagnet resourceMagnet;

    private void Update()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.Score.ToString();
    }
}

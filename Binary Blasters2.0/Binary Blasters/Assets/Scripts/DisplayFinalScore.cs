using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DisplayFinalScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject shipA;
    public GameObject shipB;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }

        GameEnding();
    }

    void GameEnding()
    {
        int score = ScoreManager.Instance.Score;
        string formattedScore = string.Format("{0:N0}", score); // Adiciona espaços a cada três dígitos

        scoreText.text = "Score: " + formattedScore;
        shipA.SetActive(false);
        shipB.SetActive(false);
    }

    void RestartGame()
    {
        // Obtém o índice da cena atual
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calcula o índice da próxima cena
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // Carrega a próxima cena
        SceneManager.LoadScene(nextSceneIndex);
    }
}

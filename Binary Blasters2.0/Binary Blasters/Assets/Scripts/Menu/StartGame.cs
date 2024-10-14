using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Update()
    {
        // Verifica se alguma das teclas WASD ou as teclas de seta foi pressionada
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Obtém o índice da cena atual
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Calcula o índice da próxima cena
            int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

            // Carrega a próxima cena
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}

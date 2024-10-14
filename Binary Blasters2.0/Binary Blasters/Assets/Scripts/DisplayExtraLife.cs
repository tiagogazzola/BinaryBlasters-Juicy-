using UnityEngine;
using TMPro;

public class DisplayExtraLife : MonoBehaviour
{
    public GameObject lifeSymbol; // Referência ao GameObject do símbolo de vida
    public LifeManager lifeManager; // Referência ao script Respawn

    private void Start()
    {
        // Oculta o símbolo de vida no início do jogo
        lifeSymbol.SetActive(false);
    }

    private void Update()
    {
        // Verifica se o jogador tem uma vida no script Respawn
        if (lifeManager.life == 1)
        {
            // Mostra o símbolo de vida
            lifeSymbol.SetActive(true);
        }
        else
        {
            // Oculta o símbolo de vida
            lifeSymbol.SetActive(false);
        }
    }
}

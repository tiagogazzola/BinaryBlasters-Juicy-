using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int life; // Número de vidas do jogador
    private static LifeManager instance; // Referência estática ao gerenciador de vida

    private void Awake()
    {
        // Garante que apenas uma instância do gerenciador de vida exista
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o objeto ao trocar de cena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static LifeManager Instance
    {
        get { return instance; }
    }

    public void Update() 
    {
        if (life>1) 
        {
            life = 1;
        }
    }
}

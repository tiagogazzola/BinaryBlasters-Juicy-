using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimerController : MonoBehaviour
{
    public float totalTime; // Tempo total em segundos (10 minutos)
    private float currentTime; // Tempo atual
    private float LastSeconds = 10; // Ultimos segundos antes de acabar o jogo 
    private TextMeshProUGUI timerText; // Referência ao componente de texto

    public GameObject OffScreenIndicator; // Referência ao canva OffScreenIndicator
    public GameObject Score; // Referência ao canva Score
    public GameObject Store; // Referência ao canva Store
    public GameObject Resources; // Referência ao canva Resources
    public GameObject ExtraLife; // Referência ao canva ExtraLife
    public GameObject EtoShop; // Referência ao canva EtoShop
    public GameObject Timer; // Referência ao canva Timer
    public GameObject gameEnding; // Referência ao canva GameEnding
    public GameObject Spawner; // Referência ao Spawner

    public GameObject spaceStation1; // Referência ao objeto SpaceStation 1
    public GameObject spaceStation2; // Referência ao objeto SpaceStation 2
    public GameObject spaceStation3; // Referência ao objeto SpaceStation 3
    public GameObject spaceStation4; // Referência ao objeto SpaceStation 4
    public GameObject spaceStation5; // Referência ao objeto SpaceStation 5

    private bool spaceStation1Activated = false; // Verifica se SpaceStation 1 já foi ativada
    private bool spaceStation2Activated = false; // Verifica se SpaceStation 2 já foi ativada
    private bool spaceStation3Activated = false; // Verifica se SpaceStation 3 já foi ativada
    private bool spaceStation4Activated = false; // Verifica se SpaceStation 4 já foi ativada
    private bool spaceStation5Activated = false; // Verifica se SpaceStation 5 já foi ativada

    private float firstActivationTime; // Tempo em que a primeira SpaceStation deve ser ativada
    private float secondActivationTime; // Tempo em que a segunda SpaceStation deve ser ativada
    private float thirdActivationTime; // Tempo em que a terceira SpaceStation deve ser ativada
    private float fourthActivationTime; // Tempo em que a quarta SpaceStation deve ser ativada
    private float fifthActivationTime; // Tempo em que a quinta SpaceStation deve ser ativada

    public Volume globalVolume; // Reference to the Global Volume
    private ColorAdjustments colorAdjustments; // Reference to the Color Adjustments

    private bool SFXGameEndPlayed = true;

    private void Start()
    {
        currentTime = totalTime;
        timerText = GetComponent<TextMeshProUGUI>();

        // Define os tempos de ativação com base nas porcentagens do tempo total
        firstActivationTime = totalTime * 0.98f;
        secondActivationTime = totalTime * 0.8f;
        thirdActivationTime = totalTime * 0.6f;
        fourthActivationTime = totalTime * 0.4f;
        fifthActivationTime = totalTime * 0.2f;

        // Get the Color Adjustments component from the Global Volume
        globalVolume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        // Atualiza o tempo restante
        currentTime -= Time.deltaTime;

        // Atualiza o texto do timer
        UpdateTimerText();
        ActivateStations();
        EndGameEffects();
        EndGame();
    }

    private void UpdateTimerText()
    {
        // Converte o tempo para minutos e segundos
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        // Formata o texto do timer
        string timerString = minutes.ToString() + ":" + string.Format("{0:00}", seconds);

        // Atualiza o texto no objeto TextMeshProUGUI
        timerText.text = timerString;
    }

    private void ActivateStations()
    {
        // Verifica se o tempo restante é igual ou menor que o tempo de ativação da SpaceStation1
        if (currentTime <= firstActivationTime && !spaceStation1Activated)
        {
            spaceStation1.SetActive(true); //Ativa objeto SpaceStation
            spaceStation1Activated = true;
        }

        // Verifica se o tempo restante é igual ou menor que o tempo de ativação da SpaceStation2
        if (currentTime <= secondActivationTime && !spaceStation2Activated)
        {
            spaceStation2.SetActive(true); //Ativa objeto SpaceStation
            spaceStation2Activated = true;
        }

        // Verifica se o tempo restante é igual ou menor que o tempo de ativação da SpaceStation3
        if (currentTime <= thirdActivationTime && !spaceStation3Activated)
        {
            spaceStation3.SetActive(true); //Ativa objeto SpaceStation
            spaceStation3Activated = true;
        }

        // Verifica se o tempo restante é igual ou menor que o tempo de ativação da SpaceStation4
        if (currentTime <= fourthActivationTime && !spaceStation4Activated)
        {
            spaceStation4.SetActive(true); //Ativa objeto SpaceStation
            spaceStation4Activated = true;
        }

        // Verifica se o tempo restante é igual ou menor que o tempo de ativação da SpaceStation5
        if (currentTime <= fifthActivationTime && !spaceStation5Activated)
        {
            spaceStation5.SetActive(true); //Ativa objeto SpaceStation
            spaceStation5Activated = true;
        }
    }

    private void EndGameEffects()
    {
        if (currentTime <= LastSeconds)
        {
            // Calcula a diferença total de contraste
            float deltaContrast = 100f;

            // Calcula a mudança gradual de contraste por quadro
            float contrastChangePerFrame = deltaContrast / LastSeconds+0.3f;

            // Reduz gradualmente o valor do contraste
            colorAdjustments.contrast.value -= contrastChangePerFrame * Time.deltaTime;

            if (SFXGameEndPlayed)
            {
                SFXGameEndPlayed = false; // Play SFX only one time
                FindObjectOfType<AudioManager>().Play("GameEnd"); // SFX GameEnd
            }
        }
    }


    private void EndGame()
    {
        if (currentTime <= 0f)
        {
            // Desativa toda HUD
            Score.SetActive(false);
            Store.SetActive(false);
            Resources.SetActive(false);
            ExtraLife.SetActive(false);
            EtoShop.SetActive(false);
            Timer.SetActive(false);
            OffScreenIndicator.SetActive(false);

            // Desativa todas SpaceStations (Questão visual)
            spaceStation1.SetActive(false);
            spaceStation2.SetActive(false);
            spaceStation3.SetActive(false);
            spaceStation4.SetActive(false);
            spaceStation5.SetActive(false);

            //Ajusta brilho da tela
            colorAdjustments.contrast.value = 0;

            // Ativa o canva GameEnding
            gameEnding.SetActive(true);
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public string upgradeName;

    public UpgradeInfo[] upgrades; // Array de informações de cada upgrade

    private Button button;
    private TextMeshProUGUI buttonText;

    public GameObject ShipA;
    private ShotManager shotManager; //Bullet Damage (damage) - Bullet Size (size) - Bullet Speed (speed) - Shoot Interval (Shoot Interval) - Shoot Formation (Shoot Type)
    private DistanceJoint2D distanceJoint2D;

    public GameObject ShipB;
    private ResourceMagnet resourceMagnet; //Magnet Attaction (attractionForce) - Magnet Range (attractionRadius) - Max Cargo (maxResourcesInStorage)
    private ShipBMovement shipBMovement; //Speed (Velocity)

    public GameObject lifeManager;
    private LifeManager life; //Extra Life (life)

    public GameObject Spawner;
    private AsteroidSpawner asteroidSpawner; //More Enemies (MaxEnemyAlive) 

    private Camera mainCamera;

    private int currentLevel = 0; // Nível atual do upgrade

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // Registra a função OnButtonClick() para o evento de clique do botão
        button.onClick.AddListener(OnButtonClick);
    }

    private void Start()
    {
        // Atualiza o texto do botão com base no nível atual do upgrade
        UpdateButtonText();
    }

    private void Update()
    {
        // Verifica inicialmente se o jogador tem recursos suficientes para comprar o upgrade
        CheckButtonInteractable();
    }

    private void UpdateButtonText()
    {
        // Verifica se o upgrade atingiu o nível máximo
        if (currentLevel >= upgrades.Length - 1)
        {
            // Desativa o botão e mostra "MAX" no texto
            button.interactable = false;
            buttonText.text = string.Format("{0} (MAX)\nCurrent: {1}", upgrades[currentLevel].itemName, upgrades[currentLevel].currentUpgrade);
        }
        else
        {
            // Atualiza o texto do botão com base no nível atual e próximo
            UpgradeInfo currentUpgrade = upgrades[currentLevel];
            
            string buttonTextString = string.Format("{0} (${1})\nCurrent: {2} | Next: {3}",
                currentUpgrade.itemName, currentUpgrade.upgradeCost, currentUpgrade.currentUpgrade, currentUpgrade.nextUpgrade);

            buttonText.text = buttonTextString;
        }
    }

    private void CheckButtonInteractable()
    {
        bool canAffordUpgrade = false;

        if (currentLevel < upgrades.Length - 1)
        {  
            UpgradeInfo currentUpgrade = upgrades[currentLevel];
            
            // Verifica se o jogador tem recursos suficientes para comprar o upgrade
            resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
            canAffordUpgrade = resourceMagnet.totalResourcesInStorage >= currentUpgrade.upgradeCost;
        }

        if (upgradeName == "Extra Life")
        {
            lifeManager = GameObject.Find("LifeManager");
            life = lifeManager.GetComponent<LifeManager>();

            // Verifica se o jogador já comprou vidas e tem recursos suficientes para comprar
            canAffordUpgrade = life.life == 0 && resourceMagnet.totalResourcesInStorage >= upgrades[currentLevel].upgradeCost;
        }

        // Define a interatividade do botão com base na disponibilidade de recursos/vidas
        button.interactable = canAffordUpgrade;
    }

    private void OnButtonClick()
    {
        // Verifica se o upgrade não atingiu o nível máximo
        if (currentLevel < upgrades.Length - 1)
        {
            UpgradeInfo currentUpgrade = upgrades[currentLevel];

            // Verifica se o jogador tem recursos suficientes para comprar o upgrade
            if (resourceMagnet.totalResourcesInStorage >= currentUpgrade.upgradeCost)
            {
                // Gasta os recursos necessários para o upgrade
                resourceMagnet.totalResourcesInStorage -= currentUpgrade.upgradeCost;

                // Atualiza o nível do upgrade
                currentLevel++;

                // SFX da Compra
                FindObjectOfType<AudioManager>().Play("PurchasedUpgrade");

                // Atualiza o texto do botão
                UpdateButtonText();

                // Realiza as ações do upgrade
                UpgradeVariable();
            }
        }
    }

    public void UpgradeVariable()
    {
        UpgradeInfo currentUpgrade = upgrades[currentLevel];
        switch (upgradeName)
        {
            //ShipA
            case "Bullet Damage":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.damage = currentUpgrade.currentUpgrade;
                break;
            case "Bullet Size":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.size = currentUpgrade.currentUpgrade;
                break;
            case "Bullet Speed":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.speed = currentUpgrade.currentUpgrade;
                break;
            case "Shoot Interval":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.shootInterval = currentUpgrade.currentUpgrade;
                break;
            case "Shoot Formation":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.shootType = Mathf.RoundToInt(currentUpgrade.currentUpgrade);
                break;


            //ShipB
            case "Speed":
                shipBMovement = ShipB.GetComponent<ShipBMovement>();
                shipBMovement.velocity = currentUpgrade.currentUpgrade;
                break;
            case "Magnet Attaction":
                resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
                resourceMagnet.attractionForce = currentUpgrade.currentUpgrade;
                break;
            case "Magnet Range":
                resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
                resourceMagnet.attractionRadius = currentUpgrade.currentUpgrade;
                break;
            case "Max Cargo":
                resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
                resourceMagnet.maxResourcesInStorage = currentUpgrade.currentUpgrade;
                break;

            //Distance Joint 2D
            case "Cable Length":
                distanceJoint2D = ShipA.GetComponent<DistanceJoint2D>();
                distanceJoint2D.distance = (float)currentUpgrade.currentUpgrade;
                break;

            //Spawner
            case "More Enemies":
                asteroidSpawner = Spawner.GetComponent<AsteroidSpawner>();
                asteroidSpawner.maxEnemiesAlive = Mathf.RoundToInt(currentUpgrade.currentUpgrade);
                break;

            //Camera
            case "Camera Size":
                mainCamera = Camera.main;
                mainCamera.orthographicSize = currentUpgrade.currentUpgrade;
                break;

            //Extra Life
            case "Extra Life":
                lifeManager = GameObject.Find("LifeManager");
                life = lifeManager.GetComponent<LifeManager>();
                life.life = (int)currentUpgrade.currentUpgrade;
                break;

            default:
                Debug.Log("Upgrade not found.");
                break;
        }
    }
}

[System.Serializable]
public class UpgradeInfo
{
    public string itemName; // Nome do item do upgrade
    public float upgradeCost; // Custo do upgrade
    public float currentUpgrade; // Valor atual do upgrade
    public float nextUpgrade; // Próximo valor do upgrade
}


/*
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public string upgradeName;

    public UpgradeInfo[] upgrades; // Array de informações de cada upgrade

    private Button button;
    private TextMeshProUGUI buttonText;

    public GameObject ShipA;
    private ShotManager shotManager; //Bullet Damage (damage) - Bullet Size (size) - Bullet Speed (speed) - Shoot Interval (Shoot Interval) - Shoot Formation (Shoot Type)
    private DistanceJoint2D distanceJoint2D;

    public GameObject ShipB;
    private ResourceMagnet resourceMagnet; //Magnet Attaction (attractionForce) - Magnet Range (attractionRadius) - Max Cargo (maxResourcesInStorage)
    private ShipBMovement shipBMovement; //Speed (Velocity)

    public GameObject lifeManager;
    private LifeManager life; //Extra Life (life)

    public GameObject Spawner;
    private AsteroidSpawner asteroidSpawner; //More Enemies (MaxEnemyAlive) 

    private Camera mainCamera;

    private int currentLevel = 0; // Nível atual do upgrade

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // Registra a função OnButtonClick() para o evento de clique do botão
        button.onClick.AddListener(OnButtonClick);
    }

    private void Start()
    {
        // Atualiza o texto do botão com base no nível atual do upgrade
        UpdateButtonText();

        // Obtém a referência do script ResourceMagnet no objeto ShipB
        resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
    }

    private void UpdateButtonText()
    {
        // Verifica se o upgrade atingiu o nível máximo
        if (currentLevel >= upgrades.Length - 1)
        {
            // Desativa o botão e mostra "MAX" no texto
            button.interactable = false;
            buttonText.text = string.Format("{0} (MAX)\nCurrent: {1}", upgrades[currentLevel].itemName, upgrades[currentLevel].currentUpgrade);
        }
        else
        {
            // Atualiza o texto do botão com base no nível atual e próximo
            UpgradeInfo currentUpgrade = upgrades[currentLevel];
            
            string buttonTextString = string.Format("{0} (${1})\nCurrent: {2} | Next: {3}",
                currentUpgrade.itemName, currentUpgrade.upgradeCost, currentUpgrade.currentUpgrade, currentUpgrade.nextUpgrade);

            buttonText.text = buttonTextString;
        }
    }

    private void OnButtonClick()
    {
        // Verifica se o upgrade não atingiu o nível máximo
        if (currentLevel < upgrades.Length - 1)
        {
            UpgradeInfo currentUpgrade = upgrades[currentLevel];

            lifeManager = GameObject.Find("LifeManager");
            life = lifeManager.GetComponent<LifeManager>();

            if (life.life == 0)
            {
                // Verifica se o jogador tem recursos suficientes para comprar o upgrade
                if (resourceMagnet.totalResourcesInStorage >= currentUpgrade.upgradeCost)
                {
                    // Gasta os recursos necessários para o upgrade
                    resourceMagnet.totalResourcesInStorage -= currentUpgrade.upgradeCost;

                    // Atualiza o nível do upgrade
                    currentLevel++;

                    // SFX da Compra
                    FindObjectOfType<AudioManager>().Play("PurchasedUpgrade");

                    // Atualiza o texto do botão
                    UpdateButtonText();

                    // Realiza as ações do upgrade
                    UpgradeVariable();
                }
                else
                {
                    // SFX da Compra Falhada
                    FindObjectOfType<AudioManager>().Play("DeclinedUpgrade");
                }
            }
        }
    }

    public void UpgradeVariable()
    {
        UpgradeInfo currentUpgrade = upgrades[currentLevel];
        switch (upgradeName)
        {
            //ShipA
            case "Bullet Damage":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.damage = currentUpgrade.currentUpgrade;
                break;
            case "Bullet Size":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.size = currentUpgrade.currentUpgrade;
                break;
            case "Bullet Speed":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.speed = currentUpgrade.currentUpgrade;
                break;
            case "Shoot Interval":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.shootInterval = currentUpgrade.currentUpgrade;
                break;
            case "Shoot Formation":
                shotManager = ShipA.GetComponent<ShotManager>();
                shotManager.shootType = Mathf.RoundToInt(currentUpgrade.currentUpgrade);
                break;


            //ShipB
            case "Speed":
                shipBMovement = ShipB.GetComponent<ShipBMovement>();
                shipBMovement.velocity = currentUpgrade.currentUpgrade;
                break;
            case "Magnet Attaction":
                resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
                resourceMagnet.attractionForce = currentUpgrade.currentUpgrade;
                break;
            case "Magnet Range":
                resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
                resourceMagnet.attractionRadius = currentUpgrade.currentUpgrade;
                break;
            case "Max Cargo":
                resourceMagnet = ShipB.GetComponent<ResourceMagnet>();
                resourceMagnet.maxResourcesInStorage = currentUpgrade.currentUpgrade;
                break;

            //Distance Joint 2D
            case "Cable Length":
                distanceJoint2D = ShipA.GetComponent<DistanceJoint2D>();
                distanceJoint2D.distance = (float)currentUpgrade.currentUpgrade;
                break;

            //Spawner
            case "More Enemies":
                asteroidSpawner = Spawner.GetComponent<AsteroidSpawner>();
                asteroidSpawner.maxEnemiesAlive = Mathf.RoundToInt(currentUpgrade.currentUpgrade);
                break;

            //Camera
            case "Camera Size":
                mainCamera = Camera.main;
                mainCamera.orthographicSize = currentUpgrade.currentUpgrade;
                break;

            //Extra Life
            case "Extra Life":
                lifeManager = GameObject.Find("LifeManager");
                life = lifeManager.GetComponent<LifeManager>();
                life.life = (int)currentUpgrade.currentUpgrade;
                break;

            default:
                Debug.Log("Upgrade not found.");
                break;
        }
    }
}

[System.Serializable]
public class UpgradeInfo
{
    public string itemName; // Nome do item do upgrade
    public float upgradeCost; // Custo do upgrade
    public float currentUpgrade; // Valor atual do upgrade
    public float nextUpgrade; // Próximo valor do upgrade
}
*/
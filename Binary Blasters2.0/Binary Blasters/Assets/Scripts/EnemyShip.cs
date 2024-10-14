using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShip : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public TextMeshProUGUI healthText;

    public float size = 1f;
    public float minSize = 0.35f;
    public float maxSize = 1.65f;
    public float movementSpeed = 50f;
    public float maxHealth = 100f;
    public float currentHealth;
    public float dropQuality = 1f;

    bool dead = false;

    public Transform spaceStation;

    public float dropQualityMultiplier;
    public float distanceMultiplier = 1f;

    public GameObject enemyBullet;
    public GameObject resourcePrefab;

    public float bulletSpeed;
    public float bulletSize;
    public float initialShootInterval = 3f;
    public float rapidShootInterval = 0.25f;
    public float pauseInterval = 3f;
    private float shootTimer = 0f;
    private int rapidShootCount = 0;
    private bool waitingToShoot = false;
    
    private bool isVisible = false; // Variável para controlar a visibilidade do inimigo
    private bool startShooting = false; // Variável para controlar quando começar a atirar

    public float shrinkSpeed = 3f; // Velocidade de diminuição do tamanho
    bool shrinking = false;

    public GameObject explosionParticle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;

        float distance = Vector2.Distance(transform.position, spaceStation.position);
        maxHealth += distanceMultiplier * distance;
        dropQuality += dropQualityMultiplier * distance;

        currentHealth = maxHealth;
    }

    void Update()
    {
        FollowPlayer();
        AimAtPlayer();

        if (isVisible && startShooting) // Verifica se o inimigo está visível e pode começar a atirar
        {
            ShootPlayer();
        }

        if (shrinking && !dead)
        {
            // Reduz o tamanho do asteroide rapidamente
            size = Mathf.MoveTowards(size, 0.1f, shrinkSpeed * Time.deltaTime);
            transform.localScale = Vector3.one * size;

            // Quando o tamanho atinge 0.1f, o asteroide "morre"
            if (size <= 0.1f)
            {
                size = 0.1f;
                isDying();
            }
        }
    }
    
    // Função chamada quando o objeto se torna visível para a câmera
    private void OnBecameVisible()
    {
        isVisible = true;

        // Inicia um timer para começar a atirar após um intervalo
        Invoke("StartShooting", initialShootInterval);
    }

    // Função chamada quando o objeto se torna invisível para a câmera
    private void OnBecameInvisible()
    {
        isVisible = false;
        startShooting = false;
    }

public void FollowPlayer()
{
    GameObject shipA = GameObject.Find("ShipA");
    if (shipA != null)
    {
        Vector2 direction = shipA.transform.position - transform.position;
        rigidbody.velocity = direction.normalized * movementSpeed;
        UpdateHealthText();
    }
    else
    {
        Debug.LogError("ShipA object not found in the hierarchy!");
    }
}


    public void AimAtPlayer()
    {
        GameObject shipA = GameObject.Find("ShipA");
        if (shipA != null)
        {
            Vector2 direction = (shipA.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f;
            rigidbody.rotation = angle;
        }
        else
        {
            Debug.LogError("ShipA object not found in the hierarchy!");
        }
    }

    public void ShootPlayer()
    {
        shootTimer += Time.deltaTime;

        if (!waitingToShoot)
        {
            if (rapidShootCount < 3)
            {
                if (shootTimer >= rapidShootInterval)
                {
                    FindObjectOfType<AudioManager>().Play("EnemyShoot"); //SFX tiro inimigo
                    GameObject bulletObject = Instantiate(enemyBullet, transform.position, transform.rotation);
                    Bullet bullet = bulletObject.GetComponent<Bullet>();
                    if (bullet != null)
                    {
                        Vector2 direction = (GameObject.Find("ShipA").transform.position - transform.position).normalized;
                        bullet.Project(direction, bulletSpeed, 0f, bulletSize);
                    }

                    rapidShootCount++;
                    shootTimer = 0f;
                }
            }
            else
            {
                waitingToShoot = true;
                shootTimer = 0f;
            }
        }
        else
        {
            if (shootTimer >= pauseInterval)
            {
                rapidShootCount = 0;
                waitingToShoot = false;
                shootTimer = 0f;
            }
        }
    }

    private void StartShooting()
    {
        startShooting = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                float damage = bullet.damage;
                DecreaseHealth(damage);
                GetRedAfterDamage();
            }

            Destroy(other.gameObject);
        }
    }
    public void DecreaseHealth(float damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("EnemyGotShoot");

        if (currentHealth <= 0 && !dead)
        {
            shrinking = true; // Começa a diminuir o tamanho
        }
    }

    public void GetRedAfterDamage()
    {
        // Altera a cor da sprite para vermelho
        spriteRenderer.color = Color.red;

        // Chama o outro método após um certo tempo
        Invoke(nameof(ResetToWhiteColorAfterDamage), 0.04f);
    }

    public void ResetToWhiteColorAfterDamage()
    {
        // Volta a cor da sprite para branco
        spriteRenderer.color = Color.white;
    }

public void isDying()
    {
        //Instancia explosão de particula
        Instantiate(explosionParticle, transform.position, transform.rotation);

        // SFX da Morte
        FindObjectOfType<AudioManager>().Play("KilledAsteroid");

        dead = true;
        ResourceGenerator();
        ScoreGenerator();
        FindObjectOfType<AudioManager>().Play("KilledEnemyShip"); //SFX nave inimiga morrendo
        Destroy(gameObject);
    }

    private void ScoreGenerator()
    {
        ScoreManager.Instance.Score += Mathf.RoundToInt(maxHealth);
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = Mathf.CeilToInt(currentHealth).ToString();
            healthText.transform.rotation = Quaternion.identity;
        }
    }

    private void ResourceGenerator()
    {
        int resourceCount = Random.Range(3, 5);

        for (int i = 0; i < resourceCount; i++)
        {
            CreateResource();
        }
    }

    private void CreateResource()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * Random.Range(1f, 3f);

        GameObject resourceObject = Instantiate(resourcePrefab, position, Quaternion.identity);
        ResourceValue resourceValue = resourceObject.GetComponent<ResourceValue>();
        if (resourceValue != null)
        {
            float value = dropQuality * Random.Range(1.5f, 2f);
            resourceValue.ResourceValuePerUnit = value;
        }
    }
}

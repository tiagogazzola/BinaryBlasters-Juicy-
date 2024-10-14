using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;
    public TextMeshProUGUI healthText; // Referência ao componente TextMeshProUGUI para exibir a vida

    public float size = 1f;
    public float minSize = 0.35f;
    public float maxSize = 1.65f;
    public float movementSpeed = 50f;
    public float maxHealth = 100f;
    public float currentHealth;
    public float dropQuality = 1f;

    public GameObject explosionParticle;


    bool dead = false;

    public float shrinkSpeed = 3f; // Velocidade de diminuição do tamanho
    bool shrinking = false;

    // Space Station
    public Transform spaceStation;

    //public float maxHealthMultiplier;
    public float dropQualityMultiplier;
    public float distanceMultiplier = 1f;

    public GameObject asteroidSplitPrefab; // Prefab do AsteroidSplit
    public GameObject resourcePrefab; // Prefab do Resource

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Assign random properties to make each asteroid feel unique
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

        // Set the scale and mass of the asteroid based on the assigned size so
        // the physics is more realistic
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;

        // Calculate the distance between the asteroid and the space station
        float distance = Vector2.Distance(transform.position, spaceStation.position);

        // Increase maxHealth / dropQuality based on the distance from SpaceStation
        maxHealth += distanceMultiplier * distance;
        dropQuality += dropQualityMultiplier * distance;

        // Definindo a vida atual
        currentHealth = maxHealth;

        UpdateHealthText();
    }

    void Update()
    {
        // Calculate the distance between the asteroid and the space station
        float distance = Vector2.Distance(transform.position, spaceStation.position);

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

    public void SetTrajectory(Vector2 direction)
    {
        // The asteroid only needs a force to be added once since they have no
        // drag to make them stop moving
        rigidbody.AddForce(direction * movementSpeed);
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

            // Destroy the bullet
            Destroy(other.gameObject);
        }
    }

    public void DecreaseHealth(float damage)
    {
        // Reduce the health of the asteroid based on the damage
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("EnemyGotShoot");

        if (currentHealth <= 0 && !dead)
        {
            shrinking = true; // Começa a diminuir o tamanho
        }

        UpdateHealthText();
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
        dead = true;

        // Check if the asteroid is larger than the double of minSize
        if (size > 2 * minSize) // Verifica se o método CreateSplit ainda não foi chamado
        {
            CreateSplit();
            CreateSplit();
        }

        ResourceGenerator();
        ScoreGenerator();

        // SFX da Morte
        FindObjectOfType<AudioManager>().Play("KilledAsteroid");

        //Instancia explosão de particula
        Instantiate(explosionParticle, transform.position, transform.rotation);

        // Destroy the asteroid
        Destroy(gameObject);
    }

    private void CreateSplit()
    {
        // Set the new asteroid position to be the same as the current asteroid
        // but with a slight offset so they do not spawn inside each other
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        // Create the new asteroid split with reduced size and adjusted maxHealth and dropQuality
        GameObject asteroidSplitObject = Instantiate(asteroidSplitPrefab, position, transform.rotation);
        AsteroidSplit asteroidSplit = asteroidSplitObject.GetComponent<AsteroidSplit>();
        if (asteroidSplit != null)
        {
            asteroidSplit.size = size * 0.5f;
            asteroidSplit.maxHealth = maxHealth * 0.33f;
            asteroidSplit.currentHealth = asteroidSplit.maxHealth;
            asteroidSplit.dropQuality = dropQuality * 0.66f;
            asteroidSplit.SetTrajectory(Random.insideUnitCircle.normalized);
        }
    }

    private void ScoreGenerator()
    {
        ScoreManager.Instance.Score += Mathf.RoundToInt(maxHealth);
    }

    private void UpdateHealthText()
    {
        // Atualiza o texto da vida com a informação atual
        if (healthText != null)
        {
            healthText.text = Mathf.CeilToInt(currentHealth).ToString();
            healthText.transform.rotation = Quaternion.identity;
        }
    }

    private void ResourceGenerator()
    {
        int resourceCount = Random.Range(3, 5); // Número aleatório de recursos (2 a 5)

        for (int i = 0; i < resourceCount; i++)
        {
            CreateResource();
        }
    }

    private void CreateResource()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * Random.Range(1f, 3f); // Posição aleatória dentro de um círculo de raio aleatório entre 1 e 3

        GameObject resourceObject = Instantiate(resourcePrefab, position, Quaternion.identity);
        ResourceValue resourceValue = resourceObject.GetComponent<ResourceValue>();
        if (resourceValue != null)
        {
            float value = dropQuality * Random.Range(1.5f, 2f); // Valor do recurso baseado no dropQuality multiplicado por um fator aleatório
            resourceValue.ResourceValuePerUnit = value;
        }
    }
}

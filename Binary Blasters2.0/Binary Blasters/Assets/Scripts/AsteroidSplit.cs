using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidSplit : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;

    public float size = 0.5f;
    public float maxHealth = 33.3f;
    public float currentHealth;
    public float dropQuality = 0.333f;

    public float movementSpeed = 50f;

    bool dead = false;

    public TextMeshProUGUI healthText;

    public GameObject resourcePrefab; // Prefab do Resource

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Assign random properties to make each asteroid split feel unique
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

        // Set the scale and mass of the asteroid split based on the assigned size
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;

        currentHealth = maxHealth;

        SetTrajectory(Random.insideUnitCircle.normalized);

        UpdateHealthText();
    }

    public void SetTrajectory(Vector2 direction)
    {
        // The asteroid split only needs a force to be added once since they have no
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
            }

            // Destroy the bullet
            Destroy(other.gameObject);
        }
    }

    public void DecreaseHealth(float damage)
    {
        // Reduce the health of the asteroid based on the damage
        currentHealth -= damage;

        // Check if the asteroid's already dying
        if (currentHealth <= 0 && !dead) { isDying(); }

        UpdateHealthText();
    }

    public void isDying()
    {
        dead = true;

        ResourceGenerator();
        ScoreGenerator();

        //SFX da Morte
        FindObjectOfType<AudioManager>().Play("KilledAsteroid");
    
        // Destroy the asteroid
        Destroy(gameObject);
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
}

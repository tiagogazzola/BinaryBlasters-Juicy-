using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }

    public float damage;
    public float size;

    // Referência ao ScreenShake2D
    private ScreenShake2D screenShake;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        // Busca o componente ScreenShake2D na cena
        screenShake = FindObjectOfType<ScreenShake2D>();
    }

    private void Update()
    {
        // Obtenha a posição da bala nas coordenadas da viewport
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Se a bala estiver fora da viewport, destrua-a
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject);
        }
    }

    public void Project(Vector2 direction, float speed, float damage, float size)
    {
        // Atualize a velocidade, o dano e o tamanho da bala com os valores fornecidos pelo ShotManager
        this.damage = damage;
        this.size = size;
        rigidbody.AddForce(direction * speed);

        // Atualize a escala da bala com base no tamanho
        transform.localScale = Vector3.one * size;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifique se a colisão é com um asteroide
        Asteroid asteroid = other.gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            asteroid.DecreaseHealth(damage);
            // Ativa o screen shake
            if (screenShake != null)
            {
                screenShake.intensity = 1f;  // Define a intensidade do shake ao máximo
            }
        }

        // Verifique se a colisão é com um asteroide
        EnemyShip enemyship = other.gameObject.GetComponent<EnemyShip>();
        if (enemyship != null)
        {
            enemyship.DecreaseHealth(damage);
            // Ativa o screen shake
            if (screenShake != null)
            {
                screenShake.intensity = 1f;  // Define a intensidade do shake ao máximo
            }
        }

        // Destrua a bala assim que ela colidir com qualquer coisa
        Destroy(gameObject);
    }
}

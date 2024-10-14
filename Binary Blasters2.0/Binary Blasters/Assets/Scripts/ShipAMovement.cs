using UnityEngine;

public class ShipAMovement : MonoBehaviour
{
    public float velocity;

    private GameObject closestEnemy;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Desativa a rotação após colisões
    }

    void Update()
    {
        HandleMovement();
        FindClosestEnemy();
        HandleRotation();
    }

    void HandleMovement()
    {
        // Movimento da nave A com as teclas WASD
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Movimento da nave A em relação aos eixos globais
        Vector2 movement = new Vector2(moveHorizontal, moveVertical) * velocity;
        rb.velocity = movement;
    }

    void FindClosestEnemy()
    {
        // Encontrar o inimigo mais próximo
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        closestEnemy = null; // Reinicia o inimigo mais próximo

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
    }

    void HandleRotation()
    {
        if (closestEnemy != null)
        {
            RotateTowardsEnemy();
        }
        else
        {
            RotateTowardsInput();
        }
    }

    void RotateTowardsEnemy()
    {
        // Rotação da nave A para olhar para o inimigo mais próximo
        Vector2 direction = closestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    void RotateTowardsInput()
    {
        // Rotação da nave A para apontar na direção do input do WASD
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }

    public float GetVelocity()
    {
        return rb.velocity.magnitude;
    }

    public void SetVelocity(float velocity)
    {
        rb.velocity = rb.velocity.normalized * velocity;
    }
}

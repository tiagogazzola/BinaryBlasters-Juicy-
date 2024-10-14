using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBMovement : MonoBehaviour
{
    public float velocity;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Desativa a rotação após colisões
    }

    void Update()
    {
        HandleMovement();
        RotateTowardsInput();
    }

    void HandleMovement()
    {
        // Movimento da nave B com as setas do teclado
        float moveHorizontal = Input.GetAxis("Horizontal2");
        float moveVertical = Input.GetAxis("Vertical2");

        // Movimento da nave B em relação aos eixos globais
        Vector2 movement = new Vector2(moveHorizontal, moveVertical) * velocity;
        rb.velocity = movement;
    }

    void RotateTowardsInput()
    {
        // Rotação da nave B para apontar na direção do input do teclado
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
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
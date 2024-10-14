using UnityEngine;

public class ShieldCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se a colisão é com um inimigo
        if (collision.CompareTag("Enemy"))
        {
            FindObjectOfType<AudioManager>().Play("ShieldShipB"); //SFX Destroy Enemy
            // Destroi o inimigo instantaneamente
            Destroy(collision.gameObject);
        }
    }
}

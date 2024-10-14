using UnityEngine;

public class ShieldZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que colidiu possui a tag "Enemy"
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            // Destroi o inimigo após um pequeno atraso
            FindObjectOfType<AudioManager>().Play("ShieldSpaceStation"); //SFX Destroy Enemy
            Destroy(collision.gameObject, 0);
        }
    }
}

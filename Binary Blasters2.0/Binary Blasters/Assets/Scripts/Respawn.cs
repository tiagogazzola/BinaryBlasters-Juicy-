using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject ThisShip; // Objeto a ser teleportado (defina no Inspector)
    public GameObject OtherShip; // Objeto a ser teleportado (defina no Inspector)
    public GameObject spaceStation; // Objeto SpaceStation (defina no Inspector)
    public ResourceMagnet resourceMagnet; // Referência ao componente ResourceMagnet no objeto "ShipB"
    private LifeManager lifeManager; // Referência ao script LifeManager

    private void Awake()
    {
        lifeManager = LifeManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || (other.CompareTag("EnemyBullet")) && other.gameObject.layer != LayerMask.NameToLayer("Shield"))
        {
            if (lifeManager.life == 0)
            {
                // Jogador morre
                TeleportObjectToSpaceStation();
                ResetTotalResourcesInStorage();

                //SFX da Morte
                FindObjectOfType<AudioManager>().Play("PlayerDeath");
            }
            else if (lifeManager.life > 0)
            {
                // Jogador perde uma vida
                LoseExtraLife();
            }
        }
    }

    private void LoseExtraLife()
    {
        lifeManager.life -= 1;
    }

    private void TeleportObjectToSpaceStation()
    {
        ThisShip.transform.position = spaceStation.transform.position; // Teleporta essa nave para a posição da SpaceStation
        OtherShip.transform.position = spaceStation.transform.position; // Teleporta a outra nave para a posição da SpaceStation
    }

    private void ResetTotalResourcesInStorage()
    {
        resourceMagnet.totalResourcesInStorage = 0f; // Define a variável totalResourcesInStorage como 0
    }
}

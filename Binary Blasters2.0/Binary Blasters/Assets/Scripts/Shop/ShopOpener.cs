using UnityEngine;

public class ShopOpener : MonoBehaviour
{
    public GameObject shopCanvas; // Referência ao GameObject que contém o Canvas da loja
    public GameObject pressEtoShop; // Referência ao GameObject do botão "[E] to Shop"
    public GameObject ShipA;
    public GameObject ShipB;

    private ShipAMovement shipAMovement; // Referência ao componente ShipAMovement da ShipA
    private ShipBMovement shipBMovement; // Referência ao componente ShipBMovement da ShipB

    private float shipASavedVelocity; // Velocidade salva da ShipA
    private float shipBSavedVelocity; // Velocidade salva da ShipB

    private void Update()
    {
        // Verifica se o jogador está na trigger e pressionou a tecla E
        bool isPlayerInTrigger = IsPlayerInTrigger();
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger)
        {
            ToggleShop();
        }

        // Ativa ou desativa o botão "[E] to Shop" com base na presença do jogador na trigger e no estado da loja
        pressEtoShop.SetActive(isPlayerInTrigger && !shopCanvas.activeSelf);
    }

    private bool IsPlayerInTrigger()
    {
        // Obtém todos os colliders que entraram na trigger
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);

        // Verifica se algum dos colliders é o jogador
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void ToggleShop()
    {
        // SFX quando abre/fecha a loja
        FindObjectOfType<AudioManager>().Play("ShopToggle");

        // Ativa ou desativa o Canvas da loja
        bool shopActive = !shopCanvas.activeSelf;
        shopCanvas.SetActive(shopActive);

        // Obtém o componente ShipAMovement da ShipA
        ShipAMovement shipAMovement = ShipA.GetComponent<ShipAMovement>();
        // Ativa ou desativa o script ShipAMovement
        shipAMovement.enabled = !shopActive;

        if (shopActive)
        {
            // Salva a velocidade atual da ShipA
            shipASavedVelocity = shipAMovement.GetVelocity();
            // Define a velocidade da ShipA como zero para parar o movimento
            shipAMovement.SetVelocity(0f);
        }
        else
        {
            // Restaura a velocidade da ShipA
            shipAMovement.SetVelocity(shipASavedVelocity);
        }

        // Ativa ou desativa o script ShipBMovement
        ShipBMovement shipBMovement = ShipB.GetComponent<ShipBMovement>();
        shipBMovement.enabled = !shopActive;

        if (shopActive)
        {
            // Salva a velocidade atual da ShipB
            shipBSavedVelocity = shipBMovement.GetVelocity();
            // Define a velocidade da ShipB como zero para parar o movimento
            shipBMovement.SetVelocity(0f);
        }
        else
        {
            // Restaura a velocidade da ShipB
            shipBMovement.SetVelocity(shipBSavedVelocity);
        }

        // Ativa ou desativa o cursor do mouse com base no estado da loja
        Cursor.visible = shopActive;
    }
}

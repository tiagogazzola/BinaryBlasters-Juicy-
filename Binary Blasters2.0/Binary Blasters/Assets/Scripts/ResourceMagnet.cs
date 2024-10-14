using UnityEngine;

public class ResourceMagnet : MonoBehaviour
{
    public float attractionForce = 10f;
    public float attractionRadius = 5f;
    public float destructionRadius = 4f; // Tamanho da área de destruição
    public float totalResourcesInStorage;
    public float maxResourcesInStorage = 100f; // Limite máximo de recursos no storage

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Resources"))
            {
                ResourceValue resource = collider.GetComponent<ResourceValue>();
                if (resource != null)
                {
                    Vector2 direction = transform.position - collider.transform.position;
                    Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(direction.normalized * attractionForce);

                        // Verifica se a distância é menor que o raio de atração
                        if (direction.magnitude < attractionRadius)
                        {
                            // Verifica se a distância é menor que o raio de destruição
                            if (direction.magnitude < destructionRadius)
                            {
                                // Verifica se o limite máximo de recursos no storage não foi atingido
                                if (totalResourcesInStorage < maxResourcesInStorage)
                                {
                                    totalResourcesInStorage += resource.resourceValue;
                                    FindObjectOfType<AudioManager>().Play("ColectResource"); //SFX Coletou Recurso
                                    Destroy(collider.gameObject);
                                }
                                else
                                {
                                    FindObjectOfType<AudioManager>().Play("ResourcesAtMax"); //SFX Recurso no Maximo
                                }
                            }
                        }
                    }
                }
            }
        }

        // Verifica se o armazenamento ultrapassou o limite máximo
        if (totalResourcesInStorage > maxResourcesInStorage)
        {
            // Arredonda o valor para o limite máximo
            totalResourcesInStorage = maxResourcesInStorage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha a área de atração
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);

        // Desenha a área de destruição
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destructionRadius);
    }
}

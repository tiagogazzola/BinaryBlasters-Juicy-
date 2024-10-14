using UnityEngine;

public class ResourceFade : MonoBehaviour
{
    public float shrinkSpeed = 1f; // Velocidade de diminuição da escala
    public float timeToShrink = 2f; // Tempo em segundos para o objeto começar a diminuir
    public float minScale = 0.1f; // Tamanho mínimo da escala antes de destruir o objeto
    public float easingExponent = 0.5f; // Exponente para controlar o easing (inverso)

    private float timer = 0f;
    private bool isShrinking = false;
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer; // Para controlar o alpha da sprite

    private void Start()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Pega o SpriteRenderer do objeto
        timer = 0f;
        isShrinking = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToShrink)
        {
            // Começa a diminuir a escala do objeto
            if (!isShrinking)
            {
                isShrinking = true;
            }

            // Calcula o progresso para o easing (diminui rapidamente no início e depois mais lento)
            float progress = (timer - timeToShrink) * shrinkSpeed;
            float easedProgress = 1 - Mathf.Pow(1 - Mathf.Clamp01(progress), easingExponent); // Easing inverso
            float scaleValue = Mathf.Lerp(1f, minScale, easedProgress);

            // Aplica a nova escala
            transform.localScale = originalScale * scaleValue;

            // Calcula e ajusta o alpha com base no mesmo easing
            if (spriteRenderer != null)
            {
                float alphaValue = Mathf.Lerp(1f, 0.3f, easedProgress);
                SetAlpha(alphaValue);
            }

            // Destrói o objeto se a escala atingir o valor mínimo
            if (scaleValue <= minScale + 0.02f)
            {
                Destroy(gameObject);
            }
        }
    }

    // Função para ajustar o alpha da sprite
    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}

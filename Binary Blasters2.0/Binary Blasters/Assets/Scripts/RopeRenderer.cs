using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public Transform startPoint; // Ponto de ancoragem inicial da corda
    public Transform endPoint; // Ponto de ancoragem final da corda
    public float scrollSpeed = 0.5f;

    private LineRenderer lineRenderer;
    private Material material;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        material = lineRenderer.material;
    }

    private void Update()
    {
        // Atualiza a posição dos pontos inicial e final do LineRenderer
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

        // Aplica o scrolling da textura
        float offset = Time.time * scrollSpeed;
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}

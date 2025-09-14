using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimacionAgua : MonoBehaviour
{
    private SpriteRenderer sr;
    private float tiempo;

    [Header("Colores animados")]
    public Color color1 = new Color(0f, 0.5f, 1f, 0.6f);
    public Color color2 = new Color(0f, 0.7f, 1f, 0.6f);
    public float velocidad = 2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        tiempo += Time.deltaTime * velocidad;
        sr.color = Color.Lerp(color1, color2, (Mathf.Sin(tiempo) + 1f) / 2f);
    }
}
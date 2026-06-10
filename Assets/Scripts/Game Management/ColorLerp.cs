using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float lerpDuration = 1f;

    void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError($"{gameObject.name}: SpriteRenderer not found");
            enabled = false;
        }
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time / lerpDuration, 1f);
        spriteRenderer.color = Color.Lerp(startColor, endColor, t);
    }
}
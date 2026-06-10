using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    void Awake()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 screenPos = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            null,
            out Vector2 localPos
        );

        rectTransform.anchoredPosition = localPos;
    }
}
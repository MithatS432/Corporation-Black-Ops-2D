using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;

    public AudioClip cursorSound;
    private AudioSource audioSource;

    void Start()
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("CursorManager: cursorTexture atanmamış!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cursorSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(cursorSound);
            }
        }
    }
}
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera cam;

    public Vector2 MoveDirection { get; private set; }
    public Vector2 MouseWorldPosition { get; private set; }
    public bool AbilityPressed { get; private set; }
    public bool IsShooting { get; private set; }


    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        MoveDirection =
            new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;

        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(cam.transform.position.z);

        MouseWorldPosition = cam.ScreenToWorldPoint(mouse);

        AbilityPressed = Input.GetKeyDown(KeyCode.Space);
        IsShooting = Input.GetMouseButtonDown(0);
    }
}
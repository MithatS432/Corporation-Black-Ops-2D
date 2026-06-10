using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 5f;
    [SerializeField] private float closeDelay = 2f;

    private bool isOpen;
    private Quaternion closedRot;
    private Quaternion openRot;
    private float closeTimer;

    void Start()
    {
        closedRot = transform.localRotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        Open(collision.transform.position);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        closeTimer = closeDelay;
    }

    void Open(Vector3 playerPos)
    {
        Vector3 dir =
            (playerPos - transform.position).normalized;

        float dot =
            Vector2.Dot(transform.right, dir);

        float angle =
            dot > 0
                ? -openAngle
                : openAngle;

        openRot =
            closedRot *
            Quaternion.Euler(0, 0, angle);

        isOpen = true;

        closeTimer = closeDelay;
    }

    void Update()
    {
        if (isOpen)
        {
            closeTimer -= Time.deltaTime;

            if (closeTimer <= 0f)
            {
                isOpen = false;
            }
        }

        Quaternion target =
            isOpen
                ? openRot
                : closedRot;

        transform.localRotation =
            Quaternion.Slerp(
                transform.localRotation,
                target,
                Time.deltaTime * openSpeed
            );
    }
}
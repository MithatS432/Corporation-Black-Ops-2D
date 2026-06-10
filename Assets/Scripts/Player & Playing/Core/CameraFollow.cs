using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (target == null)
        {
            target = GameManager.Instance.GetPlayer();
            return;
        }

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime * 60f
        );
    }
}
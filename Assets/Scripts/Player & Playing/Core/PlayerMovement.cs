using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody2D rb;

    [Header("Movement")]
    Vector2 currentDirection;
    [SerializeField] private float moveSpeed = 7f;


    [Header("Player1")]
    private bool isDashing;

    void Awake()
    {
        rb =
            GetComponent<Rigidbody2D>();
    }

    public void SetDirection(
        Vector2 direction)
    {
        currentDirection =
            direction;
    }

    void FixedUpdate()
    {
        if (isDashing)
            return;

        rb.linearVelocity =
            currentDirection *
            moveSpeed;
    }


    #region Player1 State
    public void Dash(
    Vector2 direction,
    float dashForce,
    float dashDuration)
    {
        StartCoroutine(
            DashRoutine(
                direction,
                dashForce,
                dashDuration
            )
        );
    }
    IEnumerator DashRoutine(
    Vector2 direction,
    float dashForce,
    float dashDuration)
    {
        isDashing = true;

        rb.linearVelocity =
            direction * dashForce;

        yield return new WaitForSeconds(
            dashDuration
        );

        rb.linearVelocity = Vector2.zero;

        isDashing = false;
    }
    #endregion
}
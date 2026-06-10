using UnityEngine;
using System.Collections;

public class PlayerController :
MonoBehaviour
{
    [SerializeField]
    PlayerInput input;

    [SerializeField]
    PlayerMovement movement;

    [SerializeField]
    GameManager gameManager;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip walkClip;
    private bool isMoving;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (!gameManager.isGameStarted || gameManager.isGameOver || Time.timeScale == 0f)
        {
            if (isMoving) StopWalking();
            isMoving = false;
            movement.SetDirection(Vector2.zero);
            return;
        }

        Vector2 dir = input.MoveDirection;
        movement.SetDirection(dir);

        HandleFacing(input.MouseWorldPosition);
        HandleFootsteps(dir);

        if (input.AbilityPressed)
        {
            UseSpecial();
        }

        if (input.IsShooting)
        {
            Shoot();
        }
    }
    #region Footsteps
    void HandleFootsteps(Vector2 moveDir)
    {
        bool moving = moveDir.sqrMagnitude > 0.01f;

        if (moving && !isMoving)
        {
            StartWalking();
        }
        else if (!moving && isMoving)
        {
            StopWalking();
        }

        isMoving = moving;
    }

    void StartWalking()
    {
        audioSource.clip = walkClip;
        audioSource.loop = true;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = 1f;

        audioSource.Play();
    }
    void StopWalking()
    {
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= Time.deltaTime * 4f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    void HandleFacing(
 Vector2 targetPosition)
    {
        Vector2 direction =
            targetPosition -
            (Vector2)transform.position;

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            )
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0,
                0,
                angle
            );
    }
    #endregion


    #region Powers
    void UseSpecial()
    {
        AbilitySystem abilitySystem = GetComponent<AbilitySystem>();
        if (abilitySystem != null)
            abilitySystem.Use(this);
        else
            Debug.Log("AbilitySystem bulunamadı!");
    }
    void Shoot()
    {
        Vector2 dir =
            (input.MouseWorldPosition - (Vector2)transform.position).normalized;

        WeaponSystem ws = GetComponent<WeaponSystem>();
        ws.Shoot(transform.position, dir);
    }
    #endregion
}
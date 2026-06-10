using UnityEngine;
using TMPro;
using System.Collections;

public class DashAbility : Ability, IInitializableAbility
{
    [Header("UI")]
    private TMP_Text specialPowerText;

    [Header("Audio")]
    [SerializeField] private AudioClip activeSound;
    [SerializeField] private AudioClip dashSound;

    [Header("VFX")]
    [SerializeField] private ParticleSystem readyEffectPrefab;

    [Header("Settings")]
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool canUse = true;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SetAbilityReady(true);
    }
    public void SetUI(TMP_Text ui)
    {
        specialPowerText = ui;

        specialPowerText.gameObject.SetActive(canUse);
    }
    public override void Activate(PlayerController player)
    {
        if (!canUse)
            return;

        canUse = false;

        if (dashSound)
            audioSource.PlayOneShot(dashSound);

        SetAbilityReady(false);

        PlayerMovement movement =
    player.GetComponent<PlayerMovement>();

        movement.Dash(
            player.transform.right,
            dashDistance,
            dashDuration
        );

        StartCoroutine(
            CooldownRoutine(player)
        );
    }
    private IEnumerator CooldownRoutine(PlayerController player)
    {
        yield return new WaitForSeconds(cooldown);

        canUse = true;

        SetAbilityReady(true);

        if (activeSound)
            audioSource.PlayOneShot(activeSound);

        PlayReadyEffect(player);
    }

    private void SetAbilityReady(bool ready)
    {
        if (specialPowerText != null)
            specialPowerText.gameObject.SetActive(ready);
    }

    private void PlayReadyEffect(PlayerController player)
    {
        if (readyEffectPrefab == null || player == null)
            return;

        ParticleSystem fx = Instantiate(
            readyEffectPrefab,
            player.transform.position,
            Quaternion.identity
        );

        fx.Play();

        Destroy(fx.gameObject, 2f);
    }
}
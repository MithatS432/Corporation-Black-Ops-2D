using UnityEngine;
using TMPro;
using System.Collections;

public class ShieldAbility : Ability, IInitializableAbility
{
    [Header("UI")]
    private TMP_Text specialPowerText;

    [Header("Audio")]
    [SerializeField] private AudioClip activeSound;
    [SerializeField] private AudioClip shieldSound;

    [Header("VFX")]
    [SerializeField] private ParticleSystem readyEffectPrefab;

    [Header("Settings")]
    [SerializeField] private float cooldown = 8f;
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

        Health health = player.GetComponent<Health>();

        if (health != null && health.CurrentHealth >= 2)
        {
            Debug.Log("Can zaten full, yetenek kullanılmadı!");
            return;
        }

        canUse = false;

        if (health != null)
        {
            health.AddHealth(1);
            Debug.Log("Shield/Health Ability used on: " + player.name);
        }

        if (shieldSound)
            audioSource.PlayOneShot(shieldSound);

        SetAbilityReady(false);

        StartCoroutine(CooldownRoutine(player));
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
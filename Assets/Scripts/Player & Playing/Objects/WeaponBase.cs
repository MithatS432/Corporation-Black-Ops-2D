using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public int maxAmmo = 15;
    public int currentAmmo;

    [Header("Audio")]
    [SerializeField] protected AudioClip shootSound;
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        currentAmmo = maxAmmo;

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    protected void PlayShootSound()
    {
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    public abstract void Shoot(Vector2 origin, Vector2 direction);

    public bool CanShoot()
    {
        return currentAmmo > 0;
    }
}
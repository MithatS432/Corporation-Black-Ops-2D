using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private WeaponSystem weaponSystem;

    void Start()
    {
        if (weaponSystem == null)
            weaponSystem = FindFirstObjectByType<WeaponSystem>();

        if (weaponSystem != null)
        {
            weaponSystem.OnAmmoChanged += UpdateAmmoUI;

            if (weaponSystem.CurrentWeapon != null)
                UpdateAmmoUI(weaponSystem.CurrentWeapon.currentAmmo, weaponSystem.CurrentWeapon.maxAmmo);
        }
    }

    void UpdateAmmoUI(int current, int max)
    {
        if (ammoText != null)
            ammoText.text = $"{current} / {max}";
    }

    void OnDestroy()
    {
        if (weaponSystem != null)
            weaponSystem.OnAmmoChanged -= UpdateAmmoUI;
    }
}
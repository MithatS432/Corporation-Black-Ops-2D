using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponHolder;
    private WeaponUIController ui;

    [Header("Starting Weapon")]
    [SerializeField] private WeaponBase startingWeaponPrefab;

    private WeaponBase currentWeapon;
    public WeaponBase CurrentWeapon => currentWeapon;

    public System.Action<int, int> OnAmmoChanged;

    private void Start()
    {
        if (ui == null)
        {
            ui = FindFirstObjectByType<WeaponUIController>();
        }

        if (startingWeaponPrefab != null)
            EquipWeapon(startingWeaponPrefab);
    }

    public void EquipWeapon(WeaponBase weaponPrefab)
    {
        if (weaponPrefab == null) return;

        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);


        currentWeapon = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        string weaponType = currentWeapon.GetType().Name;
        ui.SetActiveWeapon(currentWeapon.name);
        ui.UpdateAmmo(currentWeapon.currentAmmo, currentWeapon.maxAmmo);
        OnAmmoChanged?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo);
    }

    public void Shoot(Vector2 origin, Vector2 direction)
    {
        if (currentWeapon == null) return;

        if (!currentWeapon.CanShoot())
        {
            Debug.Log("Mermin kalmadı!");
            return;
        }

        currentWeapon.Shoot(origin, direction);
        OnAmmoChanged?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo);
        ui.UpdateAmmo(currentWeapon.currentAmmo, currentWeapon.maxAmmo);
    }
}
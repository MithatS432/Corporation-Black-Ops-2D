using UnityEngine;
using TMPro;

public class WeaponUIController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text gunText;
    [SerializeField] private TMP_Text shotgunText;
    [SerializeField] private TMP_Text smgText;

    public void SetActiveWeapon(string weaponName)
    {
        gunText.gameObject.SetActive(weaponName.Contains("Gun"));
        shotgunText.gameObject.SetActive(weaponName.Contains("Shotgun"));
        smgText.gameObject.SetActive(weaponName.Contains("SMG"));
    }

    public void UpdateAmmo(int current, int max)
    {
        if (gunText.gameObject.activeSelf)
            gunText.text = $"{current}/{max}";

        if (shotgunText.gameObject.activeSelf)
            shotgunText.text = $"{current}/{max}";

        if (smgText.gameObject.activeSelf)
            smgText.text = $"{current}/{max}";
    }
}
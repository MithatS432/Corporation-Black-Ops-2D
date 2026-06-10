using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponPrefab;

    public int maxAmmo = 0;
    [SerializeField] private AudioClip pickupSound;

    [SerializeField] private float pickUpRange = 1f;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (Input.GetMouseButton(1) && Vector2.Distance(other.transform.position, transform.position) <= pickUpRange)
        {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);

            WeaponSystem ws = other.GetComponent<WeaponSystem>();

            ws.EquipWeapon(weaponPrefab);

            Destroy(gameObject);
        }
    }
}
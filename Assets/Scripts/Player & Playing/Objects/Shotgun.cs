using UnityEngine;

public class Shotgun : WeaponBase
{
    public int pelletCount = 5;
    public float spreadAngle = 15f;
    [SerializeField] private Bullet bulletPrefab;
    protected override void Awake()
    {
        maxAmmo = 6;
        base.Awake();
    }

    public override void Shoot(Vector2 origin, Vector2 direction)
    {
        if (!CanShoot()) return;

        currentAmmo--;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = Random.Range(-spreadAngle, spreadAngle);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * direction;

            float bulletAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, bulletAngle);

            PlayShootSound();

            Bullet b = Instantiate(bulletPrefab, origin, rotation);

            b.Init(dir);
        }
    }
}
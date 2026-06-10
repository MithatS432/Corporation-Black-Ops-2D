using UnityEngine;

public class Gun : WeaponBase
{
    [SerializeField] private Bullet bulletPrefab;

    protected override void Awake()
    {
        maxAmmo = 15;
        base.Awake();
    }

    public override void Shoot(Vector2 origin, Vector2 direction)
    {
        if (!CanShoot()) return;

        currentAmmo--;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        PlayShootSound();

        Bullet b = Instantiate(bulletPrefab, origin, rotation);

        b.Init(direction);
    }
}
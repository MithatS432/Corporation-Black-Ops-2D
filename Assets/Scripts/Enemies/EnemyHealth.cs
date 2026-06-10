using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;
    public GameObject[] weaponDrops;
    [SerializeField] private GameObject bloodEffectPrefab;

    private bool isDead = false;

    public void GetDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        SpawnBlood();
        DropWeapon();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyKilled(transform.position, 6000);
        }

        if (LevelManager.Instance != null)
            LevelManager.Instance.EnemyKilled();

        Destroy(gameObject);
    }

    void SpawnBlood()
    {
        if (bloodEffectPrefab == null) return;
        Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
    }

    void DropWeapon()
    {
        if (weaponDrops.Length == 0) return;

        float chance = Random.Range(0f, 1f);
        if (chance < 0.5f)
        {
            int index = Random.Range(0, weaponDrops.Length);
            Instantiate(weaponDrops[index], transform.position, Quaternion.identity);
        }
    }
}